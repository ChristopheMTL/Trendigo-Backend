using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum PromotionScheduleForecastEnum
    {
        OnHold = 0,
        WaitingForApproval = 1,
        Accepted = 2,
        Refused = 3,
        DoNotProcess = 99
    }
}
