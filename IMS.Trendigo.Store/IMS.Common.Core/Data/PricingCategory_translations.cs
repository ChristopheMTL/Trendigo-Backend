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
    
    public partial class PricingCategory_translations
    {
        public int Id { get; set; }
        public int PricingCategoryId { get; set; }
        public int LanguageId { get; set; }
        public string Description { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual PricingCategory PricingCategory { get; set; }
    }
}
