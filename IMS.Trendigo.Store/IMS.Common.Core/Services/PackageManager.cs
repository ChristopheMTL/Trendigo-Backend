using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IMS.Common.Core.Services
{
    public class PackageManager
    {
        IMSEntities context = new IMSEntities();

        public List<Package> GetPackageWithRole(IMSUser imsUser, String EnterpriseId, Boolean ActiveOnly = false)
        {
            List<Package> packages = GetPackageWithRole(imsUser, ActiveOnly);

            return packages.Where(a => a.EnterpriseId.ToString() == EnterpriseId).ToList();
        }

        public List<Package> GetPackageWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<Package> packages = new List<Package>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                packages = context.Packages.ToList();
                if (ActiveOnly)
                    packages = packages.Where(a => a.IsActive == true).ToList();
                return packages;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                packages = context.Packages.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (ActiveOnly)
                packages = packages.Where(a => a.IsActive == true).ToList();

            return packages;
        }

        public List<PackageType> GetPackageTypeWithRole(IMSUser user, String EnterpriseId, Boolean ActiveOnly = false)
        {
            List<PackageType> packageTypes = GetPackageTypesWithRole(user, ActiveOnly);

            return packageTypes.Where(a => a.EnterpriseId.ToString() == EnterpriseId).ToList();
        }

        public List<PackageType> GetPackageTypesWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<PackageType> packageTypes = new List<PackageType>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                packageTypes = context.PackageTypes.ToList();
                if (ActiveOnly)
                    packageTypes = packageTypes.Where(a => a.IsActive == true).ToList();
                return packageTypes;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                packageTypes = context.PackageTypes.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (ActiveOnly)
                packageTypes = packageTypes.Where(a => a.IsActive == true).ToList();

            return packageTypes;
        }

        public PackageType GetPackageTypeWithRole(IMSUser user, String packageTypeId)
        {
            List<PackageType> packageTypes = GetPackageTypesWithRole(user);
            var listOfPackageTypeId = packageTypes.Select(m => m.Id);

            return context.PackageTypes.Where(a => a.Id.ToString() == packageTypeId).Where(a => listOfPackageTypeId.Contains(a.Id)).FirstOrDefault();
        }

        public Decimal GetPackageCommissionPercentage(Contract contract)
        {
            Decimal commission = 0;

            if (contract != null)
            {
                commission = Convert.ToDecimal(contract.Packages.Where(a => a.IsActive == true).FirstOrDefault().PackageType.MinCommissionAllYear);
            }
            else //No contract, we take the base commission
            {
                Decimal baseCommission = Convert.ToDecimal(ConfigurationManager.AppSettings["minPercentageCommission"]);
                commission = Convert.ToDecimal(baseCommission);
            }

            return commission >= 1 ? commission / 100 : commission;
        }
    }
}
