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
    
    public partial class merchant_translations
    {
        public int id { get; set; }
        public long merchant_id { get; set; }
        public string locale { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string About { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    
        public virtual Merchant Merchant { get; set; }
    }
}