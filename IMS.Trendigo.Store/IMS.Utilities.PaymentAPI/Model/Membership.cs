using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Membership in the system.
    /// </summary>
    [DataContract]
    public class Membership
    {
        /// <summary>
        /// The is the unique Membership identifier
        /// </summary>
        /// <value>The is the unique Membership identifier</value>
        [DataMember(Name = "membershipId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "membershipId")]
        public int? MembershipId { get; set; }

        /// <summary>
        /// The is the unique Member identifier
        /// </summary>
        /// <value>The is the unique Member identifier</value>
        [DataMember(Name = "memberId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "memberId")]
        public int? MemberId { get; set; }

        /// <summary>
        /// The is the unique Program identifier
        /// </summary>
        /// <value>The is the unique Program identifier</value>
        [DataMember(Name = "programId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "programId")]
        public int? ProgramId { get; set; }

        /// <summary>
        /// The number of point that the member currently have for this Membership
        /// </summary>
        /// <value>The number of point that the member currently have for this Membership</value>
        [DataMember(Name = "pointBalance", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pointBalance")]
        public int? PointBalance { get; set; }

        /// <summary>
        /// This field is an enumeration that represents the status of the Membership.
        /// </summary>
        /// <value>This field is an enumeration that represents the status of the Membership.</value>
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
            sb.Append("class Membership {\n");
            sb.Append("  MembershipId: ").Append(MembershipId).Append("\n");
            sb.Append("  MemberId: ").Append(MemberId).Append("\n");
            sb.Append("  ProgramId: ").Append(ProgramId).Append("\n");
            sb.Append("  PointBalance: ").Append(PointBalance).Append("\n");
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
