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
    /// Describes a Processor in the system.
    /// </summary>
    [DataContract]
    public class Processor
    {
        /// <summary>
        /// The is the unique Processor identifier
        /// </summary>
        /// <value>The is the unique Processor identifier</value>
        [DataMember(Name = "processorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "processorId")]
        public int? ProcessorId { get; set; }

        /// <summary>
        /// This field represent a short description of the Processor name.
        /// </summary>
        /// <value>This field represent a short description of the Processor name.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Processor.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Processor.</value>
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
            sb.Append("class Processor {\n");
            sb.Append("  ProcessorId: ").Append(ProcessorId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
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
