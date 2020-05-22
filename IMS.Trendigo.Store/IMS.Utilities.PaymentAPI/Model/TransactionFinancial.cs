using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IMS.Utilities.PaymentAPI.Model
{

    /// <summary>
    /// Describes a Financial Transaction in the system.
    /// </summary>
    [DataContract]
    public class TransactionFinancial
    {
        /// <summary>
        /// The Financial Transaction unique identifier.
        /// </summary>
        /// <value>The Financial Transaction unique identifier.</value>
        [DataMember(Name = "transactionId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "transactionId")]
        public long? TransactionId { get; set; }

        /// <summary>
        /// The Processor unique identifier.
        /// </summary>
        /// <value>The Processor unique identifier.</value>
        [DataMember(Name = "processorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "processorId")]
        public int? ProcessorId { get; set; }

        /// <summary>
        /// The Enterprise Id.
        /// </summary>
        /// <value>The Enterprise Id.</value>
        [DataMember(Name = "enterpriseId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enterpriseId")]
        public int? EnterpriseId { get; set; }

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
        /// The Location unique identifier.
        /// </summary>
        /// <value>The Location unique identifier.</value>
        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "locationId")]
        public int? LocationId { get; set; }

        /// <summary>
        /// The vendor unique identifier.
        /// </summary>
        /// <value>The vendor unique identifier.</value>
        [DataMember(Name = "vendorId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "vendorId")]
        public int? VendorId { get; set; }

        /// <summary>
        /// The Terminal reference
        /// </summary>
        /// <value>The Terminal reference</value>
        [DataMember(Name = "terminalReference", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "terminalReference")]
        public string TerminalReference { get; set; }

        /// <summary>
        /// The Terminal local date time reference
        /// </summary>
        /// <value>The Terminal local date time reference</value>
        [DataMember(Name = "localDateTime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "localDateTime")]
        public DateTime? LocalDateTime { get; set; }

        /// <summary>
        /// The acquirer reference
        /// </summary>
        /// <value>The acquirer reference</value>
        [DataMember(Name = "acquirerReference", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "acquirerReference")]
        public string AcquirerReference { get; set; }

        /// <summary>
        /// The Financial Transaction Currency.
        /// </summary>
        /// <value>The Financial Transaction Currency.</value>
        [DataMember(Name = "currencyId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currencyId")]
        public int? CurrencyId { get; set; }

        /// <summary>
        /// The Input Mode (TODO: defined the exact list of input mode)
        /// </summary>
        /// <value>The Input Mode (TODO: defined the exact list of input mode)</value>
        [DataMember(Name = "inputModeId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "inputModeId")]
        public int? InputModeId { get; set; }

        /// <summary>
        /// The type of card that was used to perform the Financial Transaction. Possible values are : UNKNOWN(1), VISA(2), MASTERCARD(3), DISCOVER(4), AMEX(5), INTERAC(6), CHASE_GIFT_CARD(7)
        /// </summary>
        /// <value>The type of card that was used to perform the Financial Transaction. Possible values are : UNKNOWN(1), VISA(2), MASTERCARD(3), DISCOVER(4), AMEX(5), INTERAC(6), CHASE_GIFT_CARD(7)</value>
        [DataMember(Name = "creditCardTypeId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "creditCardTypeId")]
        public int? CreditCardTypeId { get; set; }

        /// <summary>
        /// The type of Financial Transaction that was performed.
        /// </summary>
        /// <value>The type of Financial Transaction that was performed.</value>
        [DataMember(Name = "transactionTypeId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "transactionTypeId")]
        public int? TransactionTypeId { get; set; }

        /// <summary>
        /// The Non Financial Transaction unique identifier.
        /// </summary>
        /// <value>The Non Financial Transaction unique identifier.</value>
        [DataMember(Name = "transactionNonFinancialId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "transactionNonFinancialId")]
        public int? TransactionNonFinancialId { get; set; }

        /// <summary>
        /// The system date time
        /// </summary>
        /// <value>The system date time</value>
        [DataMember(Name = "systemDateTime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "systemDateTime")]
        public DateTime? SystemDateTime { get; set; }

        /// <summary>
        /// The Terminal unique identifier that was used to perform the Financial Transaction.
        /// </summary>
        /// <value>The Terminal unique identifier that was used to perform the Financial Transaction.</value>
        [DataMember(Name = "terminalId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "terminalId")]
        public int? TerminalId { get; set; }

        /// <summary>
        /// The Credit Card that was used to perform the Financial Transaction.
        /// </summary>
        /// <value>The Credit Card that was used to perform the Financial Transaction.</value>
        [DataMember(Name = "creditCardId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "creditCardId")]
        public int? CreditCardId { get; set; }

        /// <summary>
        /// The Acquirer response message.
        /// </summary>
        /// <value>The Acquirer response message.</value>
        [DataMember(Name = "acquirerResponseMessage", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "acquirerResponseMessage")]
        public string AcquirerResponseMessage { get; set; }

        /// <summary>
        /// The Acquirer response code
        /// </summary>
        /// <value>The Acquirer response code</value>
        [DataMember(Name = "acquirerResponseCode", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "acquirerResponseCode")]
        public string AcquirerResponseCode { get; set; }

        /// <summary>
        /// Acquirer merchant ID
        /// </summary>
        /// <value>Acquirer merchant ID</value>
        [DataMember(Name = "acquirerMerchantId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "acquirerMerchantId")]
        public string AcquirerMerchantId { get; set; }

        /// <summary>
        /// Acquirer Terminal ID
        /// </summary>
        /// <value>Acquirer Terminal ID</value>
        [DataMember(Name = "acquirerTerminalId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "acquirerTerminalId")]
        public string AcquirerTerminalId { get; set; }

        /// <summary>
        /// The clerk that was associate with the Financial Transaction.
        /// </summary>
        /// <value>The clerk that was associate with the Financial Transaction.</value>
        [DataMember(Name = "clerk", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "clerk")]
        public string Clerk { get; set; }

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
        /// The Non Financial Transaction related unique identifier. Only populated on void and Reversal
        /// </summary>
        /// <value>The Non Financial Transaction related unique identifier. Only populated on void and Reversal</value>
        [DataMember(Name = "relatedTransactionId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "relatedTransactionId")]
        public long? RelatedTransactionId { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TransactionFinancial {\n");
            sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
            sb.Append("  ProcessorId: ").Append(ProcessorId).Append("\n");
            sb.Append("  EnterpriseId: ").Append(EnterpriseId).Append("\n");
            sb.Append("  BaseAmount: ").Append(BaseAmount).Append("\n");
            sb.Append("  ApprovedAmount: ").Append(ApprovedAmount).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  AdditionalAmount: ").Append(AdditionalAmount).Append("\n");
            sb.Append("  Tip: ").Append(Tip).Append("\n");
            sb.Append("  LocationId: ").Append(LocationId).Append("\n");
            sb.Append("  VendorId: ").Append(VendorId).Append("\n");
            sb.Append("  TerminalReference: ").Append(TerminalReference).Append("\n");
            sb.Append("  LocalDateTime: ").Append(LocalDateTime).Append("\n");
            sb.Append("  AcquirerReference: ").Append(AcquirerReference).Append("\n");
            sb.Append("  CurrencyId: ").Append(CurrencyId).Append("\n");
            sb.Append("  InputModeId: ").Append(InputModeId).Append("\n");
            sb.Append("  CreditCardTypeId: ").Append(CreditCardTypeId).Append("\n");
            sb.Append("  TransactionTypeId: ").Append(TransactionTypeId).Append("\n");
            sb.Append("  TransactionNonFinancialId: ").Append(TransactionNonFinancialId).Append("\n");
            sb.Append("  SystemDateTime: ").Append(SystemDateTime).Append("\n");
            sb.Append("  TerminalId: ").Append(TerminalId).Append("\n");
            sb.Append("  CreditCardId: ").Append(CreditCardId).Append("\n");
            sb.Append("  AcquirerResponseMessage: ").Append(AcquirerResponseMessage).Append("\n");
            sb.Append("  AcquirerResponseCode: ").Append(AcquirerResponseCode).Append("\n");
            sb.Append("  AcquirerMerchantId: ").Append(AcquirerMerchantId).Append("\n");
            sb.Append("  AcquirerTerminalId: ").Append(AcquirerTerminalId).Append("\n");
            sb.Append("  Clerk: ").Append(Clerk).Append("\n");
            sb.Append("  Voided: ").Append(Voided).Append("\n");
            sb.Append("  Refunded: ").Append(Refunded).Append("\n");
            sb.Append("  Concluded: ").Append(Concluded).Append("\n");
            sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
            sb.Append("  RelatedTransactionId: ").Append(RelatedTransactionId).Append("\n");
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
