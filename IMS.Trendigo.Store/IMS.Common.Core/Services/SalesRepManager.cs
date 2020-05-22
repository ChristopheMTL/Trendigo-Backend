using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Services
{
    public class SalesRepManager
    {
        public Decimal GetSalesRepCommission(SalesRep salesRep, Int64 enterpriseId, Int64 merchantId, Decimal amount) 
        {
            if (salesRep.Contracts == null) 
            { 
                //Cannot apply commission since ther is no contract associated to the sales rep
                return 0;
            }

            if (salesRep.Contracts.Count != 1) 
            {
                //Cannot apply commission since there is more or less one contract for that sales rep
                return 0;
            }

            if (salesRep.Contracts.FirstOrDefault().CommissionRate == null) 
            { 
                //Cannot apply commission since commission rate is null
                return 0;
            }

            return Convert.ToDecimal(salesRep.Contracts.FirstOrDefault().CommissionRate) * amount;
        }
    }
}
