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
    public class HistoryService
    {
        IMSEntities context = new IMSEntities();

        public List<IMS_Header> GetHeaderListWithRole(IMSUser user)
        {
            List<IMS_Header> headers = new List<IMS_Header>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                headers = context.IMS_Header.Where(a => a.LocationId.HasValue).OrderByDescending(a => a.CreationDate).ToList();

                return headers;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                headers = context.IMS_Header.Where(a => a.LocationId.HasValue && a.Location.Merchant.Enterprises.Any(b => b.Id == user.Id)).OrderByDescending(a => a.CreationDate).ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                headers = context.IMS_Header.Where(a => a.MerchantId.HasValue && a.Merchant.IMSUsers.Any(b => b.UserId == user.UserId)).OrderByDescending(a => a.CreationDate).ToList();
            }

            return headers;
        }
    }
}
