using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a QR Code Generation request.
  /// </summary>
  [DataContract]
  public class QrcodeGenerationRequest {
    /// <summary>
    /// Identifies where the transaction takes place.
    /// </summary>
    /// <value>Identifies where the transaction takes place.</value>
    [DataMember(Name="locationId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "locationId")]
    public long? LocationId { get; set; }

    /// <summary>
    /// The userId identifier. Use 0 if the QR Code is generated outside the merchant mobile application.
    /// </summary>
    /// <value>The userId identifier. Use 0 if the QR Code is generated outside the merchant mobile application.</value>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public long UserId { get; set; }

    /// <summary>
    /// The amount in cents (excluding the taxes and tips). If the amount is set to 0 (zero), it mean that we are dealing with a static merchant QR Code and the application must be able to specify the amount.
    /// </summary>
    /// <value>The amount in cents (excluding the taxes and tips). If the amount is set to 0 (zero), it mean that we are dealing with a static merchant QR Code and the application must be able to specify the amount.</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public long? Amount { get; set; }

    /// <summary>
    /// A unique order identifier. This number of the property of the merchant.
    /// </summary>
    /// <value>A unique order identifier. This number of the property of the merchant.</value>
    [DataMember(Name="orderNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "orderNumber")]
    public string OrderNumber { get; set; }

    /// <summary>
    /// This flag indicates if we have to ask for a tip amount during the purchase transaction.
    /// </summary>
    /// <value>This flag indicates if we have to ask for a tip amount during the purchase transaction.</value>
    [DataMember(Name="supportTips", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportTips")]
    public bool? SupportTips { get; set; }

    /// <summary>
    /// This flag indicates if its possible for this transaction to pay with the members points.
    /// </summary>
    /// <value>This flag indicates if its possible for this transaction to pay with the members points.</value>
    [DataMember(Name="payWithPoints", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payWithPoints")]
    public bool? PayWithPoints { get; set; }

    /// <summary>
    /// The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR.
    /// </summary>
    /// <value>The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR.</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets QrcodeInformation
    /// </summary>
    [DataMember(Name="qrcodeInformation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "qrcodeInformation")]
    public QrcodeInformation QrcodeInformation { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class QrcodeGenerationRequest {\n");
      sb.Append("  LocationId: ").Append(LocationId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  OrderNumber: ").Append(OrderNumber).Append("\n");
      sb.Append("  SupportTips: ").Append(SupportTips).Append("\n");
      sb.Append("  PayWithPoints: ").Append(PayWithPoints).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  QrcodeInformation: ").Append(QrcodeInformation).Append("\n");
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
