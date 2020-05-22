using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Non Financial Transaction in the system.
    /// </summary>
    [DataContract]
    public class TransactionNonFinancial
    {
        /// <summary>
        /// The Non Financial Transaction unique identifier.
        /// </summary>
        /// <value>The Non Financial Transaction unique identifier.</value>
        [DataMember(Name = "transactionId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "transactionId")]
        public long? TransactionId { get; set; }

        /// <summary>
        /// The merchant unique transaction identifier. This value must be unique per merchant.
        /// </summary>
        /// <value>The merchant unique transaction identifier. This value must be unique per merchant.</value>
        [DataMember(Name = "orderNumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// The base amount
        /// </summary>
        /// <value>The base amount</value>
        [DataMember(Name = "baseAmount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "baseAmount")]
        public double? BaseAmount { get; set; }

        /// <summary>
        /// The approved amount
        /// </summary>
        /// <value>The approved amount</value>
        [DataMember(Name = "approvedAmount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "approvedAmount")]
        public double? ApprovedAmount { get; set; }

        /// <summary>
        /// The amount
        /// </summary>
        /// <value>The amount</value>
        [DataMember(Name = "amount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// The additional amount
        /// </summary>
        /// <value>The additional amount</value>
        [DataMember(Name = "additionalAmount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "additionalAmount")]
        public double? AdditionalAmount { get; set; }

        /// <summary>
        /// The tip amount
        /// </summary>
        /// <value>The tip amount</value>
        [DataMember(Name = "tip", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "tip")]
        public double? Tip { get; set; }

        /// <summary>
        /// The Trendigo Member unique identifier.
        /// </summary>
        /// <value>The Trendigo Member unique identifier.</value>
        [DataMember(Name = "memberId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "memberId")]
        public int? MemberId { get; set; }

        /// <summary>
        /// The type of transaction : UNKNOWN(1), SALE(2), PREAUTH(3), CONCLUSION(4), SALE MOTO(5), REFUND(6), CASHBACK(7), VOID PREAUTH(8), VOID CONCLUSION(9), VOID SALE(10), VOID REFUND(11), MANUAL REVERSAL(12), ACTIVATION(13), RECHARGE(14), DEACTIVATION(15), REACTIVATION(16), INVOICE INQUIRY(17), BATCH CLOSE(60), BALANCE INQUIRY(19), BATCH INQUIRY(20), REVERSAL(27)
        /// </summary>
        /// <value>The type of transaction : UNKNOWN(1), SALE(2), PREAUTH(3), CONCLUSION(4), SALE MOTO(5), REFUND(6), CASHBACK(7), VOID PREAUTH(8), VOID CONCLUSION(9), VOID SALE(10), VOID REFUND(11), MANUAL REVERSAL(12), ACTIVATION(13), RECHARGE(14), DEACTIVATION(15), REACTIVATION(16), INVOICE INQUIRY(17), BATCH CLOSE(60), BALANCE INQUIRY(19), BATCH INQUIRY(20), REVERSAL(27)</value>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// The number of points that were used for the Non Financial Transaction
        /// </summary>
        /// <value>The number of points that were used for the Non Financial Transaction</value>
        [DataMember(Name = "pointExpended", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pointExpended")]
        public int? PointExpended { get; set; }

        /// <summary>
        /// The number of points that were attributed for the Non Financial Transaction
        /// </summary>
        /// <value>The number of points that were attributed for the Non Financial Transaction</value>
        [DataMember(Name = "pointGained", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pointGained")]
        public int? PointGained { get; set; }

        /// <summary>
        /// The system date time reference
        /// </summary>
        /// <value>The system date time reference</value>
        [DataMember(Name = "systemDateTime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "systemDateTime")]
        public DateTime? SystemDateTime { get; set; }

        /// <summary>
        /// The Terminal local date time reference
        /// </summary>
        /// <value>The Terminal local date time reference</value>
        [DataMember(Name = "terminalDateTime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "terminalDateTime")]
        public DateTime? TerminalDateTime { get; set; }

        /// <summary>
        /// The Terminal unique identifier that was used to perform the Financial Transaction.
        /// </summary>
        /// <value>The Terminal unique identifier that was used to perform the Financial Transaction.</value>
        [DataMember(Name = "terminalId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "terminalId")]
        public int? TerminalId { get; set; }

        /// <summary>
        /// The Program unique identifier.
        /// </summary>
        /// <value>The Program unique identifier.</value>
        [DataMember(Name = "programId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "programId")]
        public int? ProgramId { get; set; }

        /// <summary>
        /// The Location unique identifier.
        /// </summary>
        /// <value>The Location unique identifier.</value>
        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locationId")]
        public int? LocationId { get; set; }

        /// <summary>
        /// The Terminal unique identifier.
        /// </summary>
        /// <value>The Terminal unique identifier.</value>
        [DataMember(Name = "promotionId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "promotionId")]
        public int? PromotionId { get; set; }

        /// <summary>
        /// The Vendor unique identifier (matches the merchant userId).
        /// </summary>
        /// <value>The Vendor unique identifier (matches the merchant userId).</value>
        [DataMember(Name = "vendorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "vendorId")]
        public int? VendorId { get; set; }

        /// <summary>
        /// The Terminal input mode.
        /// </summary>
        /// <value>The Terminal input mode.</value>
        [DataMember(Name = "inputModeId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "inputModeId")]
        public int? InputModeId { get; set; }

        /// <summary>
        /// The type of Financial Transaction that was performed.
        /// </summary>
        /// <value>The type of Financial Transaction that was performed.</value>
        [DataMember(Name = "transactionTypeId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "transactionTypeId")]
        public int? TransactionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets Voided
        /// </summary>
        [DataMember(Name = "voided", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "voided")]
        public bool? Voided { get; set; }

        /// <summary>
        /// Gets or Sets Refunded
        /// </summary>
        [DataMember(Name = "refunded", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "refunded")]
        public bool? Refunded { get; set; }

        /// <summary>
        /// Gets or Sets Concluded
        /// </summary>
        [DataMember(Name = "concluded", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "concluded")]
        public bool? Concluded { get; set; }

        /// <summary>
        /// The Creation Date Time reference
        /// </summary>
        /// <value>The Creation Date Time reference</value>
        [DataMember(Name = "creationDate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "creationDate")]
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// The Enterprise Id.
        /// </summary>
        /// <value>The Enterprise Id.</value>
        [DataMember(Name = "enterpriseId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enterpriseId")]
        public int? EnterpriseId { get; set; }

        /// <summary>
        /// The Non Financial Transaction related unique identifier. Only populated on void and Reversal
        /// </summary>
        /// <value>The Non Financial Transaction related unique identifier. Only populated on void and Reversal</value>
        [DataMember(Name = "relatedTransactionId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "relatedTransactionId")]
        public long? RelatedTransactionId { get; set; }

        /// <summary>
        /// The payment Api Context error message
        /// </summary>
        /// <value>The payment Api Context error message</value>
        [DataMember(Name = "responseMessage", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "responseMessage")]
        public string ResponseMessage { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TransactionNonFinancial {\n");
            sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
            sb.Append("  OrderNumber: ").Append(OrderNumber).Append("\n");
            sb.Append("  BaseAmount: ").Append(BaseAmount).Append("\n");
            sb.Append("  ApprovedAmount: ").Append(ApprovedAmount).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  AdditionalAmount: ").Append(AdditionalAmount).Append("\n");
            sb.Append("  Tip: ").Append(Tip).Append("\n");
            sb.Append("  MemberId: ").Append(MemberId).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  PointExpended: ").Append(PointExpended).Append("\n");
            sb.Append("  PointGained: ").Append(PointGained).Append("\n");
            sb.Append("  SystemDateTime: ").Append(SystemDateTime).Append("\n");
            sb.Append("  TerminalDateTime: ").Append(TerminalDateTime).Append("\n");
            sb.Append("  TerminalId: ").Append(TerminalId).Append("\n");
            sb.Append("  ProgramId: ").Append(ProgramId).Append("\n");
            sb.Append("  LocationId: ").Append(LocationId).Append("\n");
            sb.Append("  PromotionId: ").Append(PromotionId).Append("\n");
            sb.Append("  VendorId: ").Append(VendorId).Append("\n");
            sb.Append("  InputModeId: ").Append(InputModeId).Append("\n");
            sb.Append("  TransactionTypeId: ").Append(TransactionTypeId).Append("\n");
            sb.Append("  Voided: ").Append(Voided).Append("\n");
            sb.Append("  Refunded: ").Append(Refunded).Append("\n");
            sb.Append("  Concluded: ").Append(Concluded).Append("\n");
            sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
            sb.Append("  EnterpriseId: ").Append(EnterpriseId).Append("\n");
            sb.Append("  RelatedTransactionId: ").Append(RelatedTransactionId).Append("\n");
            sb.Append("  ResponseMessage: ").Append(ResponseMessage).Append("\n");
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
