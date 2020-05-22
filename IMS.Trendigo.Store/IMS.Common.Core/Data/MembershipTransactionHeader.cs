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
    
    public partial class MembershipTransactionHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MembershipTransactionHeader()
        {
            this.MembershipTransactionDetails = new HashSet<MembershipTransactionDetail>();
        }
    
        public long Id { get; set; }
        public long EnterpriseId { get; set; }
        public Nullable<long> OutsideChannelId { get; set; }
        public int CurrencyId { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime CurrentDate { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Enterprise Enterprise { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MembershipTransactionDetail> MembershipTransactionDetails { get; set; }
        public virtual OutsideChannel OutsideChannel { get; set; }
    }
}
