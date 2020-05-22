using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IMS.Common.Core.Services
{
    public class PricingService
    {
        private IMSEntities db = new IMSEntities();

        public async Task<List<PricingCategory>> GetPricingList()
        {
            List<PricingCategory> pricings = new List<PricingCategory>();

            pricings = await db.PricingCategories.OrderBy(a => a.Description).ToListAsync();

            return pricings;
        }
    }
}
