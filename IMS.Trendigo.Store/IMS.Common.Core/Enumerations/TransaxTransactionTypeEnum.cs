using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum TransaxTransactionType
    {
        Unknown = 1,
        Sale = 2,
        Preauth = 3,
        Conclusion = 4,
        SaleMOTO = 5,
        Refund = 6,
        Cashback = 7,
        VoidPreauth = 8,
        VoidConclusion = 9,
        VoidSale = 10,
        VoidRefund = 11,
        ManualReversal = 12,
        Activation = 13,
        Recharge = 14,
        Deactivation = 15,
        Reactivation = 16,
        InvoiceInquiry = 17,
        BatchClose = 60, //BatchClose = 18,
        BalanceInquiry = 19,
        BatchInquiry = 20,
        Reversal = 27
    }
}
