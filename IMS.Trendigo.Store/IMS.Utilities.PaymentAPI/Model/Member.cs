using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Member in the system.
    /// </summary>
    [DataContract]
    public class Member
    {
        /// <summary>
        /// The is the unique Member identifier.
        /// </summary>
        /// <value>The is the unique Member identifier.</value>
        [DataMember(Name = "memberId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "memberId")]
        public int? MemberId { get; set; }

        /// <summary>
        /// This field represent a short description of the Member name.
        /// </summary>
        /// <value>This field represent a short description of the Member name.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// This field represent the Member email address.
        /// </summary>
        /// <value>This field represent the Member email address.</value>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// This will be used to select the languages associate with this Member. This will be used to send the notifications in the proper language.
        /// </summary>
        /// <value>This will be used to select the languages associate with this Member. This will be used to send the notifications in the proper language.</value>
        [DataMember(Name = "locale", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or Sets Notifications
        /// </summary>
        [DataMember(Name = "notifications", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "notifications")]
        public List<Notification> Notifications { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Member.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Member.</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Member {\n");
            sb.Append("  MemberId: ").Append(MemberId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("  Notifications: ").Append(Notifications).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
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
