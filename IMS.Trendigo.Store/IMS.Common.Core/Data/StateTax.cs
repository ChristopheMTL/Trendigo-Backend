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
    
    public partial class StateTax
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StateTax()
        {
            this.LocationTaxes = new HashSet<LocationTax>();
            this.EnterpriseStateTaxNumbers = new HashSet<EnterpriseStateTaxNumber>();
        }
    
        public int Id { get; set; }
        public int StateId { get; set; }
        public int CurrencyId { get; set; }
        public string TransaxId { get; set; }
        public string Code { get; set; }
        public double Value { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModificationDate { get; set; }
    
        public virtual Currency Currency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationTax> LocationTaxes { get; set; }
        public virtual State State { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnterpriseStateTaxNumber> EnterpriseStateTaxNumbers { get; set; }
    }
}
