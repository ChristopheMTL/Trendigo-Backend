using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a void response.
  /// </summary>
  [DataContract]
  public class VoidResponse {
    /// <summary>
    /// The refund transaction unique identifier.
    /// </summary>
    /// <value>The refund transaction unique identifier.</value>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public long? TransactionId { get; set; }

    /// <summary>
    /// The number of points removed by the void transaction.
    /// </summary>
    /// <value>The number of points removed by the void transaction.</value>
    [DataMember(Name="pointRemoved", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointRemoved")]
    public int? PointRemoved { get; set; }

    /// <summary>
    /// The current remaining number of points after the transaction.
    /// </summary>
    /// <value>The current remaining number of points after the transaction.</value>
    [DataMember(Name="pointBalance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointBalance")]
    public int? PointBalance { get; set; }

    /// <summary>
    /// The transaction voided amount.
    /// </summary>
    /// <value>The transaction voided amount.</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public long? Amount { get; set; }

    /// <summary>
    /// The transaction voided amount (this amount might differs from the requested amount if the member points can not be recovered)
    /// </summary>
    /// <value>The transaction voided amount (this amount might differs from the requested amount if the member points can not be recovered)</value>
    [DataMember(Name="voidedAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "voidedAmount")]
    public long? VoidedAmount { get; set; }

    /// <summary>
    /// Gets or Sets TransactionStatus
    /// </summary>
    [DataMember(Name="transactionStatus", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionStatus")]
    public string TransactionStatus { get; set; }

    /// <summary>
    /// A description of the transaction result in the language passed in the request.
    /// </summary>
    /// <value>A description of the transaction result in the language passed in the request.</value>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    /// <summary>
    /// The transaction authorization code obtain from the Payment Processor.
    /// </summary>
    /// <value>The transaction authorization code obtain from the Payment Processor.</value>
    [DataMember(Name="autorizationCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "autorizationCode")]
    public string AutorizationCode { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VoidResponse {\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  PointRemoved: ").Append(PointRemoved).Append("\n");
      sb.Append("  PointBalance: ").Append(PointBalance).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  VoidedAmount: ").Append(VoidedAmount).Append("\n");
      sb.Append("  TransactionStatus: ").Append(TransactionStatus).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  AutorizationCode: ").Append(AutorizationCode).Append("\n");
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
