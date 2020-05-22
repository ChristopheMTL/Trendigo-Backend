using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a purchase response.
  /// </summary>
  [DataContract]
  public class PurchaseResponse {
    /// <summary>
    /// The transaction unique identifier.
    /// </summary>
    /// <value>The transaction unique identifier.</value>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public long? TransactionId { get; set; }

    /// <summary>
    /// The transaction authorization code obtain from the Payment Processor.
    /// </summary>
    /// <value>The transaction authorization code obtain from the Payment Processor.</value>
    [DataMember(Name="authorizationCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "authorizationCode")]
    public string AuthorizationCode { get; set; }

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
    /// The authorized amount in cents (including taxes and tips).
    /// </summary>
    /// <value>The authorized amount in cents (including taxes and tips).</value>
    [DataMember(Name="authorizedAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "authorizedAmount")]
    public long? AuthorizedAmount { get; set; }

    /// <summary>
    /// The number of points used during the transaction.
    /// </summary>
    /// <value>The number of points used during the transaction.</value>
    [DataMember(Name="pointExpended", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointExpended")]
    public int? PointExpended { get; set; }

    /// <summary>
    /// The number of points gained during the transaction.
    /// </summary>
    /// <value>The number of points gained during the transaction.</value>
    [DataMember(Name="pointGained", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointGained")]
    public int? PointGained { get; set; }

    /// <summary>
    /// The current remaining number of points after the transaction.
    /// </summary>
    /// <value>The current remaining number of points after the transaction.</value>
    [DataMember(Name="pointBalance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointBalance")]
    public int? PointBalance { get; set; }

    /// <summary>
    /// A unique order identifier. This number of the property of the merchant.
    /// </summary>
    /// <value>A unique order identifier. This number of the property of the merchant.</value>
    [DataMember(Name="orderNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "orderNumber")]
    public string OrderNumber { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PurchaseResponse {\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  AuthorizationCode: ").Append(AuthorizationCode).Append("\n");
      sb.Append("  TransactionStatus: ").Append(TransactionStatus).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  AuthorizedAmount: ").Append(AuthorizedAmount).Append("\n");
      sb.Append("  PointExpended: ").Append(PointExpended).Append("\n");
      sb.Append("  PointGained: ").Append(PointGained).Append("\n");
      sb.Append("  PointBalance: ").Append(PointBalance).Append("\n");
      sb.Append("  OrderNumber: ").Append(OrderNumber).Append("\n");
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
