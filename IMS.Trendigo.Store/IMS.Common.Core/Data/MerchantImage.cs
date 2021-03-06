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
    
    public partial class MerchantImage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MerchantImage()
        {
            this.Merchants = new HashSet<Merchant>();
            this.Merchants1 = new HashSet<Merchant>();
        }
    
        public System.Guid Id { get; set; }
        public long MerchantId { get; set; }
        public string Filepath { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsPromoted { get; set; }
        public short Weight { get; set; }
        public Nullable<bool> IsPublished { get; set; }
        public string ImagePath { get; set; }
    
        public virtual Merchant Merchant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Merchant> Merchants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Merchant> Merchants1 { get; set; }
    }
}
