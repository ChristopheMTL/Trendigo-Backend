using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using IMS.Utilities.PaymentAPI.Model;
using IMS.Utilities.PaymentAPI.Client;
using System.Web.Http.Cors;
using IMS.Common.Core.Data;
using IMS.Service.WebAPI2.Models;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Services;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.DTO;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Bindings;
using System.Resources;
using System.Collections;
using System.Globalization;
using IMS.Service.WebAPI2.Services;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("members")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class MembersController : ApiController
    {
        private ApplicationUserManager _userManager;
        private IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        #region Constructor Section

        public MembersController()
        {
            
        }

        public MembersController(ApplicationUserManager userManager,
                    ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        #endregion

        #region Member Section

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateMember([FromBody] MemberDTO model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            ApplicationUser user = null;
            Common.Core.Data.Member member = new Common.Core.Data.Member();
            createMemberRS response = new createMemberRS();
            IMS.Common.Core.Data.IMSUser imsUser = new IMS.Common.Core.Data.IMSUser();

            #endregion

            #region Validation Section

            if (model == null || !ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            #endregion

            #region AspNet Section

            try
            {
                user = new ApplicationUser
                {
                    UserName = model.email,
                    Email = model.email
                };

                AspNetUser exist = await db.AspNetUsers.FirstOrDefaultAsync(a => a.UserName == model.email);

                if (exist != null)
                {
                    return Content(HttpStatusCode.NotModified, MessageService.GetMessage("EmailAlreadyUsed_", locale));
                }

                if (string.IsNullOrEmpty(model.password))
                {
                    model.passwordNotSet = true;
                    model.password = "Tr3nd1g0";
                }
                else
                {
                    model.passwordNotSet = false;
                }

                IdentityResult result;
                //Create AspNetUser
                result = await UserManager.CreateAsync(user, model.password);

                if (!result.Succeeded == true)
                {
                    logger.ErrorFormat("CreateASync - UserId {0}", user.Id);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }

                //Add role member to new user
                result = UserManager.AddToRole(user.Id, IMSRole.Member.ToString());

                if (!result.Succeeded == true)
                {
                    logger.ErrorFormat("AddToRole Not Created - UserId {0}", user.Id);
                    UserManager.Delete(user);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }
            }
            catch (Exception ex)
            {
                // Rollback identity user creation
                try
                {
                    UserManager.Delete(user);
                }
                catch { }

                logger.ErrorFormat("Create AspNetUser - Exception {0} InnerException {1} Member info {2}", ex.ToString(), ex.InnerException.ToString(), model.ToString());

                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            #region Social Media Section

            if (!string.IsNullOrEmpty(user.Id) && !string.IsNullOrEmpty(model.uid) && !string.IsNullOrEmpty(model.provider))
            {
                try
                {
                    SocialMediaUser newSocialMediaUser = new SocialMediaManager().CreateSocialMediaUser(user.Id, model.uid, model.provider);
                }
                catch (Exception ex)
                {
                    try
                    {
                        UserManager.Delete(user);
                    }
                    catch{ }

                    logger.ErrorFormat("Create SocialMediaUser - Exception {0} InnerException {1} Member info {2}", ex.ToString(), ex.InnerException.ToString(), model.ToString());
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }
            }

            #endregion

            #region Member Section

            member = Mapper.Map<MemberDTO, Common.Core.Data.Member>(model);

            member.Language = await db.Languages.FirstOrDefaultAsync(x => x.ISO639_1 == model.language);
            member.UserId = user.Id;
            member.AspNetUser = await db.AspNetUsers.Where(a => a.Id == user.Id).FirstOrDefaultAsync();

            Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();
            member.EnterpriseId = enterprise.Id;

            //Add new member
            try
            {
                var registerMemberCmd = DataCommandFactory.AddMemberCommand(member, model.email, model.password, db);

                var cmdResult = await registerMemberCmd.Execute();

                if (cmdResult != DataCommandResult.Success)
                {
                    logger.ErrorFormat("Register Member Command - Result {0} Member info {1}", cmdResult.ToString(), model.ToString());
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    //Delete AspNetUserRole
                    IdentityResult deleteRole_result = UserManager.RemoveFromRole(user.Id, IMSRole.Member.ToString());
                    IHttpActionResult deleteRole_errorResult = GetErrorResult(deleteRole_result);

                    if (deleteRole_errorResult != null)
                    {
                        logger.ErrorFormat("Member Delete Role - Result {0} Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                    }

                    //Delete AspNetUser
                    IdentityResult delete_result = UserManager.Delete(user);
                    IHttpActionResult delete_errorResult = GetErrorResult(delete_result);

                    if (delete_errorResult != null)
                    {
                        logger.ErrorFormat("User Manager Delete - Result {0} Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                    }
                }

                logger.ErrorFormat("Add Member Command - Exception {0} InnerException {1} Member info {2}", ex.ToString(), ex.InnerException.ToString(), model.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            #region Membership Section

            Common.Core.Data.Program program = await db.Programs.Where(a => a.ShortDescription.ToLower().Contains("trendigo")).FirstOrDefaultAsync();

            try
            {
                IMSMembership membership = new IMSMembership();
                membership.ProgramID = program.Id;
                membership.Program = program;
                membership.CreationDate = DateTime.Now;
                membership.IsActive = true;
                membership.MemberID = member.Id;
                membership.Member = member;
                membership.NoShipping = true;

                var registerMembershipCmd = DataCommandFactory.AddMembershipCommand(membership, member.Id.ToString(), 0, db);

                var membershipResult = await registerMembershipCmd.Execute();

                if (membershipResult != DataCommandResult.Success)
                {
                    var deleteCmd = DataCommandFactory.DeleteMemberCommand(member, db);

                    var deleteResult = await deleteCmd.Execute();

                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }
            }
            catch (Exception ex)
            {
                //Delete Member
                if (member != null)
                {
                    var deleteMemberCmd = DataCommandFactory.DeleteMemberCommand(member, db);
                    var transaxResult = await deleteMemberCmd.Execute();

                    if (transaxResult != DataCommandResult.Success)
                    {
                        logger.ErrorFormat("DeleteMemberCommand - Result {0}, Member info {1}", transaxResult.ToString(), model.ToString());
                    }
                }

                if (user != null)
                {
                    //Delete AspNetUserRole
                    IdentityResult deleteRole_result = UserManager.RemoveFromRole(user.Id, IMSRole.Member.ToString());
                    IHttpActionResult deleteRole_errorResult = GetErrorResult(deleteRole_result);

                    if (deleteRole_errorResult != null)
                    {
                        logger.ErrorFormat("UserManager.RemoveFormRole - Result {0}, Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                    }

                    //Delete AspNetUser
                    IdentityResult delete_result = UserManager.Delete(user);
                    IHttpActionResult delete_errorResult = GetErrorResult(delete_result);

                    if (delete_errorResult != null)
                    {
                        logger.ErrorFormat("UserManager.Delete - Result {0}, Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                    }
                }

                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            #region Email Validation Section

            try
            {
                string language = locale;
                if (member.Language != null)
                    language = member.Language.ISO639_1;

                var emailValidationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                var callbackUrl = String.Format(ConfigurationManager.AppSettings["IMS.Service.WebAPI.Member.EmailValidation.CallbackUrl"], user.Id, HttpUtility.UrlEncode(emailValidationToken), locale);

                var subject = Messages.ResourceManager.GetString("CourrielEmailValidationSubject_" + locale);
                var textLink = Messages.ResourceManager.GetString("CourrielEmailValidationTextLink_" + locale);

                await new EmailService().SendConfirmEmailAddressEmail(member, model.email, subject, textLink, callbackUrl);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("SendValidationEmail - Exception {0} Inner Exception {1} Member info {2}", ex.ToString(), ex.InnerException.ToString(), model.ToString());
            }

            #endregion

            createMemberRS memberRS = new createMemberRS();

            memberRS = Mapper.Map<Common.Core.Data.Member, createMemberRS>(member);

            return Content(HttpStatusCode.OK, memberRS);
        }

        [HttpPut]
        [Route("{memberId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateMember([FromBody]UpdateMemberRQ model, long memberId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Language lang = null;

            #endregion

            #region Validation Section

            if (model == null || !ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            var member = await db.Members.FirstOrDefaultAsync(x => x.AspNetUser.UserName == model.email);

            if (member != null && member.Id != memberId)
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("EmailAlreadyUsed_", locale));
            }

            member = await db.Members.FirstOrDefaultAsync(a => a.Id == memberId);

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            #region Initialization Section

            if (model.language != null)
                lang = await db.Languages.FirstOrDefaultAsync(x => x.ISO639_1.ToLower() == model.language.ToLower());

            member.Language = lang;
            member.FirstName = model.firstName;
            member.LastName = model.lastName;
            member.AvatarLink = model.avatar;

            try
            {
                var updateMemberCmd = DataCommandFactory.UpdateMemberCommand(member, db);
                var transaxResult = await updateMemberCmd.Execute();

                if (transaxResult != DataCommandResult.Success)
                {
                    logger.ErrorFormat("UpdateMemberCommand - Result {0} Member info {1}", transaxResult.ToString(), model.ToString());
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateMember_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdateMemberCommand - Exception {0} InnerException {1} Member info {2}", ex.ToString(), ex.InnerException.ToString(), model.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateMember_", locale));
            }

            #endregion

            #region Email Validation Section

            if (member.AspNetUser.Email != model.email.Trim())
            {
                var user = await UserManager.FindByIdAsync(member.AspNetUser.Id);

                user.Email = model.email.Trim();
                user.UserName = model.email.Trim();

                await UserManager.UpdateAsync(user);

                var emailValidationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                var callbackUrl = String.Format(ConfigurationManager.AppSettings["IMS.Service.WebAPI.Member.EmailValidation.CallbackUrl"], user.Id, HttpUtility.UrlEncode(emailValidationToken), locale);

                var subject = Messages.ResourceManager.GetString("CourrielEmailValidationSubject_" + locale);
                var textLink = Messages.ResourceManager.GetString("CourrielEmailValidationTextLink_" + locale);

                try
                {
                    await new EmailService().SendConfirmEmailAddressEmail(member, model.email, subject, textLink, callbackUrl);
                }
                catch(Exception ex)
                {
                    logger.ErrorFormat("SendConfirmEmailAddressEmail - Exception {0} InnerException {1} Member info {2)", ex.ToString(), ex.InnerException.ToString(), model.ToString());
                }

                return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyUpdatedEmailValidation_", locale));
            }

            #endregion

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyUpdated_", locale));
        }

        [HttpDelete]
        [Route("{memberId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteMember(long memberId, [fromHeader] string locale = "en")
        {
            Common.Core.Data.Member member = null;

            #region Validation Section

            member = await db.Members.FirstOrDefaultAsync(a => a.Id == memberId);

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            try
            {
                var deleteMemberCmd = DataCommandFactory.DeleteMemberCommand(member, db);
                var transaxResult = await deleteMemberCmd.Execute();

                if (transaxResult != DataCommandResult.Success)
                {
                    logger.ErrorFormat("DeleteMemberCommand - Result {0} MemberId {1}", transaxResult.ToString(), memberId);
                    return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UnableToDeleteMember_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeleteMemberCommand - Exception {0} InnerException {1}, MemberId {2}", ex.ToString(), ex.InnerException.ToString(), memberId);
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UnableToDeleteMember_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("MemberDeleted_", locale));

        }

        [HttpGet]
        [Route("{memberId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMember(long memberId, [fromHeader] string locale = "en")
        {
            MemberDTO memberDTO = new MemberDTO();
            Common.Core.Data.Member member = null;

            try
            {
                member = await db.Members.FirstOrDefaultAsync(a => a.Id == memberId && a.IsActive == true);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetMember - MemberId {0} Exception {1} InnerException {2}", memberId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieve_", locale));
            }

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            //Set Avatar link
            if (!string.IsNullOrEmpty(member.AvatarLink))
                member.AvatarLink = string.Concat(ConfigurationManager.AppSettings["IMS.WebAPI.Address"], member.AvatarLink);

            memberDTO = Mapper.Map<Common.Core.Data.Member, MemberDTO>(member);

            return Content(HttpStatusCode.OK, memberDTO);
        }

        #endregion

        #region Authentication Section

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login([FromBody] LoginMemberRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            TokenDTO token = new TokenDTO();
            Common.Core.Data.Member member = new Common.Core.Data.Member();

            #endregion

            #region Validation Section

            if (model == null || !ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            var _user = db.AspNetUsers.FirstOrDefault(a => a.UserName == model.email);
            var user = UserManager.FindById(_user.Id);
            if (user == null)
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("InvalidUsernameOrPassword_", locale));
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("AccountLocked_", locale));
            }

            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("EmailNotValidated_", locale));
            }

            var result = await UserManager.CheckPasswordAsync(user, model.password);

            if (!result)
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("InvalidUsernameOrPassword_", locale));
            }

            member = await db.Members.FirstOrDefaultAsync(a => a.UserId == _user.Id);

            if (member == null)
            {
                db.Entry(_user).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("AccountCreationFailed_", locale));
            }

            #endregion

            #region Token Section

            try
            {
                token = await TokenManager.generateToken(model.deviceId);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Login GenerateToken - MemberId {0} Exception {1} InnerException {2}", member.Id, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToLogin_", locale));
            }

            #endregion

            #region PaymentAPI admin section

            IMS.Utilities.PaymentAPI.Model.AuthenticationData TransaxEntity = new IMS.Utilities.PaymentAPI.Model.AuthenticationData();
            TransaxEntity.DeviceId = model.deviceId;
            TransaxEntity.Jti = token.jti;

            try
            {
                EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().MemberLogin(Convert.ToInt32(member.TransaxId), TransaxEntity);
            }
            catch (ApiException ex)
            {
                logger.ErrorFormat("Login PaymentAPI - MemberId {0} Exception {1} InnerException {2}", member.Id, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToLogin_", locale));
            }

            #endregion

            LoginMemberRS loginRS = new LoginMemberRS();
            loginRS.memberId = member.Id;
            loginRS.sessionToken = token.signedJwt;

            return Content(HttpStatusCode.OK, loginRS);
        }

        [HttpPost]
        [Route("socialLogin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SocialLogin([FromBody] SocialLoginRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            TokenDTO token = new TokenDTO();
            Common.Core.Data.Member member = new Common.Core.Data.Member();
            SocialMediaUser socialMediaUser = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            AspNetUser aspNetUser = await db.AspNetUsers.FirstOrDefaultAsync(a => a.UserName == model.email);

            if (aspNetUser == null)
            {
                socialMediaUser = await db.SocialMediaUsers.FirstOrDefaultAsync(a => a.UID == model.uid && a.Provider == model.provider);

                if (socialMediaUser != null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("SocialMediaUserAlreadyUsed_", locale));
                }

                #region Create Member

                #region Declaration Section

                ApplicationUser user = null;
                IMS.Common.Core.Data.IMSUser imsUser = new IMS.Common.Core.Data.IMSUser();
                string password = "Tr3nd1g0";

                #endregion

                #region AspNet Section

                user = new ApplicationUser
                {
                    UserName = model.email,
                    Email = model.email,
                    EmailConfirmed = true
                };

                IdentityResult result;
                //Create AspNetUser
                result = await UserManager.CreateAsync(user, password);

                if (!result.Succeeded == true)
                {
                    logger.ErrorFormat("CreateASync - UserId {0}", user.Id);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }

                //Add role member to new user
                result = UserManager.AddToRole(user.Id, IMSRole.Member.ToString());

                if (!result.Succeeded == true)
                {
                    logger.ErrorFormat("AddToRole Not Created - UserId {0}", user.Id);
                    UserManager.Delete(user);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }

                #endregion

                #region Member Section

                member.FirstName = model.firstName;
                member.LastName = model.lastName;
                member.Language = await db.Languages.FirstOrDefaultAsync(x => x.ISO639_1 == model.language);
                member.UserId = user.Id;
                member.AspNetUser = await db.AspNetUsers.Where(a => a.Id == user.Id).FirstOrDefaultAsync();

                Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();
                member.EnterpriseId = enterprise.Id;

                //Add new member
                try
                {
                    var registerMemberCmd = DataCommandFactory.AddMemberCommand(member, model.email, password, db);

                    var cmdResult = await registerMemberCmd.Execute();

                    if (cmdResult != DataCommandResult.Success)
                    {
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                    }
                }
                catch (Exception ex)
                {
                    if (user != null)
                    {
                        //Delete AspNetUserRole
                        IdentityResult deleteRole_result = UserManager.RemoveFromRole(user.Id, IMSRole.Member.ToString());
                        IHttpActionResult deleteRole_errorResult = GetErrorResult(deleteRole_result);

                        if (deleteRole_errorResult != null)
                        {
                            logger.ErrorFormat("UserManager.RemoveFromRole - Result {0} Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                        }

                        //Delete AspNetUser
                        IdentityResult delete_result = UserManager.Delete(user);
                        IHttpActionResult delete_errorResult = GetErrorResult(delete_result);

                        if (delete_errorResult != null)
                        {
                            logger.ErrorFormat("UserManager.Delete - Result {0} Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                        }
                    }

                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }

                #endregion

                #region Membership Section

                Common.Core.Data.Program program = await db.Programs.Where(a => a.ShortDescription.ToLower().Contains("trendigo")).FirstOrDefaultAsync();

                try
                {
                    IMSMembership membership = new IMSMembership();
                    membership.ProgramID = program.Id;
                    membership.Program = program;
                    membership.CreationDate = DateTime.Now;
                    membership.IsActive = true;
                    membership.MemberID = member.Id;
                    membership.Member = member;
                    membership.NoShipping = true;

                    var registerMembershipCmd = DataCommandFactory.AddMembershipCommand(membership, member.Id.ToString(), 0, db);

                    var membershipResult = await registerMembershipCmd.Execute();

                    if (membershipResult != DataCommandResult.Success)
                    {
                        var deleteCmd = DataCommandFactory.DeleteMemberCommand(member, db);

                        var deleteResult = await deleteCmd.Execute();

                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                    }
                }
                catch (Exception ex)
                {
                    //Delete Member
                    if (member != null)
                    {

                    }

                    if (user != null)
                    {
                        //Delete AspNetUserRole
                        IdentityResult deleteRole_result = UserManager.RemoveFromRole(user.Id, IMSRole.Member.ToString());
                        IHttpActionResult deleteRole_errorResult = GetErrorResult(deleteRole_result);

                        if (deleteRole_errorResult != null)
                        {
                            logger.ErrorFormat("UserManager.RemoveFromRole - Result {0} Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                        }

                        //Delete AspNetUser
                        IdentityResult delete_result = UserManager.Delete(user);
                        IHttpActionResult delete_errorResult = GetErrorResult(delete_result);

                        if (delete_errorResult != null)
                        {
                            logger.ErrorFormat("UserManager.Delete - Result {0} Member info {1}", deleteRole_errorResult.ToString(), model.ToString());
                        }
                    }

                    return Content(HttpStatusCode.InternalServerError, ex.ToString());
                }

                #endregion

                #region Social Media Section

                try
                {
                    SocialMediaUser newSocialMediaUser = new SocialMediaManager().CreateSocialMediaUser(user.Id, model.uid, model.provider);
                }
                catch (Exception ex)
                {
                    try
                    {
                        UserManager.Delete(user);
                    }
                    catch { }

                    logger.ErrorFormat("CreateSocialMediaUser UserId {0} UID {1} Provider {2} Exception {3} InnerException {4}", user.Id, model.uid, model.provider, ex.ToString(), ex.InnerException.ToString());
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
                }

                #endregion

                #endregion
            }

            Common.Core.Data.Member existingMember = await db.Members.Where(a => a.AspNetUser.Email == model.email).FirstOrDefaultAsync();

            socialMediaUser = await db.SocialMediaUsers.FirstOrDefaultAsync(a => a.UserId == existingMember.AspNetUser.Id && a.UID == model.uid);

            if (socialMediaUser == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            try
            {
                token = await TokenManager.generateToken(model.deviceId);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Login GenerateToken - MemberId {0} Exception {1} InnerException {2}", existingMember.Id, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #region PaymentAPI admin section

            IMS.Utilities.PaymentAPI.Model.AuthenticationData TransaxEntity = new IMS.Utilities.PaymentAPI.Model.AuthenticationData();
            TransaxEntity.DeviceId = model.deviceId;
            TransaxEntity.Jti = token.jti;

            try
            {
                EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().MemberLogin(Convert.ToInt32(existingMember.TransaxId), TransaxEntity);
            }
            catch (ApiException ex)
            {
                logger.ErrorFormat("Login PaymentAPI - MemberId {0} Exception {1} InnerException {2}", member.Id, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            LoginMemberRS loginRS = new LoginMemberRS();
            loginRS.memberId = existingMember.Id;
            loginRS.sessionToken = token.signedJwt;

            return Content(HttpStatusCode.OK, loginRS);
        }

        [HttpPost]
        [Route("{memberId:long}/emailValidation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> EmailValidation(long memberId, [FromBody] EmailValidationRQ model, [fromHeader] string locale = "en")
        {
            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            Common.Core.Data.Member member = await db.Members.FirstOrDefaultAsync(a => a.Id == memberId);

            if (member == null)
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));

            var result = await UserManager.ConfirmEmailAsync(member.UserId, model.code);

            if (!result.Succeeded)
            {
                model.code = HttpUtility.UrlDecode(model.code);
                //logger.Debug(code);
                result = await UserManager.ConfirmEmailAsync(member.UserId, model.code);

                if (!result.Succeeded)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("UnableToValidateEmailAddress_", locale));
                }
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("EmailAddressValidated_", locale));
        }

        [HttpPost]
        [Route("forgotPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ForgotPassword([FromBody] ForgotPasswordRQ model, [fromHeader] string locale = "en")
        {
            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            var user = await UserManager.FindByNameAsync(model.email);

            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Content(HttpStatusCode.OK, MessageService.GetMessage("LinkWasSentSuccessfully_", locale));
            }

            var member = await db.Members.FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            try
            {
                locale = string.IsNullOrEmpty(locale) ? member.Language.ISO639_1 : locale;

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = String.Format(
                        ConfigurationManager.AppSettings["IMS.Service.WebAPI.Member.PasswordReset.CallbackUrl"], member.Id, HttpUtility.UrlEncode(code), locale);

                var subject = Messages.ResourceManager.GetString("CourrielPasswordResetSubject_" + locale);
                var textLink = Messages.ResourceManager.GetString("CourrielPasswordResetTextLink_" + locale);

                await new EmailService().SendForgotPasswordEmail(member, model.email, subject, textLink, callbackUrl);

            }
            catch (Exception ex)
            {
                logger.ErrorFormat("ForgotPassword Email {0} Exception {1} InnerException {2}", model.email, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, "Unable to send link");
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("LinkWasSentSuccessfully_", locale));

        }

        [HttpPost]
        [Route("{memberId:long}/changePassword")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> ChangePassword([FromBody] ChangePasswordRQ model, long memberId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Member member = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            member = await db.Members.Where(a => a.Id == memberId).FirstOrDefaultAsync();

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            try
            {
                var _user = db.AspNetUsers.FirstOrDefault(a => a.Id == member.AspNetUser.Id);
                var user = UserManager.FindById(_user.Id);
                if (user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("InvalidUsernameOrPassword_", locale));
                }

                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("AccountLocked_", locale));
                }

                var result = UserManager.ChangePassword(user.Id, model.oldPassword, model.newPassword);

                if (!result.Succeeded)
                {
                    return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("CannotChangePasswordForThatMember_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("ChangePassword - MemberId {0} Exception {1} InnerException {2}", memberId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotChangePasswordForThatMember_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("PasswordSuccessfullyUpdated_", locale));
        }

        [HttpPost]
        [Route("{memberId:long}/resetPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> resetPassword(long memberId, [FromBody] ResetPasswordRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Member member = null;

            #endregion

            #region Validation Section

            if (model == null || !ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            member = await db.Members.Where(a => a.Id == memberId).FirstOrDefaultAsync();

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            var _user = db.AspNetUsers.FirstOrDefault(a => a.Id == member.AspNetUser.Id);
            var user = UserManager.FindById(_user.Id);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Content(HttpStatusCode.OK, "Successfull");
            }

            #endregion

            //var decoded = HttpUtility.UrlDecode(model.code);
            var result = await UserManager.ResetPasswordAsync(user.Id, model.code, model.password);

            if (!result.Succeeded)
            {
                logger.ErrorFormat("UserManager.ResetPasswordAsync MemberId {0} Result {1} Code {2}", memberId, result.Errors.FirstOrDefault(), model.code);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("ResetPasswordFailed_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("PasswordWasSetSuccessfully_", locale));

        }

        #endregion

        #region Notification Section

        [HttpPost]
        [Route("{memberId:long}/notifications")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddNotification([FromBody] NotificationRQ model, long memberId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Member member = null;
            string oldDeviceId = "";

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            member = await db.Members.FirstOrDefaultAsync(a => a.Id == memberId);

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            if (member.AspNetUser.UserNotifications != null && member.AspNetUser.UserNotifications.Count > 0)
            {
                foreach(UserNotification un in member.AspNetUser.UserNotifications.Where(a => a.IsActive == true).ToList())
                {
                    oldDeviceId = un.DeviceId;

                    try
                    {
                        un.IsActive = false;
                        un.ModificationDate = DateTime.Now;
                        db.Entry(un).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);

                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddNotification_", locale));
                    }
                }
            }

            UserNotification newNotification = new UserNotification();
            newNotification.DeviceId = model.deviceId;
            newNotification.NotificationToken = model.notificationToken;
            newNotification.AspNetUser = member.AspNetUser;

            try
            {
                var addUserCmd = DataCommandFactory.AddUserNotificationCommand(newNotification, Convert.ToInt32(member.TransaxId), db);
                var result = await addUserCmd.Execute();

                if (result != DataCommandResult.Success)
                {
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddNotification_", locale));
                }

                //TODO send email for change in deviceId
                if(!string.IsNullOrEmpty(oldDeviceId) && oldDeviceId != newNotification.DeviceId)
                {

                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddNotification_", locale));
            }

            return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("NotificationAddedSuccessfully_", locale));
        }


        #endregion

        #region Credit Card Section

        [HttpPost]
        [Route("{memberId:long}/creditCards")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddCreditCard(long memberId, [FromBody] CreditCardRQ model, [fromHeader] string locale = "en")
        {
            CreditCardDTO ccDTO = new CreditCardDTO();
            CreditCard cc = new CreditCard();

            using (IMSEntities db = new IMSEntities())
            {
                #region Validation Section

                if (!ModelState.IsValid || model == null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
                }

                var member = await db.Members.FirstOrDefaultAsync(x => x.Id == memberId);

                if (member == null)
                {
                    return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
                }

                model.memberId = memberId;

                List<CreditCard> ccards = await new CreditCardService().GetCreditCards(member.Id);

                if (ccards.Count > 0 && ccards.Where(a => a.CardNumber == model.cardNumber && a.IsActive == true).FirstOrDefault() != null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("CreditCardAlreadyExist_", locale));
                }

                if (member.AspNetUser.EmailConfirmed == false)
                {
                    try
                    {
                        string language = locale;
                        if (member.Language != null)
                            language = member.Language.ISO639_1;

                        var emailValidationToken = await UserManager.GenerateEmailConfirmationTokenAsync(member.AspNetUser.Id);

                        var callbackUrl = String.Format(ConfigurationManager.AppSettings["IMS.Service.WebAPI.Member.EmailValidation.CallbackUrl"], member.AspNetUser.Id, HttpUtility.UrlEncode(emailValidationToken), locale);

                        var subject = Messages.ResourceManager.GetString("CourrielEmailValidationSubject_" + locale);
                        var textLink = Messages.ResourceManager.GetString("CourrielEmailValidationTextLink_" + locale);

                        await new EmailService().SendConfirmEmailAddressEmail(member, member.AspNetUser.Email, subject, textLink, callbackUrl);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("SendValidationEmail - Exception {0} Inner Exception {1} Member info {2}", ex.ToString(), ex.InnerException.ToString(), model.ToString());
                    }

                    return Content(HttpStatusCode.Forbidden, MessageService.GetMessage("EmailValidationNecessary_", locale));
                }

                #endregion

                ccDTO = Mapper.Map<CreditCardDTO>(model);

                ccDTO.CreationDate = DateTime.Now;
                ccDTO.IsActive = true;

                cc = Mapper.Map<CreditCardRQ, CreditCard>(model);
                cc.CreationDate = DateTime.Now;
                cc.IsActive = true;

                var command = DataCommandFactory.AddCreditCardCommand(cc, ccDTO, member.TransaxId, db);

                try
                {
                    var result = await command.Execute();

                    if (result != DataCommandResult.Success)
                    {
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddCreditCard_", locale));
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("AddCreditCard - MemberId {0} Exception {1} InnerException {2}", memberId, ex, ex.InnerException);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddCreditCard_", locale));
                }
            }

            CreditCardRS creditCardRS = Mapper.Map<CreditCardRS>(cc);

            return Content(HttpStatusCode.OK, creditCardRS);
        }

        [HttpGet]
        [Route("{memberId:long}/creditCards")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetCreditCards(long memberId, [fromHeader] string locale = "en")
        {
            List<CreditCardRS> CreditCards = null;

            #region Validation Section

            var member = await db.Members.FirstOrDefaultAsync(x => x.Id == memberId);

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            List<CreditCard> ccards = await db.CreditCards.Where(a => a.MemberId == memberId && a.IsActive == true).ToListAsync();

            if (ccards == null)
            {
                return Content(HttpStatusCode.OK, CreditCards);
            }

            CreditCards = Mapper.Map<List<CreditCardRS>>(ccards);
            return Content(HttpStatusCode.OK, CreditCards);
        }

        [HttpGet]
        [Route("{memberId:long}/creditCards/{creditCardId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetCreditCard(long memberId, long creditCardId, [fromHeader] string locale = "en")
        {

            CreditCardRS CreditCard = null;

            #region Validation Section

            var member = await db.Members.FirstOrDefaultAsync(x => x.Id == memberId);

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            CreditCard ccard = await db.CreditCards.Where(a => a.MemberId == memberId && a.Id == creditCardId && a.IsActive == true).FirstOrDefaultAsync();

            if (ccard == null)
            {
                return Content(HttpStatusCode.OK, CreditCard);
            }

            CreditCard = Mapper.Map<CreditCardRS>(ccard);
            return Content(HttpStatusCode.OK, CreditCard);
        }

        [HttpPut]
        [Route("{memberId:long}/creditCards/{creditCardId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateCreditCard([FromBody] UpdateCreditCardRQ model, long memberId, long creditCardId, [fromHeader] string locale = "en")
        {
            CreditCardRS creditCardRS = new CreditCardRS();

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            CreditCard ccard = await db.CreditCards.FirstOrDefaultAsync(x => x.Id == creditCardId && x.MemberId == memberId && x.IsActive == true);

            if (ccard == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("CreditCardNotFound_", locale));
            }

            #endregion

            ccard.CardHolder = model.cardholderName;
            ccard.ExpiryDate = model.expiryDate;

            CreditCardDTO ccDTO = new CreditCardDTO();

            ccDTO = Mapper.Map<CreditCard, CreditCardDTO>(ccard);

            ccDTO.IsActive = true;

            var command = DataCommandFactory.UpdateCreditCardCommand(ccard, ccDTO, db);

            try
            {
                var result = await command.Execute();

                if (result != DataCommandResult.Success)
                {
                    logger.ErrorFormat("UpdateCreditCard - MemberId {0} DataCommandResult {1}", memberId, result);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateCreditCard_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdateCreditCard - MemberId {0} Exception {1} InnerException {2}", memberId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateCreditCard_", locale));
            }

            //creditCardRS = Mapper.Map<CreditCard, CreditCardRS>(ccard);

            return Content(HttpStatusCode.OK, MessageService.GetMessage("CreditCardUpdated_", locale));
        }

        [HttpDelete]
        [Route("{memberId:long}/creditCards/{creditCardId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteCreditCard(long memberId, long creditCardId, [fromHeader] string locale = "en")
        {
            #region Validation Section

            CreditCard creditCard = await db.CreditCards.FirstOrDefaultAsync(a => a.Id == creditCardId && a.MemberId == memberId && a.IsActive == true);

            if (creditCard == null)
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("CreditCardNotFound_", locale));

            if (creditCard.Member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            var command = DataCommandFactory.DeleteCreditCardCommand(creditCard, db);

            try
            {
                var result = await command.Execute();

                if (result != DataCommandResult.Success)
                {
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteCreditCard_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MemberController - DeleteCreditCard - MemberId {0} CardId {1} Exception {2} InnerException {3}", creditCard.MemberId, creditCard.Id, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteCreditCard_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("CreditCardDeleted_", locale));
        }

        #endregion

        #region Transaction Section

        [HttpGet]
        [Route("{memberId:long}/transactions")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> TransactionsHistory(long memberId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Member member = null;
            List<MemberTransactionHistoryRS> result = new List<MemberTransactionHistoryRS>();

            #endregion

            #region Validation Section

            member = await db.Members.Where(a => a.Id == memberId).FirstOrDefaultAsync();

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            try
            {
                result = (from ft in db.TrxFinancialTransactions
                          from nft in db.TrxNonFinancialTransactions
                          from m in db.Merchants
                          from l in db.Locations
                          where l.MerchantId == m.Id
                          where ft.legTransaction == nft.Id
                          where nft.memberId.ToString() == member.TransaxId
                          where ft.entityId == l.TransaxId
                          select new MemberTransactionHistoryRS
                          {
                              invoiceId = ft.Id,
                              merchantId = m.Id,
                              merchant = m.Name,
                              date = ft.localDateTime ?? DateTime.Now,
                              amount = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0),
                              reward = nft.pointsGained ?? 0,
                              pointUsed = nft.pointsExpended ?? 0

                          }).OrderByDescending(a => a.date).ToList();
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("TransactionsHistory - MemberId {0} Exception {1} InnerException {2}", memberId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTransaction_", locale));
            }

            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("{memberId:long}/wallets")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> PointsBalance(long memberId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Member member = null;
            List<Membership> response = null;
            List<pointBalanceRS> pointsRS = new List<pointBalanceRS>();

            List<Common.Core.Data.Merchant> merchants = await db.Merchants.Where(a => a.IsActive == true).ToListAsync();
            List<Common.Core.Data.Program> programs = await db.Programs.Where(a => a.IsActive == true).ToListAsync();

            #endregion

            #region Validation Section

            member = await db.Members.Where(a => a.Id == memberId).FirstOrDefaultAsync();

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            try
            {
                response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().FindMemberMemberships(Convert.ToInt32(member.TransaxId));
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("PointsBalance - MemberId {0} Exception {1} InnerException {2}", memberId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrievePointBalance_", locale));
            }
            
            foreach(Membership m in response)
            {
                Common.Core.Data.Program prg = programs.FirstOrDefault(a => a.TransaxId == m.ProgramId.ToString());


                pointBalanceRS pointRS = new pointBalanceRS();
                pointRS.communityType = prg.ProgramType.Description;
                pointRS.communityName = prg.ShortDescription;
                if (prg.ProgramTypeId == (int)ProgramTypeEnum.Personnal)
                {
                    pointRS.merchantId = prg.Merchants.FirstOrDefault().Id;
                    pointRS.merchantName = prg.Merchants.FirstOrDefault().Name;
                }
                pointRS.points = m.PointBalance.HasValue ? m.PointBalance.Value : 0;

                pointsRS.Add(pointRS);
            }

            return Content(HttpStatusCode.OK, pointsRS);
        }

        [HttpGet]
        [Route("{memberId:long}/invoices/{invoiceId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetInvoice(long memberId, long invoiceId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Member member = null; 
            Common.Core.Data.Location location = null;
            TrxFinancialTransaction financialTransaction = null;
            MemberInvoiceTransactionRS result = null;

            #endregion

            #region Validation Section

            member = await db.Members.Where(a => a.Id == memberId).FirstOrDefaultAsync();

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            financialTransaction = await db.TrxFinancialTransactions.Where(a => a.Id == invoiceId).FirstOrDefaultAsync();

            if (financialTransaction == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("TransactionNotFound_", locale));
            }

            location = await db.Locations.Where(a => a.TransaxId == financialTransaction.entityId).FirstOrDefaultAsync();

            if (location == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("TransactionNotFound_", locale));
            }

            if (financialTransaction.CreditCard != null && financialTransaction.CreditCard.MemberId != memberId)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("TransactionNotFound_", locale));
            }

            #endregion

            try
            {
                result = (from ft in db.TrxFinancialTransactions
                          join nft in db.TrxNonFinancialTransactions on ft.legTransaction equals nft.Id
                          join l in db.Locations on nft.entityId equals l.TransaxId
                          join m in db.Merchants on l.MerchantId equals m.Id
                          join u in db.IMSUsers on ft.vendorId equals u.TransaxId
                          join cc in db.CreditCards on ft.creditCardId.Value.ToString() equals cc.TransaxId into ncc
                          from ccardInfo in ncc.DefaultIfEmpty()
                          where ft.Id == invoiceId
                          select new MemberInvoiceTransactionRS
                          {
                              merchantName = m.Name,
                              merchantAddress = l.Address.StreetAddress,
                              merchantCity = l.Address.City,
                              merchantState = l.Address.State.Name,
                              merchantZip = l.Address.Zip,
                              merchantPhone = l.Telephone,
                              invoiceId = ft.Id,
                              orderNumber = nft.orderNumber,
                              transactionTypeId = ft.transactionTypeId.Value,
                              collaborator = ft.description,
                              //collaborator = string.Concat(u.FirstName, " ", u.LastName),
                              date = ft.localDateTime ?? DateTime.Now,
                              amount = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0) - ft.tip ?? 0,
                              tip = ft.tip ?? 0,
                              total = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0),
                              reward = nft.pointsGained == null ? 0 : nft.pointsGained.Value,
                              pointUsed = nft.pointsExpended.HasValue ? nft.pointsExpended.Value : 0,
                              cardNumber = ccardInfo.CardNumber ?? string.Empty,
                              cardType = ccardInfo.CreditCardType.Description ?? string.Empty,
                              authorization = ft.merchantResponseMessage.Replace(" ", "").Replace("APPROVED", ""),
                              cardTransactionAmount = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0) - (nft.pointsExpended.HasValue ? nft.pointsExpended.Value / 100 : 0)
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetInvoice - MemberId {0}, Exception {1} InnerException {2}", memberId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UnableToRetrieveInvoiceInfo_", locale));
            }

            if (result == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("TransactionNotFound_", locale));
            }

            return Content(HttpStatusCode.OK, result);
        }

        #endregion

        #region Message Section

        [HttpGet]
        [Route("{memberId:long}/messages")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMessages(long memberId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Member member = null;
            List<getMessageRS> messages = new List<getMessageRS>();

            #endregion

            #region Validation Section

            member = await db.Members.FirstOrDefaultAsync(x => x.Id == memberId);

            if (member == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
            }

            #endregion

            getMessageRS message = new getMessageRS();
            message.messageId = 1;
            message.title = "Welcome";
            message.message = "Welcome to the Trendigo application !";

            messages.Add(message);

            return Content(HttpStatusCode.OK, messages);
        }

        #endregion

        #region Private Methods

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // Aucune erreur ModelState à envoyer, simple retour d'un BadRequest vide.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}