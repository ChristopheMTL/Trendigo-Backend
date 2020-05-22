using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Location in the system.
    /// </summary>
    [DataContract]
    public class Location
    {
        /// <summary>
        /// The is the unique Location identifier
        /// </summary>
        /// <value>The is the unique Location identifier</value>
        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locationId")]
        public int? LocationId { get; set; }

        /// <summary>
        /// The is the unique Merchant identifier associate with this Location.
        /// </summary>
        /// <value>The is the unique Merchant identifier associate with this Location.</value>
        [DataMember(Name = "merchantId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "merchantId")]
        public int? MerchantId { get; set; }

        /// <summary>
        /// This flag indicates if we have to apply taxes for this Merchant.
        /// </summary>
        /// <value>This flag indicates if we have to apply taxes for this Merchant.</value>
        [DataMember(Name = "applyTaxes", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "applyTaxes")]
        public bool? ApplyTaxes { get; set; }

        /// <summary>
        /// This flag indicates if we have to ask for a tip amount during the purchase transaction.
        /// </summary>
        /// <value>This flag indicates if we have to ask for a tip amount during the purchase transaction.</value>
        [DataMember(Name = "applyTips", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "applyTips")]
        public bool? ApplyTips { get; set; }

        /// <summary>
        /// This flag indicates if it is possible in this location for the member to pay with his points.
        /// </summary>
        /// <value>This flag indicates if it is possible in this location for the member to pay with his points.</value>
        [DataMember(Name = "payWithPoints", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "payWithPoints")]
        public bool? PayWithPoints { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Location.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Location.</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// This field represents the processor linked to this location
        /// </summary>
        /// <value>This field represents the processor linked to this location</value>
        [DataMember(Name = "processorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "processorId")]
        public int? ProcessorId { get; set; }

        /// <summary>
        /// Gets or Sets LocationInformation
        /// </summary>
        [DataMember(Name = "locationInformation", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locationInformation")]
        public LocationInformation LocationInformation { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Location {\n");
            sb.Append("  LocationId: ").Append(LocationId).Append("\n");
            sb.Append("  MerchantId: ").Append(MerchantId).Append("\n");
            sb.Append("  ApplyTaxes: ").Append(ApplyTaxes).Append("\n");
            sb.Append("  ApplyTips: ").Append(ApplyTips).Append("\n");
            sb.Append("  PayWithPoints: ").Append(PayWithPoints).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ProcessorId: ").Append(ProcessorId).Append("\n");
            sb.Append("  LocationInformation: ").Append(LocationInformation).Append("\n");
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
