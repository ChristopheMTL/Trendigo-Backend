using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum TransaxCodeMessage
    {
        Ok = 200,
        SponsorNotExist = 304,
        SponsorIdIsEmpty = 400,
        OutOfScopePermission = 404,
        SponsorPertainProgram = 501
    }
}
