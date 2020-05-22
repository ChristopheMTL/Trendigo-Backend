using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Terminal in the system.
    /// </summary>
    [DataContract]
    public class Terminal
    {
        /// <summary>
        /// This field represents the unique Terminal identifier.
        /// </summary>
        /// <value>This field represents the unique Terminal identifier.</value>
        [DataMember(Name = "terminalId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "terminalId")]
        public int? TerminalId { get; set; }

        /// <summary>
        /// This field represents the unique Location identifier.
        /// </summary>
        /// <value>This field represents the unique Location identifier.</value>
        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locationId")]
        public int? LocationId { get; set; }

        /// <summary>
        /// This field represents the logical Terminal Number defined for its associate Processor.
        /// </summary>
        /// <value>This field represents the logical Terminal Number defined for its associate Processor.</value>
        [DataMember(Name = "terminalNumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "terminalNumber")]
        public string TerminalNumber { get; set; }

        /// <summary>
        /// This field represent the Serial Number associate with the logical Terminal Number. There could be only one Terminal active at the same time with this Serial Number.
        /// </summary>
        /// <value>This field represent the Serial Number associate with the logical Terminal Number. There could be only one Terminal active at the same time with this Serial Number.</value>
        [DataMember(Name = "pinpadSerialNumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pinpadSerialNumber")]
        public string PinpadSerialNumber { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Terminal.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Terminal.</value>
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
            sb.Append("class Terminal {\n");
            sb.Append("  TerminalId: ").Append(TerminalId).Append("\n");
            sb.Append("  LocationId: ").Append(LocationId).Append("\n");
            sb.Append("  TerminalNumber: ").Append(TerminalNumber).Append("\n");
            sb.Append("  PinpadSerialNumber: ").Append(PinpadSerialNumber).Append("\n");
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
