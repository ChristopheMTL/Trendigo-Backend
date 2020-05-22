using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Trendigo Point in the system.
    /// </summary>
    [DataContract]
    public class Point
    {
        /// <summary>
        /// The number of points associate with the operation. If the operation is add and the value is 10, the member total number of points will be increased by 10.
        /// </summary>
        /// <value>The number of points associate with the operation. If the operation is add and the value is 10, the member total number of points will be increased by 10.</value>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "value")]
        public int? Value { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Point {\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
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
