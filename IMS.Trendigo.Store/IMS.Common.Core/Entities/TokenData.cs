using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Entities
{
    [Serializable]
    public class TokenData
    {
        public string restcode { get; set; }
        public string restmessage { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string admin { get; set; }
    }
}
