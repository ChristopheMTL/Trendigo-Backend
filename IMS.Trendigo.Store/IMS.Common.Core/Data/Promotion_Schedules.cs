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
    
    public partial class Promotion_Schedules
    {
        public long Id { get; set; }
        public long PromotionId { get; set; }
        public string TransaxId { get; set; }
        public double Value { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public Nullable<bool> IsPrime { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual IMSUser IMSUser { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
