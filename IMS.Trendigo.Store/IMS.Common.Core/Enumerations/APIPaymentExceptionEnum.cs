using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum APIPaymentException
    {
        OTHER = 0,
        OK = 200,
        NOTAUTHORIZED = 401,
        NOTFOUND = 404
    }
}
