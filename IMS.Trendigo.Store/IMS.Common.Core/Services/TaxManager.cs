using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class TaxManager
    {
        public decimal ApplyTaxToAmount(Enterprise enterprise, decimal amount)
        {
            decimal applicabletaxes = 0;

            if (enterprise.Address.State.StateTaxes != null)
            {
                foreach (StateTax statetax in enterprise.Address.State.StateTaxes.Where(a => a.IsActive == true).OrderBy(b => b.Priority).ToList())
                {
                    applicabletaxes += Convert.ToDecimal(statetax.Value);
                }
            }

            return amount + applicabletaxes;
        }

        public decimal ApplyTaxToAmount(Merchant merchant, decimal amount)
        {
            decimal applicabletaxes = 0;

            if (merchant.Locations.FirstOrDefault(a => a.IsActive == true).Address.State.StateTaxes != null)
            {
                foreach (StateTax statetax in merchant.Locations.FirstOrDefault(a => a.IsActive == true).Address.State.StateTaxes.Where(a => a.IsActive == true).OrderBy(b => b.Priority).ToList())
                {
                    applicabletaxes += Convert.ToDecimal(statetax.Value);
                }
            }

            return amount + applicabletaxes;
        }
    }
}
