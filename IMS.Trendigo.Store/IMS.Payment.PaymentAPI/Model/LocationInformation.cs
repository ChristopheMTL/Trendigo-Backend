using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Payment.PaymentAPI.Model
{

  /// <summary>
  /// Regroups the location information.
  /// </summary>
  [DataContract]
  public class LocationInformation {
    /// <summary>
    /// This is name of the Location.
    /// </summary>
    /// <value>This is name of the Location.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// This is Location address.
    /// </summary>
    /// <value>This is Location address.</value>
    [DataMember(Name="address", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }

    /// <summary>
    /// This field represent the Location city.
    /// </summary>
    /// <value>This field represent the Location city.</value>
    [DataMember(Name="city", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }

    /// <summary>
    /// This field represent the Location postal code (or zip code).
    /// </summary>
    /// <value>This field represent the Location postal code (or zip code).</value>
    [DataMember(Name="postalCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "postalCode")]
    public string PostalCode { get; set; }

    /// <summary>
    /// This field represent the Location province or state (please refer to https://en.wikipedia.org/wiki/Canadian_postal_abbreviations_for_provinces_and_territories or https://en.wikipedia.org/wiki/List_of_U.S._state_abbreviations).
    /// </summary>
    /// <value>This field represent the Location province or state (please refer to https://en.wikipedia.org/wiki/Canadian_postal_abbreviations_for_provinces_and_territories or https://en.wikipedia.org/wiki/List_of_U.S._state_abbreviations).</value>
    [DataMember(Name="provinceState", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "provinceState")]
    public string ProvinceState { get; set; }

    /// <summary>
    /// This field represent the Location Country (please refer to : https://countrycode.org).
    /// </summary>
    /// <value>This field represent the Location Country (please refer to : https://countrycode.org).</value>
    [DataMember(Name="country", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    /// <summary>
    /// This is the Location phone number.
    /// </summary>
    /// <value>This is the Location phone number.</value>
    [DataMember(Name="phone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "phone")]
    public string Phone { get; set; }

    /// <summary>
    /// This is the Location Timezone.
    /// </summary>
    /// <value>This is the Location Timezone.</value>
    [DataMember(Name="timezone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timezone")]
    public string Timezone { get; set; }

    /// <summary>
    /// The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR.
    /// </summary>
    /// <value>The transaction currency based on ISO 4217 code list. Common values are : CAD, USD, EUR.</value>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class LocationInformation {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Address: ").Append(Address).Append("\n");
      sb.Append("  City: ").Append(City).Append("\n");
      sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
      sb.Append("  ProvinceState: ").Append(ProvinceState).Append("\n");
      sb.Append("  Country: ").Append(Country).Append("\n");
      sb.Append("  Phone: ").Append(Phone).Append("\n");
      sb.Append("  Timezone: ").Append(Timezone).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
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
