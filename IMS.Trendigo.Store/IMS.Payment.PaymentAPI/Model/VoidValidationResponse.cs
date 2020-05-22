using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a Void Validation response.
  /// </summary>
  [DataContract]
  public class VoidValidationResponse {
    /// <summary>
    /// The JWT Transaction Token that requires to be passed in the future void transactions. All the elements that have been submitted in the validation request will be included in this Transaction Token.
    /// </summary>
    /// <value>The JWT Transaction Token that requires to be passed in the future void transactions. All the elements that have been submitted in the validation request will be included in this Transaction Token.</value>
    [DataMember(Name="transactionToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionToken")]
    public string TransactionToken { get; set; }

    /// <summary>
    /// A description of the validation result in the language passed in the request.
    /// </summary>
    /// <value>A description of the validation result in the language passed in the request.</value>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VoidValidationResponse {\n");
      sb.Append("  TransactionToken: ").Append(TransactionToken).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
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
