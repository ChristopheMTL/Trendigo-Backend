using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Trendigo Card in the system.
    /// </summary>
    [DataContract]
    public class Card
    {
        /// <summary>
        /// The is the unique Trendigo Card identifier
        /// </summary>
        /// <value>The is the unique Trendigo Card identifier</value>
        [DataMember(Name = "cardId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "cardId")]
        public int? CardId { get; set; }

        /// <summary>
        /// The unique Member identifier who own this card
        /// </summary>
        /// <value>The unique Member identifier who own this card</value>
        [DataMember(Name = "memberId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "memberId")]
        public int? MemberId { get; set; }

        /// <summary>
        /// The unique Program identifier associate with this card
        /// </summary>
        /// <value>The unique Program identifier associate with this card</value>
        [DataMember(Name = "programId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "programId")]
        public int? ProgramId { get; set; }

        /// <summary>
        /// The Trendigo Card number.
        /// </summary>
        /// <value>The Trendigo Card number.</value>
        [DataMember(Name = "cardNumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "cardNumber")]
        public string CardNumber { get; set; }

        /// <summary>
        /// The type of Trendigo Card
        /// </summary>
        /// <value>The type of Trendigo Card</value>
        [DataMember(Name = "cardType", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "cardType")]
        public string CardType { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Trendigo Card.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Trendigo Card.</value>
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
            sb.Append("class Card {\n");
            sb.Append("  CardId: ").Append(CardId).Append("\n");
            sb.Append("  MemberId: ").Append(MemberId).Append("\n");
            sb.Append("  ProgramId: ").Append(ProgramId).Append("\n");
            sb.Append("  CardNumber: ").Append(CardNumber).Append("\n");
            sb.Append("  CardType: ").Append(CardType).Append("\n");
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
