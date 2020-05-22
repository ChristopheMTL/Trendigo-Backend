using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class AuthenticationToken {
    /// <summary>
    /// The authentication token that is used to make request
    /// </summary>
    /// <value>The authentication token that is used to make request</value>
    [DataMember(Name="authenticationToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "authenticationToken")]
    public string _AuthenticationToken { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AuthenticationToken {\n");
      sb.Append("  _AuthenticationToken: ").Append(_AuthenticationToken).Append("\n");
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
