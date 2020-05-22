using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Currency Rate in the system.
    /// </summary>
    [DataContract]
    public class CurrencyRate
    {
        /// <summary>
        /// The is the unique Currency Rate value identifier
        /// </summary>
        /// <value>The is the unique Currency Rate value identifier</value>
        [DataMember(Name = "currencyRateId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currencyRateId")]
        public int? CurrencyRateId { get; set; }

        /// <summary>
        /// This field represent the first Currency. This identifier could correspond to the CND dollar (TODO: evaluate if its not preferable to use enumeration)
        /// </summary>
        /// <value>This field represent the first Currency. This identifier could correspond to the CND dollar (TODO: evaluate if its not preferable to use enumeration)</value>
        [DataMember(Name = "currencyId1", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currencyId1")]
        public int? CurrencyId1 { get; set; }

        /// <summary>
        /// This field represent the second Currency. This identifier could correspond to the USD dollar (TODO: evaluate if its not preferable to use enumeration)
        /// </summary>
        /// <value>This field represent the second Currency. This identifier could correspond to the USD dollar (TODO: evaluate if its not preferable to use enumeration)</value>
        [DataMember(Name = "currencyId2", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currencyId2")]
        public int? CurrencyId2 { get; set; }

        /// <summary>
        /// This field represent the start date from whith the Currency Rate is applied
        /// </summary>
        /// <value>This field represent the start date from whith the Currency Rate is applied</value>
        [DataMember(Name = "startDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// This field represent the end date from which the Currency Rate is applied
        /// </summary>
        /// <value>This field represent the end date from which the Currency Rate is applied</value>
        [DataMember(Name = "endDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// This field represent the conversion rate that applies between the two Currencies that are implicated.
        /// </summary>
        /// <value>This field represent the conversion rate that applies between the two Currencies that are implicated.</value>
        [DataMember(Name = "rate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "rate")]
        public double? Rate { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CurrencyRate {\n");
            sb.Append("  CurrencyRateId: ").Append(CurrencyRateId).Append("\n");
            sb.Append("  CurrencyId1: ").Append(CurrencyId1).Append("\n");
            sb.Append("  CurrencyId2: ").Append(CurrencyId2).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  Rate: ").Append(Rate).Append("\n");
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
