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
    
    public partial class IMS_Detail
    {
        public long Id { get; set; }
        public long HeaderId { get; set; }
        public int LineItemId { get; set; }
        public long TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual IMS_LineItem IMS_LineItem { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public virtual IMS_Header IMS_Header { get; set; }
    }
}
