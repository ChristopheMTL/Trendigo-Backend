using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum EnrollmentProcessEnum
    {
        EnrollMembership = 1,
        UpdateEnrollMembership = 2,
        TransferPoint = 3,
        DeleteEnrollMembership = 4,
        DeactivateOldNonFinancialCard = 5,
        AddCreditCardNewMembership = 6,
        DeleteCreditCardOldMembership = 7,
        DisableEnrollMembership = 8,
        ApplyPromoCode = 9,
        MonthlyBonusPoint = 10,
        ApplyPrefixPoints = 11
    }
}
