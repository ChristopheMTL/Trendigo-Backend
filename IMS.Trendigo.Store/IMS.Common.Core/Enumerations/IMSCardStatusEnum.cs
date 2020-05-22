using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum IMSCardStatus
    {
        INVENTORY = 1,
        ASSIGNED = 2,
        SHIPPED = 3,
        ACTIVATED = 4,
        EXPIRED = 5,
        UNUSABLE = 6,
        DEMO = 7,
        UNASSIGNABLE = 8,
        STOLEN_LOST = 99
    }
}
