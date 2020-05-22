using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class TokenDTO
    {
        public string jti { get; set; }
        public string signedJwt { get; set; }
    }
}
