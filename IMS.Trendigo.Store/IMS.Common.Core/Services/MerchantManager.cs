using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IMS.Common.Core.Services
{
    public class MerchantManager
    {
        IMSEntities context = new IMSEntities();

        public List<Merchant> GetMerchantListWithRole(IMSUser user, String EnterpriseId, Boolean ActiveOnly = false, Boolean LocationMustBePresent = true) 
        {
            List<Merchant> merchants = GetMerchantListWithRole(user, ActiveOnly, LocationMustBePresent);

            return merchants.Where(a => a.Enterprises.Any(b => b.Id.ToString() == EnterpriseId)).ToList();
        }

        public List<Merchant> GetMerchantListWithRole(IMSUser user, Boolean ActiveOnly = false, Boolean LocationMustBePresent = true) 
        {
            List<Merchant> merchants = new List<Merchant>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString())) 
            {
                merchants = context.Merchants.ToList();
                if (ActiveOnly)
                    merchants = merchants.Where(a => a.IsActive == true).ToList();
                return merchants;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                merchants = context.Merchants.Where(a => a.Enterprises.Any(b => b.Id == user.Id)).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                merchants = context.Merchants.Where(a => a.Locations.Any(b => b.Merchant.IMSUsers.Any(c => c.Id == user.Id))).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SalesRep.ToString()))
            {
                merchants = context.Merchants.Where(a => a.Locations.Any(b => b.ContractLocations.Any(c => c.Contract.SalesRepId == user.Id))).Distinct().ToList();
            }

            if (ActiveOnly)
                merchants = merchants.Where(a => a.IsActive == true).ToList();

            if (LocationMustBePresent)
                merchants = merchants.Where(a => a.Locations != null).ToList();

            return merchants;
        }

        public Merchant GetMerchantWithRole(IMSUser user, String merchantId) 
        {
            List<Merchant> merchants = GetMerchantListWithRole(user);
            var listOfMerchantId = merchants.Select(m => m.Id);

            return context.Merchants.Where(a => a.Id.ToString() == merchantId).Where(a => listOfMerchantId.Contains(a.Id)).FirstOrDefault();
        }

        public String GetMerchantLogoPath(Merchant merchant)
        {
            string logoPath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Logo"];

            if (merchant != null)
            {
                if (merchant.LogoId != null)
                {
                    if (merchant.MerchantImages.Count > 0)
                    {
                        MerchantImage merchantImage = merchant.MerchantImages.FirstOrDefault(a => a.Id == merchant.LogoId);
                        if (merchantImage != null)
                        {
                            logoPath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Path"] + "/" + merchant.Id.ToString() + "/" + merchant.MerchantImages.FirstOrDefault(a => a.Id == merchant.LogoId).Filepath + "_medium.jpg";
                        }
                    }
                }
            }

            return logoPath;
        }

        public String GetMerchantLogoPath(long merchantId, string logoId)
        {
            string logoPath = "";

            if (!string.IsNullOrEmpty(logoId))
            {
                logoPath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Logo"];

                Guid guid = new Guid(logoId);
                MerchantImage merchantImage = context.MerchantImages.FirstOrDefault(a => a.Id == guid);
                if (merchantImage != null)
                {
                    logoPath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Path"] + "/" + merchantId.ToString() + "/" + merchantImage.Filepath + "_medium.jpg";
                }
            }

            return logoPath;
        }

        public String GetMerchantDefaultImage(Merchant merchant)
        {
            string imagePath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Image"];

            if (merchant != null)
            {
                if (merchant.MerchantImages.Count > 0)
                {
                    MerchantImage merchantImage = merchant.MerchantImages.Where(z => z.Id != z.Merchant.LogoId).OrderBy(a => a.Weight).FirstOrDefault();
                    if (merchantImage != null)
                    {
                        imagePath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Path"] + "/" + merchant.Id.ToString() + "/" + merchantImage.Filepath + ".jpg";
                    }
                }
            }

            return imagePath;
        }

        public String GetMerchantDefaultImage(long merchantId, string imageId)
        {
            string imagePath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Image"];

            Guid guid = new Guid(imageId);
            MerchantImage merchantImage = context.MerchantImages.FirstOrDefault(a => a.Id == guid);
            if (merchantImage != null)
            {
                imagePath = ConfigurationManager.AppSettings["IMS.Default.Merchant.Path"] + "/" + merchantId.ToString() + "/" + merchantImage.Filepath + ".jpg";
            }

            return imagePath;
        }
    }
}
