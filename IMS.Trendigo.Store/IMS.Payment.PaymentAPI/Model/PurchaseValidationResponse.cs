using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a Purchase Validation response.
  /// </summary>
  [DataContract]
  public class PurchaseValidationResponse {
    /// <summary>
    /// The JWT Transaction Token that requires to be passed in the future payment transactions.
    /// </summary>
    /// <value>The JWT Transaction Token that requires to be passed in the future payment transactions.</value>
    [DataMember(Name="transactionToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionToken")]
    public string TransactionToken { get; set; }

    /// <summary>
    /// The current remaining number of points before performing the transaction.
    /// </summary>
    /// <value>The current remaining number of points before performing the transaction.</value>
    [DataMember(Name="pointBalance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointBalance")]
    public int? PointBalance { get; set; }

    /// <summary>
    /// Its a UUID that uniquely identifies this transaction.
    /// </summary>
    /// <value>Its a UUID that uniquely identifies this transaction.</value>
    [DataMember(Name="orderId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "orderId")]
    public string OrderId { get; set; }

    /// <summary>
    /// This flag indicates if we have to apply taxes for this Merchant.
    /// </summary>
    /// <value>This flag indicates if we have to apply taxes for this Merchant.</value>
    [DataMember(Name="applyTaxes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyTaxes")]
    public bool? ApplyTaxes { get; set; }

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
    /// A description of the validation result in the language passed in the request.
    /// </summary>
    /// <value>A description of the validation result in the language passed in the request.</value>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    /// <summary>
    /// This is the url of the merchant logo that will be displayed on the customer mobile application.
    /// </summary>
    /// <value>This is the url of the merchant logo that will be displayed on the customer mobile application.</value>
    [DataMember(Name="merchantLogoUrl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "merchantLogoUrl")]
    public string MerchantLogoUrl { get; set; }

    /// <summary>
    /// Gets or Sets LocationInformation
    /// </summary>
    [DataMember(Name="locationInformation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "locationInformation")]
    public LocationInformation LocationInformation { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PurchaseValidationResponse {\n");
      sb.Append("  TransactionToken: ").Append(TransactionToken).Append("\n");
      sb.Append("  PointBalance: ").Append(PointBalance).Append("\n");
      sb.Append("  OrderId: ").Append(OrderId).Append("\n");
      sb.Append("  ApplyTaxes: ").Append(ApplyTaxes).Append("\n");
      sb.Append("  SupportTips: ").Append(SupportTips).Append("\n");
      sb.Append("  PayWithPoints: ").Append(PayWithPoints).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  MerchantLogoUrl: ").Append(MerchantLogoUrl).Append("\n");
      sb.Append("  LocationInformation: ").Append(LocationInformation).Append("\n");
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
