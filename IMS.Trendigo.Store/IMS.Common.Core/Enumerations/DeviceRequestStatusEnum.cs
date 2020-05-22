using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum DeviceRequestStatus
    {
        DEACTIVATED = 0,
        REQUESTED = 1,
        ASSIGNED = 2,
        PROCESSED = 3,
        SHIPPED = 4,
        ACTIVATED = 5,
        REJECTED = 99
    }
}
