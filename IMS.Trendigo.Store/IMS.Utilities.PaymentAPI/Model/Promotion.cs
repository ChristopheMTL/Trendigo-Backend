using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Promotion in the system.
    /// </summary>
    [DataContract]
    public class Promotion
    {
        /// <summary>
        /// This field represent the unique Promotion identifier.
        /// </summary>
        /// <value>This field represent the unique Promotion identifier.</value>
        [DataMember(Name = "promotionId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "promotionId")]
        public int? PromotionId { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the type of the Promotion.
        /// </summary>
        /// <value>This field is an enumeration that represents the type of the Promotion.</value>
        [DataMember(Name = "promotionType", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "promotionType")]
        public string PromotionType { get; set; }

        /// <summary>
        /// This field represents the percentage that is applied to the promotion.
        /// </summary>
        /// <value>This field represents the percentage that is applied to the promotion.</value>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "value")]
        public double? Value { get; set; }

        /// <summary>
        /// This field represents the maximun discount applicable for the Promotion.
        /// </summary>
        /// <value>This field represents the maximun discount applicable for the Promotion.</value>
        [DataMember(Name = "maxDiscount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "maxDiscount")]
        public double? MaxDiscount { get; set; }

        /// <summary>
        /// This field represents the maximum amount that must be consider for the Promotion.
        /// </summary>
        /// <value>This field represents the maximum amount that must be consider for the Promotion.</value>
        [DataMember(Name = "maxAmount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "maxAmount")]
        public double? MaxAmount { get; set; }

        /// <summary>
        /// This field represent the Promotion initial effective start date.
        /// </summary>
        /// <value>This field represent the Promotion initial effective start date.</value>
        [DataMember(Name = "startDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// This field represent the Promotion initial effective end date.
        /// </summary>
        /// <value>This field represent the Promotion initial effective end date.</value>
        [DataMember(Name = "endDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Promotion.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Promotion.</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// This is an array containing the list of Locations identifiers for which the Promotion will be effective.
        /// </summary>
        /// <value>This is an array containing the list of Locations identifiers for which the Promotion will be effective.</value>
        [DataMember(Name = "locationIds", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locationIds")]
        public List<int?> LocationIds { get; set; }

        /// <summary>
        /// This is an array containing the list of Programs identifiers for which the Promotion will be effective.
        /// </summary>
        /// <value>This is an array containing the list of Programs identifiers for which the Promotion will be effective.</value>
        [DataMember(Name = "programIds", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "programIds")]
        public List<int?> ProgramIds { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Promotion {\n");
            sb.Append("  PromotionId: ").Append(PromotionId).Append("\n");
            sb.Append("  PromotionType: ").Append(PromotionType).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  MaxDiscount: ").Append(MaxDiscount).Append("\n");
            sb.Append("  MaxAmount: ").Append(MaxAmount).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  LocationIds: ").Append(LocationIds).Append("\n");
            sb.Append("  ProgramIds: ").Append(ProgramIds).Append("\n");
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
