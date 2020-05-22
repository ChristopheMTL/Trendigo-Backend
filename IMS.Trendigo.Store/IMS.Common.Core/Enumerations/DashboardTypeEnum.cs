using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum DashboardTypeEnum
    {
        Sale = 1,
        Transaction = 2,
        PointEarned = 3,
        PointSpent = 4,
        Member = 5,
        NewMember = 6,
        Merchant = 7,
        NewMerchant = 8,
        AvgMoneySpentPerTrans = 9,
        AvgPointUsePerTrans = 10,
        AvgPointEarnedPerTrans = 11,
        BestMerchant = 12,
        WorstMercant = 13,
        AvgSpentPerVisit = 14
    }
}
