using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum CommandStatusEnum
    {
        NotProcessed = 0,
        Success = 1,
        TransaxFailed = 2,
        IMSFailed = 3
    }
}
