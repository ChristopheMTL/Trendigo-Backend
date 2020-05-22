using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Credit Card in the system.
    /// </summary>
    [DataContract]
    public class Creditcard
    {
        /// <summary>
        /// The is the unique Credit Card identifier
        /// </summary>
        /// <value>The is the unique Credit Card identifier</value>
        [DataMember(Name = "creditCardId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "creditCardId")]
        public int? CreditCardId { get; set; }

        /// <summary>
        /// The unique Member identifier who own this card
        /// </summary>
        /// <value>The unique Member identifier who own this card</value>
        [DataMember(Name = "memberId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "memberId")]
        public int? MemberId { get; set; }

        /// <summary>
        /// The name that appears on the Credit Card..
        /// </summary>
        /// <value>The name that appears on the Credit Card..</value>
        [DataMember(Name = "nameOnCard", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "nameOnCard")]
        public string NameOnCard { get; set; }

        /// <summary>
        /// The token that represent the Credit Card
        /// </summary>
        /// <value>The token that represent the Credit Card</value>
        [DataMember(Name = "token", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        /// <summary>
        /// The last 4 digits of the Credit Card PAN.
        /// </summary>
        /// <value>The last 4 digits of the Credit Card PAN.</value>
        [DataMember(Name = "panMask", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "panMask")]
        public string PanMask { get; set; }

        /// <summary>
        /// The expiration date of the Credit Card (in YYMM format)
        /// </summary>
        /// <value>The expiration date of the Credit Card (in YYMM format)</value>
        [DataMember(Name = "expirationDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "expirationDate")]
        public string ExpirationDate { get; set; }

        /// <summary>
        /// The type of card that was used to perform the Financial Transaction. Possible values are : UNKNOWN(1), VISA(2), MASTERCARD(3), DISCOVER(4), AMEX(5), INTERAC(6), CHASE_GIFT_CARD(7)
        /// </summary>
        /// <value>The type of card that was used to perform the Financial Transaction. Possible values are : UNKNOWN(1), VISA(2), MASTERCARD(3), DISCOVER(4), AMEX(5), INTERAC(6), CHASE_GIFT_CARD(7)</value>
        [DataMember(Name = "cardTypeId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "cardTypeId")]
        public int? CardTypeId { get; set; }

        /// <summary>
        /// The Creation Date Time reference
        /// </summary>
        /// <value>The Creation Date Time reference</value>
        [DataMember(Name = "creationDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "creationDate")]
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Credit Card.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Credit Card.</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Creditcard {\n");
            sb.Append("  CreditCardId: ").Append(CreditCardId).Append("\n");
            sb.Append("  MemberId: ").Append(MemberId).Append("\n");
            sb.Append("  NameOnCard: ").Append(NameOnCard).Append("\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
            sb.Append("  PanMask: ").Append(PanMask).Append("\n");
            sb.Append("  ExpirationDate: ").Append(ExpirationDate).Append("\n");
            sb.Append("  CardTypeId: ").Append(CardTypeId).Append("\n");
            sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
