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
    
    public partial class Subscriber
    {
        public System.Guid Id { get; set; }
        public long EnterpriseId { get; set; }
        public string Email { get; set; }
        public Nullable<long> PromocodeId { get; set; }
        public bool Unsubscribe { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModificationDate { get; set; }
    
        public virtual Enterprise Enterprise { get; set; }
        public virtual PromoCode PromoCode { get; set; }
    }
}
