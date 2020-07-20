using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a Purchase Validation request.
  /// </summary>
  [DataContract]
  public class PurchaseValidationRequest {
    /// <summary>
    /// The Location unique identifier.
    /// </summary>
    /// <value>The Location unique identifier.</value>
    [DataMember(Name="locationId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "locationId")]
    public int? LocationId { get; set; }

    /// <summary>
    /// The clerk identifier.
    /// </summary>
    /// <value>The clerk identifier.</value>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public long UserId { get; set; }

    /// <summary>
    /// The transaction total in cents (including the taxes).
    /// </summary>
    /// <value>The transaction total in cents (including the taxes).</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public decimal? Amount { get; set; }

    /// <summary>
    /// The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR.
    /// </summary>
    /// <value>The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR.</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

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
      sb.Append("class PurchaseValidationRequest {\n");
      sb.Append("  LocationId: ").Append(LocationId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
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
