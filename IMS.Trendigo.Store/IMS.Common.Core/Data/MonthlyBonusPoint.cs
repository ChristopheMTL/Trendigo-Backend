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
    
    public partial class MonthlyBonusPoint
    {
        public long Id { get; set; }
        public long MemberId { get; set; }
        public long LocationId { get; set; }
        public int Points { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string TransaxId { get; set; }
        public int CommandStatusId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModificationDate { get; set; }
    
        public virtual CommandStatu CommandStatu { get; set; }
        public virtual Member Member { get; set; }
        public virtual Location Location { get; set; }
    }
}
