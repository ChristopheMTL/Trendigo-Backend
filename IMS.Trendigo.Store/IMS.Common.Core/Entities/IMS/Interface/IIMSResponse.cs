using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Entities.IMS.Interface
{
    interface IIMSResponse
    {
        string IMSCode
        {
            get;
            set;
        }

        string IMSMessage
        {
            get;
            set;
        }
    }
}
