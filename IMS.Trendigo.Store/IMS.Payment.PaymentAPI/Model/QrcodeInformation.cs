using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Describes a QR Code Information.
  /// </summary>
  [DataContract]
  public class QrcodeInformation {
    /// <summary>
    /// The size in pixel of the generated QR Code image.
    /// </summary>
    /// <value>The size in pixel of the generated QR Code image.</value>
    [DataMember(Name="imageSize", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "imageSize")]
    public int? ImageSize { get; set; }

    /// <summary>
    /// The image format of the generated QR Code image. The two supported options are : PNG and JPEG.
    /// </summary>
    /// <value>The image format of the generated QR Code image. The two supported options are : PNG and JPEG.</value>
    [DataMember(Name="imageFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "imageFormat")]
    public string ImageFormat { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class QrcodeInformation {\n");
      sb.Append("  ImageSize: ").Append(ImageSize).Append("\n");
      sb.Append("  ImageFormat: ").Append(ImageFormat).Append("\n");
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
