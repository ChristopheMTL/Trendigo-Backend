using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class MonthlyPointNotificationDTO
    {
        public long MemberId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        public int total { get; set; }
    }
}
