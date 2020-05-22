﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Utilities.PaymentAPI.Model
{
    /// <summary>
    /// Describe a notification.
    /// </summary>
    [DataContract]
    public class Notification
    {
        /// <summary>
        /// This is the unique identifier that is used to identify a specific mobile device.
        /// </summary>
        /// <value>This is the unique identifier that is used to identify a specific mobile device.</value>
        [DataMember(Name = "deviceId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "deviceId")]
        public string DeviceId { get; set; }

        /// <summary>
        /// This is the unique identifier that is used to identify the device in Google Firebase
        /// </summary>
        /// <value>This is the unique identifier that is used to identify the device in Google Firebase</value>
        [DataMember(Name = "notificationToken", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "notificationToken")]
        public string NotificationToken { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Notification {\n");
            sb.Append("  DeviceId: ").Append(DeviceId).Append("\n");
            sb.Append("  NotificationToken: ").Append(NotificationToken).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
