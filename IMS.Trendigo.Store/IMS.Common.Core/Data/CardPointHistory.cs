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
    
    public partial class CardPointHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CardPointHistory()
        {
            this.MemberPromoCodeHistories = new HashSet<MemberPromoCodeHistory>();
        }
    
        public long Id { get; set; }
        public long IMSCardId { get; set; }
        public int Points { get; set; }
        public string Reason { get; set; }
        public string TransaxId { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> PromoCodeId { get; set; }
    
        public virtual IMSCard IMSCard { get; set; }
        public virtual PromoCode PromoCode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberPromoCodeHistory> MemberPromoCodeHistories { get; set; }
        public virtual IMSUser IMSUser { get; set; }
    }
}
