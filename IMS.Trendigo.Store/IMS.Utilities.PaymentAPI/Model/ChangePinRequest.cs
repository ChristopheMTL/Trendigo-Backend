using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

  /// <summary>
  /// The request that allows a Member to change its PIN in the system
  /// </summary>
  [DataContract]
  public class ChangePinRequest {
    /// <summary>
    /// The new pin to be applied to the User.
    /// </summary>
    /// <value>The new pin to be applied to the User.</value>
    [DataMember(Name="newPin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newPin")]
    public string NewPin { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ChangePinRequest {\n");
      sb.Append("  NewPin: ").Append(NewPin).Append("\n");
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
