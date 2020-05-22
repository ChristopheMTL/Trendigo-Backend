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
    /// Describes a MerchantProcessor in the system.
    /// </summary>
    [DataContract]
    public class MerchantProcessor
    {
        /// <summary>
        /// This field represent the unique MerchantProcessor identifier.
        /// </summary>
        /// <value>This field represent the unique MerchantProcessor identifier.</value>
        [DataMember(Name = "merchantProcessorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "merchantProcessorId")]
        public int? MerchantProcessorId { get; set; }

        /// <summary>
        /// This field represents the Merchant identifier associated in this relation.
        /// </summary>
        /// <value>This field represents the Merchant identifier associated in this relation.</value>
        [DataMember(Name = "merchantId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "merchantId")]
        public int? MerchantId { get; set; }

        /// <summary>
        /// This field represents the Processor identifier associated in this relation.
        /// </summary>
        /// <value>This field represents the Processor identifier associated in this relation.</value>
        [DataMember(Name = "processorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "processorId")]
        public int? ProcessorId { get; set; }

        /// <summary>
        /// This field represents the discriminator that lets choose the right processor (This option is not used when doing an update)
        /// </summary>
        /// <value>This field represents the discriminator that lets choose the right processor (This option is not used when doing an update)</value>
        [DataMember(Name = "processorSelectorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "processorSelectorId")]
        public string ProcessorSelectorId { get; set; }

        /// <summary>
        /// This field represents the merchant account login.
        /// </summary>
        /// <value>This field represents the merchant account login.</value>
        [DataMember(Name = "merchantLogin", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "merchantLogin")]
        public string MerchantLogin { get; set; }

        /// <summary>
        /// This field represent the merchant account password.
        /// </summary>
        /// <value>This field represent the merchant account password.</value>
        [DataMember(Name = "merchantPassword", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "merchantPassword")]
        public string MerchantPassword { get; set; }

        /// <summary>
        /// This field is pointing to the processor selector id
        /// </summary>
        /// <value>This field is pointing to the processor selector id</value>
        [DataMember(Name = "processorSelectorIdentity", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "processorSelectorIdentity")]
        public int? ProcessorSelectorIdentity { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MerchantProcessor {\n");
            sb.Append("  MerchantProcessorId: ").Append(MerchantProcessorId).Append("\n");
            sb.Append("  MerchantId: ").Append(MerchantId).Append("\n");
            sb.Append("  ProcessorId: ").Append(ProcessorId).Append("\n");
            sb.Append("  ProcessorSelectorId: ").Append(ProcessorSelectorId).Append("\n");
            sb.Append("  MerchantLogin: ").Append(MerchantLogin).Append("\n");
            sb.Append("  MerchantPassword: ").Append(MerchantPassword).Append("\n");
            sb.Append("  ProcessorSelectorIdentity: ").Append(ProcessorSelectorIdentity).Append("\n");
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
