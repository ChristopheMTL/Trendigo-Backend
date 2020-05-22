using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum IMSCodeMessage
    {
        
        ErrorUnhandled = 100,

        MissingParameter = 125,

        TokenNotProvided = 150,

        TransaxUserNotAdmin = 151,

        Success = 200,

        

        //300-399 Membership
        InvalidUsernameOrPassword = 300,
        DuplicateEmailAddress = 301,
        DuplicateProviderUserKey = 302,
        DuplicateUserName = 303,
        InvalidAnswer = 304,
        InvalidEmail = 305,
        InvalidPassword = 306,
        InvalidProviderUserKey = 307,
        InvalidQuestion = 308,
        InvalidUserName = 309,
        ProviderError = 310,
        UserRejected = 311,

        //400-499 Enterprise
        EnterpriseTransaxNotAdded = 400,
        EnterpriseTransaxNotUpdated = 401,

        //500-600 Sponsor

        //800-900 Promotion
        UploadImageFail = 898,
        PromotionDeleteFail = 899

        //900-999
        
    }
}
