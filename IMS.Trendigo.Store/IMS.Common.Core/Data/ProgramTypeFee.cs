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
    
    public partial class ProgramTypeFee
    {
        public int Id { get; set; }
        public int ProgramTypeId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModificationDate { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual ProgramType ProgramType { get; set; }
    }
}
