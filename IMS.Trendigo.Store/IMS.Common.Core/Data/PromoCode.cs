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
    
    public partial class PromoCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PromoCode()
        {
            this.CardPointHistories = new HashSet<CardPointHistory>();
            this.IMSMemberships = new HashSet<IMSMembership>();
            this.MemberPromoCodeHistories = new HashSet<MemberPromoCodeHistory>();
            this.Subscribers = new HashSet<Subscriber>();
            this.GiftCardPurchases = new HashSet<GiftCardPurchas>();
            this.LocationPromoCodes = new HashSet<LocationPromoCode>();
        }
    
        public long Id { get; set; }
        public Nullable<long> OutsideChannelId { get; set; }
        public long ProgramId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int PrefixPoints { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int MaxLimit { get; set; }
        public int AlreadyUsed { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool NewMemberOnly { get; set; }
        public Nullable<bool> FromGiftCard { get; set; }
        public Nullable<bool> ForNewsletter { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CardPointHistory> CardPointHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IMSMembership> IMSMemberships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberPromoCodeHistory> MemberPromoCodeHistories { get; set; }
        public virtual OutsideChannel OutsideChannel { get; set; }
        public virtual Program Program { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subscriber> Subscribers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiftCardPurchas> GiftCardPurchases { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationPromoCode> LocationPromoCodes { get; set; }
    }
}
