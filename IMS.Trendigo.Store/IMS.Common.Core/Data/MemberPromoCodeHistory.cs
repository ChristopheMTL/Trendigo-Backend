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
    
    public partial class MemberPromoCodeHistory
    {
        public long Id { get; set; }
        public long MemberId { get; set; }
        public long PromocodeId { get; set; }
        public long CardId { get; set; }
        public long CardPointHistoryId { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual CardPointHistory CardPointHistory { get; set; }
        public virtual IMSCard IMSCard { get; set; }
        public virtual PromoCode PromoCode { get; set; }
        public virtual Member Member { get; set; }
    }
}
