using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Service.WebAPI2.Models
{
    public class NotificationRS
    {
        public string deviceId { get; set; }

        public string notificationToken { get; set; }

        public DateTime creationDate { get; set; }
    }
}