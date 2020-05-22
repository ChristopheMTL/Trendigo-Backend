using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum CampaignTypeEnum
    {
        HalfMemberFirstNotice = 100,
        HalfMemberSecondNotice = 101,
        HalfMemberSurveyNotice = 102,
        FullMemberCardWaiting = 200,
        FullMemberCardReceivedNotice = 201,
        FullMemberCardNotActivated = 202,
        FullMemberCardActivated = 203,
        NoTransactionFirstNotice = 300,
        NoTransactionSecondNotice = 301,
        NoTransactionThirdNotice = 302,
        NoTransactionSurveyNotice = 303,
        FirstTransactionNotice = 400,
        SecondTransactionNotice = 401,
        ThirdTransactionNotice = 402,
        NoFollowingTransaction = 403
    }
}
