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
    
    public partial class Promotion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Promotion()
        {
            this.promotion_translations = new HashSet<promotion_translations>();
            this.Locations = new HashSet<Location>();
            this.Promotion_Schedules = new HashSet<Promotion_Schedules>();
            this.Programs = new HashSet<Program>();
        }
    
        public long Id { get; set; }
        public int PromotionTypeId { get; set; }
        public double Value { get; set; }
        public Nullable<long> PackageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public Nullable<int> MaxDiscountForPromotion { get; set; }
        public Nullable<int> MaxAmountForPromotion { get; set; }
        public Nullable<double> MinRebatePercent { get; set; }
        public Nullable<double> MinBonusPercent { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModificationDate { get; set; }
        public Nullable<bool> ComingSoon { get; set; }
    
        public virtual Package Package { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<promotion_translations> promotion_translations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> Locations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Promotion_Schedules> Promotion_Schedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Program> Programs { get; set; }
        public virtual PromotionType PromotionType { get; set; }
    }
}
