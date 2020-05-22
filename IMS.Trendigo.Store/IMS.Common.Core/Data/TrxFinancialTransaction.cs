//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMS.Common.Core.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TrxFinancialTransaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TrxFinancialTransaction()
        {
            this.IncentiveProgramRefunds = new HashSet<IncentiveProgramRefund>();
        }
    
        public long Id { get; set; }
        public int acquirerId { get; set; }
        public Nullable<decimal> baseAmount { get; set; }
        public Nullable<decimal> approvedAmount { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<decimal> aditionalAmount { get; set; }
        public Nullable<decimal> tip { get; set; }
        public string entityId { get; set; }
        public string vendorId { get; set; }
        public string terminalReference { get; set; }
        public Nullable<System.DateTime> localDateTime { get; set; }
        public string acquirerReference { get; set; }
        public Nullable<int> currencyId { get; set; }
        public Nullable<int> inputModeId { get; set; }
        public Nullable<int> cardTypeId { get; set; }
        public Nullable<int> transactionTypeId { get; set; }
        public Nullable<long> legTransaction { get; set; }
        public Nullable<System.DateTime> systemDateTime { get; set; }
        public string transaxTerminalId { get; set; }
        public string merchantResponseMessage { get; set; }
        public string acquirerResponseCode { get; set; }
        public string description { get; set; }
        public Nullable<int> voided { get; set; }
        public Nullable<int> refunded { get; set; }
        public Nullable<int> concluded { get; set; }
        public Nullable<int> IMSTransferStatusId { get; set; }
        public Nullable<int> IMSTransferMessageId { get; set; }
        public Nullable<System.DateTime> IMSTransferDate { get; set; }
        public long EnterpriseId { get; set; }
        public string TerminalId { get; set; }
        public string idRelatedTransaction { get; set; }
        public Nullable<long> creditCardId { get; set; }
    
        public virtual Acquirer Acquirer { get; set; }
        public virtual GlobalResponseCode GlobalResponseCode { get; set; }
        public virtual IMSTransferMessage IMSTransferMessage { get; set; }
        public virtual IMSTransferStatu IMSTransferStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncentiveProgramRefund> IncentiveProgramRefunds { get; set; }
        public virtual CreditCard CreditCard { get; set; }
    }
}
