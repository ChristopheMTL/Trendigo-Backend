using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class UserDTO
    {
        public long userId { get; set; }
        public string language { get; set; }
        public long merchantId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public List<UserNotificationDTO> notifications { get; set; }
    }

    public class UserNotificationDTO
    {
        public string deviceId { get; set; }
        public string notificationToken { get; set; }
    }
}
