using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Program in the system.
    /// </summary>
    [DataContract]
    public class Program
    {
        /// <summary>
        /// This field represent the unique Program identifier.
        /// </summary>
        /// <value>This field represent the unique Program identifier.</value>
        [DataMember(Name = "programId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "programId")]
        public int? ProgramId { get; set; }

        /// <summary>
        /// This field represent the Program name.
        /// </summary>
        /// <value>This field represent the Program name.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// This field represent the Currency associate with the Program.
        /// </summary>
        /// <value>This field represent the Currency associate with the Program.</value>
        [DataMember(Name = "currencyId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currencyId")]
        public int? CurrencyId { get; set; }

        /// <summary>
        /// This field represents the unique Enterprise identifier (note that there is no Enterprise API at this moment).
        /// </summary>
        /// <value>This field represents the unique Enterprise identifier (note that there is no Enterprise API at this moment).</value>
        [DataMember(Name = "enterpriseId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enterpriseId")]
        public int? EnterpriseId { get; set; }

        /// <summary>
        /// This field represents the percentage that applied on the Transaction.
        /// </summary>
        /// <value>This field represents the percentage that applied on the Transaction.</value>
        [DataMember(Name = "fidelityRewardPercent", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "fidelityRewardPercent")]
        public double? FidelityRewardPercent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [DataMember(Name = "loyaltyCostUsingPoints", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "loyaltyCostUsingPoints")]
        public int? LoyaltyCostUsingPoints { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [DataMember(Name = "loyaltyValueGainingPoints", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "loyaltyValueGainingPoints")]
        public int? LoyaltyValueGainingPoints { get; set; }

        /// <summary>
        /// The type of Program
        /// </summary>
        /// <value>The type of Program</value>
        [DataMember(Name = "programType", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "programType")]
        public string ProgramType { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Program.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Program.</value>
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
            sb.Append("class Program {\n");
            sb.Append("  ProgramId: ").Append(ProgramId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  CurrencyId: ").Append(CurrencyId).Append("\n");
            sb.Append("  EnterpriseId: ").Append(EnterpriseId).Append("\n");
            sb.Append("  FidelityRewardPercent: ").Append(FidelityRewardPercent).Append("\n");
            sb.Append("  LoyaltyCostUsingPoints: ").Append(LoyaltyCostUsingPoints).Append("\n");
            sb.Append("  LoyaltyValueGainingPoints: ").Append(LoyaltyValueGainingPoints).Append("\n");
            sb.Append("  ProgramType: ").Append(ProgramType).Append("\n");
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
