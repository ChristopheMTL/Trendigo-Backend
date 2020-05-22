using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Entities
{
    [Serializable]
    public class SessionKeyData
    {
        public string session_key { get; set; }
        public string timestamp { get; set; }
        public string hash { get; set; }
    }
}
