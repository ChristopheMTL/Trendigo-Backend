using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum CardPointHistoryReason
    {
        WebsitePromocodeInsertion = 10,
        CardTransferPointBalance = 20,
        MonthlyBonusPoints = 30,
        ReferralProgramReferrer = 40,
        ReferralProgramReferred = 50,
        PrefixPoints = 60,
        Others = 999
    }
}
