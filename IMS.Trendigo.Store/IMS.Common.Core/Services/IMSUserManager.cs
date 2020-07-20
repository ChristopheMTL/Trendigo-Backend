using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IMS.Common.Core.Services
{
    public class IMSUserManager
    {
        IMSEntities context = new IMSEntities();

        public List<IMSUser> GetIMSUserListWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<IMSUser> users = new List<IMSUser>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                return context.IMSUsers.ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                users = context.IMSUsers
                    .Where(a => a.AspNetUser.AspNetRoles.Any(
                        b => b.Name != IMSRole.IMSAdmin.ToString() && 
                        b.Name != IMSRole.IMSUser.ToString() && 
                        b.Name != IMSRole.IMSSupport.ToString() &&
                        b.Name != IMSRole.SponsorAdmin.ToString() &&
                        b.Name != IMSRole.SponsorUser.ToString() && 
                        b.Name != IMSRole.Member.ToString()
                        )
                    )
                    .Where(c => c.Enterprise.Id == user.EnterpriseId).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                IQueryable<long> locations = context.Locations.Where(a => a.IMSUsers.Any(b => b.Id == user.Id)).Distinct().Select(c => c.Id);

                users = context.IMSUsers
                    .Where(a => a.AspNetUser.AspNetRoles.Any(
                        b => b.Name != IMSRole.IMSAdmin.ToString() &&
                        b.Name != IMSRole.IMSUser.ToString() &&
                        b.Name != IMSRole.IMSSupport.ToString() &&
                        b.Name != IMSRole.SponsorAdmin.ToString() &&
                        b.Name != IMSRole.SponsorUser.ToString() &&
                        b.Name != IMSRole.MerchantAdmin.ToString() &&
                        b.Name != IMSRole.MerchantUser.ToString() &&
                        b.Name != IMSRole.Member.ToString()
                        )
                    && locations.Contains(a.Id)).Distinct().ToList();
            }

            if (ActiveOnly)
                users = users.Where(a => a.IsActive == true).ToList();

            return users;
        }

        public IMSUser GetIMSUserWithRole(IMSUser user, String userId)
        {
            List<IMSUser> users = GetIMSUserListWithRole(user);
            var listOfUserId = users.Select(m => m.Id);

            return context.IMSUsers.Where(a => a.Id.ToString() == userId).Where(a => listOfUserId.Contains(a.Id)).FirstOrDefault();
        }

        /// <summary>
        /// This method is to get the password format of a new back office user
        /// </summary>
        /// <param name="id">Identifier of the user</param>
        /// <returns>The password for that user</returns>
        public String getNewUserPswd(long id) 
        {
            string pswd = "";

            IMSUser imsuser = context.IMSUsers.FirstOrDefault(a => a.Id == id);

            pswd = "TGo" + imsuser.CreationDate.ToString("yyyyMMdd") + "00" + imsuser.Id.ToString();

            return pswd;
        }

        public IMSUser GetMerchantAdminUserInfo(long merchantId)
        {
            IMSUser user = new IMSUser();

            try
            {
                user = context.IMSUsers.Where(a => a.Merchants.Any(b => b.Id == merchantId) && a.AspNetUser.AspNetRoles.Any(c => c.Name == IMSRole.MerchantAdmin.ToString())).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("IMSUserManager - GetMerchantAdminUserInfo merchantId {0} Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString()));
            }

            if (user == null)
                throw new Exception("MerchantAdminUser not found");

            return user;
        }
    }
}
