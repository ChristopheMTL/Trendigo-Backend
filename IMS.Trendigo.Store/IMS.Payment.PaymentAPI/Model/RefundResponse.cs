using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a refund response.
  /// </summary>
  [DataContract]
  public class RefundResponse {
    /// <summary>
    /// The refund transaction non financial Id
    /// </summary>
    /// <value>The refund transaction non financial Id</value>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public long? TransactionId { get; set; }

    /// <summary>
    /// The number of points removed during the transaction.
    /// </summary>
    /// <value>The number of points removed during the transaction.</value>
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
    /// The transaction total (including the taxes and the tips).
    /// </summary>
    /// <value>The transaction total (including the taxes and the tips).</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public long? Amount { get; set; }

    /// <summary>
    /// The transaction refunded amount (this amount might differs from the requested amount if the member points can not be recovered).
    /// </summary>
    /// <value>The transaction refunded amount (this amount might differs from the requested amount if the member points can not be recovered).</value>
    [DataMember(Name="refundedAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "refundedAmount")]
    public long? RefundedAmount { get; set; }

    /// <summary>
    /// The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR. If not provided, the currency of the original transaction will be use.
    /// </summary>
    /// <value>The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR. If not provided, the currency of the original transaction will be use.</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// The transaction confirmation code obtain from the Payment Processor.
    /// </summary>
    /// <value>The transaction confirmation code obtain from the Payment Processor.</value>
    [DataMember(Name="confirmationCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "confirmationCode")]
    public string ConfirmationCode { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RefundResponse {\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  PointRemoved: ").Append(PointRemoved).Append("\n");
      sb.Append("  PointBalance: ").Append(PointBalance).Append("\n");
      sb.Append("  TransactionStatus: ").Append(TransactionStatus).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  RefundedAmount: ").Append(RefundedAmount).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  ConfirmationCode: ").Append(ConfirmationCode).Append("\n");
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
