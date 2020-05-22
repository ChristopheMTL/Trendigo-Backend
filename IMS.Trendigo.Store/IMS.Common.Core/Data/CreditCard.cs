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
    
    public partial class CreditCard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CreditCard()
        {
            this.TrxFinancialTransactions = new HashSet<TrxFinancialTransaction>();
        }
    
        public long Id { get; set; }
        public long MemberId { get; set; }
        public string TransaxId { get; set; }
        public string CardHolder { get; set; }
        public int CreditCardTypeId { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public bool defaultCard { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Token { get; set; }
    
        public virtual CreditCardType CreditCardType { get; set; }
        public virtual Member Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrxFinancialTransaction> TrxFinancialTransactions { get; set; }
    }
}
