using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IMS.Common.Core.Services
{
    public class RoleManager
    {
        IMSEntities context = new IMSEntities();

        public List<Merchant> GetRoleListWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<Merchant> merchants = new List<Merchant>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                return context.Merchants.ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                merchants = context.Merchants.Where(a => a.Enterprises.Any(b => b.Id == user.Id)).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                merchants = context.Merchants.Where(a => a.Locations.Any(b => b.IMSUsers.Any(c => c.Id == user.Id))).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SalesRep.ToString()))
            {
                merchants = context.Merchants.Where(a => a.Locations.Any(b => b.ContractLocations.Any(c => c.Contract.SalesRepId == user.Id))).ToList();
            }

            if (ActiveOnly)
                merchants = merchants.Where(a => a.IsActive == true).ToList();

            return merchants;
        }
    }
}
