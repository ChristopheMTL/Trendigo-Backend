using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;

namespace IMS.Common.Core.Services
{
    public class EnterpriseManager
    {
        IMSEntities context = new IMSEntities();

        public List<Enterprise> GetEnterprisesWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<Enterprise> enterprises = new List<Enterprise>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                enterprises = context.Enterprises.ToList();
                if (ActiveOnly)
                    enterprises = enterprises.Where(a => a.IsActive == true).ToList();
                return enterprises;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                enterprises = context.Enterprises.Where(a => a.Id == user.EnterpriseId).Distinct().ToList();
            }

            if (ActiveOnly)
                enterprises = enterprises.Where(a => a.IsActive == true).ToList();

            return enterprises;
        }

        public Enterprise GetEnterpriseWithRole(IMSUser user, String enterpriseId)
        {
            List<Enterprise> enterprises = GetEnterprisesWithRole(user);
            var listOfEnterpriseId = enterprises.Select(m => m.Id);

            return context.Enterprises.Where(a => a.Id.ToString() == enterpriseId).Where(a => listOfEnterpriseId.Contains(a.Id)).FirstOrDefault();
        }

        public async Task<Enterprise> GetTrendigoEnterprise()
        {
            return await context.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();
        }
    }
}
