using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Utilities.PaymentAPI.Model
{
    /// <summary>
    /// Describes a processor credential in the system.
    /// </summary>
    [DataContract]
    public class ProcessorCredential
    {
        /// <summary>
        /// This field represent the Processor username.
        /// </summary>
        /// <value>This field represent the Processor username.</value>
        [DataMember(Name = "username", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// This field represents the Processor password.
        /// </summary>
        /// <value>This field represents the Processor password.</value>
        [DataMember(Name = "password", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ProcessorCredential {\n");
            sb.Append("  Username: ").Append(Username).Append("\n");
            sb.Append("  Password: ").Append(Password).Append("\n");
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
