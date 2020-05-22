using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a Refund Validation request.
  /// </summary>
  [DataContract]
  public class RefundValidationRequest {
    /// <summary>
    /// The transaction unique identifier.
    /// </summary>
    /// <value>The transaction unique identifier.</value>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public long? TransactionId { get; set; }

    /// <summary>
    /// The transaction total in cents (including the taxes and the tips). If not provided, the amount (including the taxes and the tips) of the original transaction will be use.
    /// </summary>
    /// <value>The transaction total in cents (including the taxes and the tips). If not provided, the amount (including the taxes and the tips) of the original transaction will be use.</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public long? Amount { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RefundValidationRequest {\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
