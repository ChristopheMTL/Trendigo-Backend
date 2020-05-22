using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using IMS.Common.Core.Entities;
using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.Transax;
using IMS.Store.Common.Extensions;
using System.Transactions;
using IMS.Common.Core.Exceptions;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.UI;
using System.Web;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Identity;
using IMSUser = IMS.Common.Core.Data.IMSUser;
using System.Data.Entity;

namespace IMS.Common.Core.Services
{
    public class RegistrationManager
    {
        IMSEntities context = new IMSEntities();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enterpriseID"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IMSMemberRS> ValidateMember(int enterpriseID, string email, string password) 
        {
            logger.DebugFormat("Enterprise {0} Email {1} Password {2}", enterpriseID, email, password);

            IMSMemberRS response = new IMSMemberRS();

            //string username = new UtilityManager().FormatUsernameWithEmail(email, enterpriseID);


            using (var ctx = new IdentityDbContext("IMS_StoreEntities"))
            {
                using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx)))
                {
                    var u = await userManager.FindByEmailAsync(email);
                    var user = await userManager.CheckPasswordAsync(u, password);

                    if (user == null)
                    {
                        response.Code = ((int)IMSCodeMessage.InvalidUsernameOrPassword).ToString();
                        response.Message = IMSCodeMessage.InvalidUsernameOrPassword.ToString();
                        //Trace.TraceWarning(msg);
                    }
                    else
                    {
                        response.Code = ((int)IMSCodeMessage.Success).ToString();
                        response.Message = IMSCodeMessage.Success.ToString();
                        //response.Member = new Member();
                        //response.Member = getMemberWithUserID(user.Id);
                    }
                }
            }

            //using (var ctx = new IdentityDbContext("IMS_StoreEntities"))
            //{
            //    using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx)))
            //    {
            //        var user = userManager.Find(username, password);
            //        //AspNetUser user = context.AspNetUsers.FirstOrDefault(a => a.UserName == username);

            //        //logger.DebugFormat("PasswordHash {0} password {1}", user.PasswordHash, password);

            //        //var result = userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);

            //        //logger.DebugFormat("result {0}", result);

            //        //if (!(result == PasswordVerificationResult.Success))
            //        //{
            //        //    response.Code = ((int)IMSCodeMessage.InvalidUsernameOrPassword).ToString();
            //        //    response.Message = IMSCodeMessage.InvalidUsernameOrPassword.ToString();
            //        //}
            //        //else
            //        //{
            //        //    response.Code = ((int)IMSCodeMessage.Success).ToString();
            //        //    response.Message = IMSCodeMessage.Success.ToString();
            //        //    //response.Member = new Member();
            //        //    //response.Member = getMemberWithUserID(user.Id);
            //        //}



            //        //if (user == null)
            //        //{
            //        //    response.Code = ((int)IMSCodeMessage.InvalidUsernameOrPassword).ToString();
            //        //    response.Message = IMSCodeMessage.InvalidUsernameOrPassword.ToString();
            //        //    //Trace.TraceWarning(msg);
            //        //}
            //        //else
            //        //{
            //        response.Code = ((int)IMSCodeMessage.Success).ToString();
            //        response.Message = IMSCodeMessage.Success.ToString();
            //        //    response.Member = new Member();
            //        //    response.Member = getMemberWithUserID(user.Id);
            //        //}
            //    }
            //}

            //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();


            //if (Membership.ValidateUser(username, password)) 
            //{
            //    String userID = (Guid)(Membership.GetUser(username, false).ProviderUserKey);

            //    response.Code = ((int)IMSCodeMessage.Success).ToString();
            //    response.Message = IMSCodeMessage.Success.ToString();
            //    response.Member = new Member();
            //    response.Member = getMemberWithUserID(userID);
            //}
            //else 
            //{
            //    response.Code = ((int)IMSCodeMessage.InvalidUsernameOrPassword).ToString();
            //    response.Message = IMSCodeMessage.InvalidUsernameOrPassword.ToString();
            //}

            return response;
        }

        public IMSMemberRS ChangePassword(int enterpriseID, string email, string oldPassword, string newPassword)
        {
            IMSMemberRS response = new IMSMemberRS();

            //string username = new UtilityManager().FormatUsernameWithEmail(email, enterpriseID);

            //if (Membership.ValidateUser(username, oldPassword))
            //{
            //    MembershipUser usr = Membership.GetUser(username, false);
            //    string resetPwd = usr.ResetPassword();
            //    usr.ChangePassword(oldPassword, newPassword);

            //    Guid userID = (Guid)(usr.ProviderUserKey);

            //    response.Code = ((int)IMSCodeMessage.Success).ToString();
            //    response.Message = IMSCodeMessage.Success.ToString();
            //    response.Member = new Member();
            //    response.Member = getMemberWithUserID(userID);
            //}
            //else
            //{
            //    response.Code = ((int)IMSCodeMessage.InvalidUsernameOrPassword).ToString();
            //    response.Message = IMSCodeMessage.InvalidUsernameOrPassword.ToString();
            //}

            return response;
        }

        public IMSMemberRS RetrievePassword(int enterpriseID, string email, string password)
        {
            IMSMemberRS response = new IMSMemberRS();

            //string username = new UtilityManager().FormatUsernameWithEmail(email, enterpriseID);

            //if (Membership.ValidateUser(username, password))
            //{
            //    MembershipUser usr = Membership.GetUser(username, false);
            //    string resetPwd = usr.ResetPassword();
            //    usr.ChangePassword(oldPassword, newPassword);

            //    Guid userID = (Guid)(usr.ProviderUserKey);

            //    response.Code = ((int)IMSCodeMessage.Success).ToString();
            //    response.Message = IMSCodeMessage.Success.ToString();
            //    response.Member = new Member();
            //    response.Member = getMemberWithUserID(userID);
            //}
            //else
            //{
            //    response.Code = ((int)IMSCodeMessage.InvalidUsernameOrPassword).ToString();
            //    response.Message = IMSCodeMessage.InvalidUsernameOrPassword.ToString();
            //}

            return response;
        }

        public IMSUserRS CreateUser(String userId, String accessToken, String enterpriseID, string firstname, string lastname, string email,
            string password, string language, string roleId, string country = "", string region = "", string city = "", string streetAddress = "",
            string postalCode = "", string numberAddress = "", string floorAddress = "")
        {
            IMSUserRS userRS = new IMSUserRS();
            IMSUser user = new IMSUser();
            user.UserId = userId;
            //user.EnterpriseId = Convert.ToInt64(enterpriseID);
            user.LastName = lastname;
            user.FirstName = firstname;
            user.CreationDate = DateTime.Now;

            ////Create Membership user
            //try
            //{
            //    IdentityUser iuser = CreateMembershipUser(enterpriseId, roleName, email, password, context);
            //    user = MapUserToIMSUser(iuser, user);
            //}
            //catch (IMSException ex)
            //{
            //    userRS.Code = ex.args[0].ToString();
            //    userRS.Message = ex.args[1].ToString();
            //    return userRS;
            //}
            //catch (Exception ex)
            //{
            //    userRS.Code = "999";
            //    userRS.Message = ex.Message.ToString();
            //    return userRS;
            //}

            //Create Transax user
            //try
            //{
            //    TransaxUser transaxUser = CreateTransaxUser(accessToken, enterpriseID, (int)TransaxUserTypeEnum.Admin, firstname, lastname, email, 
            //                                password, language, country, region, city, streetAddress, postalCode, numberAddress, floorAddress);
            //    user = MapTransaxUserToIMSUser(user, transaxUser);
            //}
            //catch (IMSException ex)
            //{
            //    userRS.Code = ex.ErrorCode.ToString();
            //    userRS.Message = ex.Message;

            //    //We have to rollback the creation of the MembershipUser

            //    return userRS;
            //}
            //catch (Exception ex)
            //{
            //    userRS.Code = "999";
            //    userRS.Message = ex.Message.ToString();
            //    return userRS;
            //}

            //Create IMS user
            try
            {
                user.TransaxId = "300";
                user = CreateIMSUser(user);
            }
            catch (IMSException ex)
            {
                userRS.Code = ex.ErrorCode.ToString();
                userRS.Message = ex.Message;

                //We have to rollback the creation of the MembershipUser/TransaxUser

                return userRS;
            }

            userRS.User = user;
            userRS.Code = ((int) IMSCodeMessage.Success).ToString();

            return userRS;
        }

        public IMSUser GetUserWithEmail(String email)
        {
            return context.IMSUsers.SingleOrDefault(s => s.AspNetUser.Email == email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IMSUser GetUserWithUserID(String userId)
        {

            var query = context.IMSUsers
                .Where(s => s.UserId == userId)
                .SingleOrDefault();

            IMSUser user = (IMSUser)query;
            
            return user;
        }

        public IMSUser GetUserWithUserID(String userId, IMSEntities context)
        {

            var query = context.IMSUsers
                .Where(s => s.UserId == userId)
                .SingleOrDefault();

            IMSUser user = (IMSUser)query;

            return user;
        }

        public IMSUser GetUserWithID(Int64 Id)
        {

            var query = context.IMSUsers
                .Where(s => s.Id == Id)
                .SingleOrDefault();

            IMSUser user = (IMSUser)query;

            return user;
        }

        public IMSUser GetUserWithTransaxId(String transaxId) 
        {
            var query = context.IMSUsers
                .Where(s => s.TransaxId == transaxId)
                .SingleOrDefault();

            IMSUser user = (IMSUser)query;

            return user;
        }

        public AspNetRole GetRoleWithRoleName(String roleName) 
        {
            var query = context.AspNetRoles
                    .Where(s => s.Name == roleName)
                    .Select(x => x);

            return (AspNetRole)query.First();
        }

        public List<IMSUser> GetAllUser(params String[] roleNames) 
        {

            return context.IMSUsers.Include(x => x.AspNetUser)
                .Where(usr => roleNames.Intersect(usr.AspNetUser.AspNetRoles.Select(x => x.Name)).Any()).ToList();

            //if (RoleName == IMSRole.IMSAdmin.ToString())
            //{
            //    var query = context.IMSUsers
            //        .Include("Enterprise").AsEnumerable()
            //        .Select(x => x);

            //    return (List<IMSUser>)query.ToList();
            //}

            //if (RoleName == IMSRole.EnterpriseAdmin.ToString()) 
            //{
            //    var query = context.IMSUsers
            //        .Include("Enterprise").AsEnumerable()
            //        .Where(a => a.RoleId == IMSRole.MerchantAdmin.ToString())
            //        .Where(a => a.RoleId == IMSRole.LocationAdmin.ToString())
            //        .Where(a => a.RoleId == IMSRole.VendorAdmin.ToString())
            //        .Where(a => a.RoleId == IMSRole.SalesRep.ToString())
            //        .Where(a => a.RoleId == IMSRole.OutsideChannel.ToString())
            //        .Select(x=>x);

            //    return (List<IMSUser>)query.ToList();
            //}

            //if (RoleName == IMSRole.MerchantAdmin.ToString())
            //{
            //    var query = context.IMSUsers
            //        .Include("Enterprise").AsEnumerable()
            //        .Where(a => a.RoleId == IMSRole.LocationAdmin.ToString())
            //        .Where(a => a.RoleId == IMSRole.VendorAdmin.ToString())
            //        .Select(x => x);

            //    return (List<IMSUser>)query.ToList();
            //}

            //if (RoleName == IMSRole.LocationAdmin.ToString())
            //{
            //    var query = context.IMSUsers
            //        .Include("Enterprise").AsEnumerable()
            //        .Where(a => a.RoleId == IMSRole.VendorAdmin.ToString())
            //        .Select(x => x);

            //    return (List<IMSUser>)query.ToList();
            //}

        }

        #region Private Section
   
        private IdentityUser CreateMembershipUser(Int64 enterpriseID, String roleName, String email, String password)
        {
            using (var ctx = new IMSEntities())
            {
                using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx)))
                {
                    //string username = new UtilityManager().FormatUsernameWithEmail(email, enterpriseID);
                    var user = new IdentityUser() { UserName = email, Email = email };
                    IdentityResult result = userManager.Create(user, password);
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(user.Id, roleName);
                    }
                    else
                    {
                        throw new IMSException(String.Format(result.Errors.FirstOrDefault() ?? ""), (int)IMSCodeMessage.UserRejected);
                    }

                    return user;
                }
            }
        }

   
        private Member MapUserToMember(IdentityUser user, Member member)
        {
            member.UserId = user.Id;
            return member;
        }

    
        private IMSUser MapUserToIMSUser(IdentityUser user, IMSUser IMSuser)
        {
            IMSuser.UserId = user.Id;
            return IMSuser;
        }

   
        private Member CreateIMSMember(Member _Member)
        {
            try
            {
                _Member.CreationDate = DateTime.Now;

                context.Members.Add(_Member);
                context.SaveChanges();
            }
            catch (IMSException ex)
            {
                throw new IMSException("cannot create", ex, 100);
            }

            return _Member;
        }


        private IMSUser CreateIMSUser(IMSUser _user)
        {
            try
            {
                _user.CreationDate = DateTime.Now;
                _user.ModificationDate = DateTime.Now;
                _user.IsActive = true;
                context.IMSUsers.Add(_user);
                context.SaveChanges();
            }
            catch (IMSException ex)
            {
                throw new IMSException("cannot create", ex, 100);
            }

            return _user;
        }


        private Member getMemberWithUserID(String userID)
        {
            var query =
            from member in context.Members
            .Include("Member_EWallet")
            .Include("Member_Card")
            .Include("IMSCard")
            .Include("Member_Address")
            .Include("Address")
            where
                member.UserId == userID
            select member;

            return (Member)query;
        }

        private Member MapTransaxUserToIMSMember(Member member, TransaxUser user)
        {
            member.TransaxId = user.id;
            return member;
        }


        private IMSUser MapTransaxUserToIMSUser(IMSUser IMSuser, TransaxUser user)
        {
            IMSuser.TransaxId = user.id;
            return IMSuser;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AspNetRole> GetAdminRoles()
        {
            List<AspNetRole> listRole = new List<AspNetRole>();

            var query =
            from roles in context.AspNetRoles
            where roles.Name != IMSRole.Member.ToString()
            select roles;

            foreach (AspNetRole role in query)
            {
                listRole.Add(new AspNetRole { Id = role.Id, Name = role.Name });
            }

            return listRole;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        public List<Enterprise> GetEnterprises(String portalId)
        {
            List<Enterprise> listEnterprise = new List<Enterprise>();

            var query =
                from enterprises in context.Enterprises
                where enterprises.PortalId == new Guid(portalId)
                where enterprises.IsActive == true
                select enterprises;

            foreach (Enterprise enterprise in query)
            {
                listEnterprise.Add(new Enterprise { Id = enterprise.Id, Name = enterprise.Name});
            }

            return listEnterprise;
        }

        public IList<OutsideChannel> GetOutsideChannels(String portalId)
        {

            return  context.OutsideChannels
                .Include(x => x.Enterprise)
                .Where(x => x.Enterprise.PortalId == new Guid(portalId))
                .ToList();
               
        }
    }
}
