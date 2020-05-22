using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Merchant in the system.
    /// </summary>
    [DataContract]
    public class Merchant
    {
        /// <summary>
        /// This field represent the unique Merchant identifier.
        /// </summary>
        /// <value>This field represent the unique Merchant identifier.</value>
        [DataMember(Name = "merchantId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "merchantId")]
        public int? MerchantId { get; set; }

        /// <summary>
        /// This field represent the program to witch the Merchant is associate with.
        /// </summary>
        /// <value>This field represent the program to witch the Merchant is associate with.</value>
        [DataMember(Name = "programId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "programId")]
        public int? ProgramId { get; set; }

        /// <summary>
        /// This field represent a short description of the Merchant name.
        /// </summary>
        /// <value>This field represent a short description of the Merchant name.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// This field represent the enterprise to witch the Merchant is associate with.
        /// </summary>
        /// <value>This field represent the enterprise to witch the Merchant is associate with.</value>
        [DataMember(Name = "enterpriseId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enterpriseId")]
        public int? EnterpriseId { get; set; }

        /// <summary>
        /// The merchant communication language.
        /// </summary>
        /// <value>The merchant communication language.</value>
        [DataMember(Name = "locale", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        /// <summary>
        /// This is the url of the merchant logo that will be displayed on the customer mobile application.
        /// </summary>
        /// <value>This is the url of the merchant logo that will be displayed on the customer mobile application.</value>
        [DataMember(Name = "logoUrl", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "logoUrl")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
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
            sb.Append("class Merchant {\n");
            sb.Append("  MerchantId: ").Append(MerchantId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  EnterpriseId: ").Append(EnterpriseId).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
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
