using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class FinancialTransactionDTO
    {
        public long Id { get; set; }
        public Nullable<System.DateTime> localDateTime { get; set; }
        public string maskedPAN { get; set; }
        public string currency { get; set; }
        public Nullable<int> transactionTypeId { get; set; }
        public Nullable<decimal> amount { get; set; }
        public int pointExpended { get; set; }
        public int pointGained { get; set; }
    }
}
