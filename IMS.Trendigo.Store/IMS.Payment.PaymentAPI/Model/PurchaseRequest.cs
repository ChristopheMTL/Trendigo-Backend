using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a Purchase request.
  /// </summary>
  [DataContract]
  public class PurchaseRequest {
    /// <summary>
    /// The transaction total in cents (excluding the taxes and the tips). If not provided, the amount will be taken from the Transaction Token.
    /// </summary>
    /// <value>The transaction total in cents (excluding the taxes and the tips). If not provided, the amount will be taken from the Transaction Token.</value>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public long? Amount { get; set; }

    /// <summary>
    /// The tips that was applied on this transaction in cents.
    /// </summary>
    /// <value>The tips that was applied on this transaction in cents.</value>
    [DataMember(Name="tips", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tips")]
    public long? Tips { get; set; }

    /// <summary>
    /// An indicator specifying if the Member is using his available points for the current transaction.
    /// </summary>
    /// <value>An indicator specifying if the Member is using his available points for the current transaction.</value>
    [DataMember(Name="usePoint", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "usePoint")]
    public bool? UsePoint { get; set; }

    /// <summary>
    /// The member Credit Card unique identifier.
    /// </summary>
    /// <value>The member Credit Card unique identifier.</value>
    [DataMember(Name="creditCardId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creditCardId")]
    public long? CreditCardId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PurchaseRequest {\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  Tips: ").Append(Tips).Append("\n");
      sb.Append("  UsePoint: ").Append(UsePoint).Append("\n");
      sb.Append("  CreditCardId: ").Append(CreditCardId).Append("\n");
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
