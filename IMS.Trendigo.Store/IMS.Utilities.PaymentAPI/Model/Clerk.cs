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
    /// Describes a Clerk in the system.
    /// </summary>
    [DataContract]
    public class Clerk
    {
        /// <summary>
        /// The is the unique Clerk identifier
        /// </summary>
        /// <value>The is the unique Clerk identifier</value>
        [DataMember(Name = "clerkId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "clerkId")]
        public int? ClerkId { get; set; }

        /// <summary>
        /// The Clerk name
        /// </summary>
        /// <value>The Clerk name</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Clerk {\n");
            sb.Append("  ClerkId: ").Append(ClerkId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
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
