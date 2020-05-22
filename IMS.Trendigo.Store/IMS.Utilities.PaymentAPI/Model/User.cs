using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a User in the system.
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// The is the unique User identifier
        /// </summary>
        /// <value>The is the unique User identifier</value>
        [DataMember(Name = "userId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "userId")]
        public int? UserId { get; set; }

        /// <summary>
        /// This is the Merchant identifier for which the user will be assign to.
        /// </summary>
        /// <value>This is the Merchant identifier for which the user will be assign to.</value>
        [DataMember(Name = "merchantId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "merchantId")]
        public int? MerchantId { get; set; }

        /// <summary>
        /// The user role in the system
        /// </summary>
        /// <value>The user role in the system</value>
        [DataMember(Name = "role", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        /// <summary>
        /// This will be used to select the language associate with this User and will allow sending the notifications in the proper language.
        /// </summary>
        /// <value>This will be used to select the language associate with this User and will allow sending the notifications in the proper language.</value>
        [DataMember(Name = "locale", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        /// <summary>
        /// This field represent a short description of the User name.
        /// </summary>
        /// <value>This field represent a short description of the User name.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// This field represent the User email address. This value must be unique in the system.
        /// </summary>
        /// <value>This field represent the User email address. This value must be unique in the system.</value>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

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
            sb.Append("class User {\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  MerchantId: ").Append(MerchantId).Append("\n");
            sb.Append("  Role: ").Append(Role).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
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
