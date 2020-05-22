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
    
    public partial class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Program()
        {
            this.IMSMemberships = new HashSet<IMSMembership>();
            this.program_translations = new HashSet<program_translations>();
            this.ProgramFeatures = new HashSet<ProgramFeature>();
            this.ProgramFees = new HashSet<ProgramFee>();
            this.PromoCodes = new HashSet<PromoCode>();
            this.Enterprises = new HashSet<Enterprise>();
            this.OutsideChannels = new HashSet<OutsideChannel>();
            this.CardTypes = new HashSet<CardType>();
            this.IMSCards = new HashSet<IMSCard>();
            this.Promotions = new HashSet<Promotion>();
            this.Merchants = new HashSet<Merchant>();
        }
    
        public long Id { get; set; }
        public long EnterpriseId { get; set; }
        public string TransaxId { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public decimal FidelityRewardPercent { get; set; }
        public int LoyaltyCostUsingPoints { get; set; }
        public int LoyaltyValueGainingPoints { get; set; }
        public bool WithoutPromoAllowed { get; set; }
        public int ExpirationInMonth { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<int> CardTypeId { get; set; }
        public Nullable<int> ProgramTypeId { get; set; }
    
        public virtual CardType CardType { get; set; }
        public virtual Enterprise Enterprise { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IMSMembership> IMSMemberships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<program_translations> program_translations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgramFeature> ProgramFeatures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgramFee> ProgramFees { get; set; }
        public virtual ProgramType ProgramType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromoCode> PromoCodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enterprise> Enterprises { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutsideChannel> OutsideChannels { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CardType> CardTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IMSCard> IMSCards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Promotion> Promotions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Merchant> Merchants { get; set; }
    }
}
