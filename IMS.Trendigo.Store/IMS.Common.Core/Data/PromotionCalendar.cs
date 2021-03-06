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
    
    public partial class PromotionCalendar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PromotionCalendar()
        {
            this.LocationPromotionCalendars = new HashSet<LocationPromotionCalendar>();
            this.PromotionCalendarDetails = new HashSet<PromotionCalendarDetail>();
            this.PromotionCalendarNotifications = new HashSet<PromotionCalendarNotification>();
            this.PromotionCalendarTranslations = new HashSet<PromotionCalendarTranslation>();
        }
    
        public long Id { get; set; }
        public int PromotionCalendarTypeId { get; set; }
        public long EnterpriseId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationPromotionCalendar> LocationPromotionCalendars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromotionCalendarDetail> PromotionCalendarDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromotionCalendarNotification> PromotionCalendarNotifications { get; set; }
        public virtual PromotionCalendarType PromotionCalendarType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromotionCalendarTranslation> PromotionCalendarTranslations { get; set; }
    }
}
