using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Services;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Models;
using IMS.Service.WebAPI2.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Swashbuckle.Swagger.Annotations;
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
using System.Web.Http.Cors;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("users")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class UsersController : ApiController
    {
        private ApplicationUserManager _userManager;
        private IMSEntities db = new IMSEntities();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructor Section

        public ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket> AccessTokenFormat { get; private set; }

        public ApplicationUserManager UserManager
        {
            get
            {
                //return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //if (_userManager == null)
                //{
                //    return new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                //}
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager,
                    ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        #endregion

        [HttpGet]
        [Route("{userId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMerchantAdmin(long merchantId, long userId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            UserDTO userDTO = new UserDTO();
            Common.Core.Data.IMSUser imsUser = null;

            #endregion

            #region Validation Section

            try
            {
                imsUser = await db.IMSUsers.FirstOrDefaultAsync(a => a.Id == userId && a.IsActive == true);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetMerchantAdmin - UserId {0} Exception {1} InnerException {2}", userId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, "Unable to retrieve the merchant admin");
            }

            if (imsUser == null)
            {
                return Content(HttpStatusCode.NotFound, "Merchant admin not found");
            }

            if (imsUser.Merchants != null && imsUser.Merchants.FirstOrDefault().Id != merchantId)
            {
                return Content(HttpStatusCode.NotFound, "Merchant not found");
            }

            #endregion

            userDTO = Mapper.Map<Common.Core.Data.IMSUser, UserDTO>(imsUser);

            return Content(HttpStatusCode.OK, userDTO);
        }

        [HttpGet]
        [Route("{userId:long}/messages")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMessages(long userId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.IMSUser user = null;
            List<getMessageRS> messages = new List<getMessageRS>();

            #endregion

            #region Validation Section

            user = await db.IMSUsers.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found");
            }

            #endregion

            getMessageRS message = new getMessageRS();
            message.messageId = 1;
            message.title = "Welcome";
            message.message = "Welcome to the Trendigo application !";

            messages.Add(message);

            return Content(HttpStatusCode.OK, messages);
        }

        [HttpPost]
        [Route("forgotPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> forgotPassword([FromBody] ForgotPasswordRQ model, [fromHeader] string locale = "en")
        {
            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, "Missing parameters");
            }

            var user = await UserManager.FindByNameAsync(model.email);

            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Content(HttpStatusCode.OK, "Link was sent successfully");
            }

            var IMSUser = await db.IMSUsers.FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (IMSUser == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Content(HttpStatusCode.OK, "Link was sent successfully");
            }

            #endregion

            try
            {
                locale = string.IsNullOrEmpty(locale) ? IMSUser.Language.ISO639_1 : locale;

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = String.Format(
                        ConfigurationManager.AppSettings["IMS.Service.WebAPI.User.PasswordReset.CallbackUrl"], IMSUser.Id, HttpUtility.UrlEncode(code), locale);

                var subject = Messages.ResourceManager.GetString("CourrielPasswordResetSubject_" + locale);
                var textLink = Messages.ResourceManager.GetString("CourrielPasswordResetTextLink_" + locale);

                await new EmailService().SendForgotPasswordEmail(IMSUser, model.email, subject, textLink, callbackUrl);

            }
            catch (Exception ex)
            {
                logger.ErrorFormat("ForgotPassword Email {0} Exception {1} InnerException {2}", model.email, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, "Unable to send link");
            }

            return Content(HttpStatusCode.OK, "Link was sent successfully");
        }

        [HttpPost]
        [Route("{userId:long}/resetPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> resetPassword(long userId, [FromBody] ResetPasswordRQ model, [fromHeader] string locale = "en")
        {
            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, "Missing parameters");
            }

            var IMSUser = await db.IMSUsers.Where(a => a.Id == userId).FirstOrDefaultAsync();

            if (IMSUser == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found");
            }

            var user = await UserManager.FindByNameAsync(IMSUser.AspNetUser.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Content(HttpStatusCode.OK, "Successfull");
            }

            #endregion

            var result = await UserManager.ResetPasswordAsync(user.Id, model.code, model.password);

            if (!result.Succeeded)
            {
                return Content(HttpStatusCode.InternalServerError, "ResetPassword failed");
            }

            return Content(HttpStatusCode.OK, "Password was set successfully");

        }

        [HttpPost]
        [Route("{userId:long}/changePassword")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> ChangePassword([FromBody] ChangePasswordRQ model, long userId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.IMSUser imsUser = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, "Missing parameters");
            }

            imsUser = await db.IMSUsers.Where(a => a.Id == userId).FirstOrDefaultAsync();

            if (imsUser == null)
            {
                return Content(HttpStatusCode.NotFound, "Member not found");
            }

            #endregion

            try
            {
                var _user = db.AspNetUsers.FirstOrDefault(a => a.Id == imsUser.AspNetUser.Id);

                if (_user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Wrong email or password");
                }

                var user = UserManager.FindById(_user.Id);

                if (user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Wrong email or password");
                }

                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return Content(HttpStatusCode.Unauthorized, "Account locked");
                }

                var result = UserManager.ChangePassword(user.Id, model.oldPassword, model.newPassword);

                if (!result.Succeeded)
                {
                    return Content(HttpStatusCode.Unauthorized, "Cannot change password for that member");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("ChangePassword - UserId {0} Exception {1} InnerException {2}", userId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, ex.ToString());
            }

            return Content(HttpStatusCode.OK, "Password sucessfully updated");
        }

        [HttpPost]
        [Route("{userId:long}/emailValidation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> EmailValidation(long userId, [FromBody] EmailValidationRQ model, [fromHeader] string locale = "en")
        {
            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, "Missing parameters");
            }

            IMSUser imsUser = await db.IMSUsers.Where(a => a.Id == userId).FirstOrDefaultAsync();

            if (imsUser == null)
                return Content(HttpStatusCode.NotFound, "User not found");

            logger.DebugFormat("Email Validation - UserId {0} Email {1} Code {2}", imsUser.Id, imsUser.AspNetUser.Email, model.code);

            #endregion

            var result = await UserManager.ConfirmEmailAsync(imsUser.UserId, model.code);

            if (!result.Succeeded)
            {
                //model.code = HttpUtility.UrlDecode(model.code);
                result = await UserManager.ConfirmEmailAsync(imsUser.UserId, model.code);

                if (!result.Succeeded)
                {
                    return Content(HttpStatusCode.BadRequest, "Unable to validate email address");
                }
            }

            return Content(HttpStatusCode.OK, "Email address was validated successfully");
        }

        [HttpGet]
        [Route("{userId:long}/notifications")]
        [JwtAuthentication]
        [SwaggerResponse(HttpStatusCode.OK, "Success", typeof(List<NotificationRS>))]
        public async Task<IHttpActionResult> GetNotification(long userId, [fromHeader] string locale = "en")
        {
            #region Declaration Section
            var notifications = new List<NotificationRS>();
            #endregion

            #region Validation Section
             var  imsUser = await db.IMSUsers.FirstOrDefaultAsync(a => a.Id == userId && a.IsActive == true);

            if (imsUser == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UserNotFound_", locale));
            }
            #endregion
            try
            {
                notifications = imsUser.AspNetUser.UserNotifications.Select(x => new NotificationRS
                {
                    deviceId = x.DeviceId,
                    notificationToken = x.NotificationToken,
                    creationDate = x.CreationDate

                }).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieve_", locale));
            }

            return Content(HttpStatusCode.OK, notifications);
        }
    }
}
