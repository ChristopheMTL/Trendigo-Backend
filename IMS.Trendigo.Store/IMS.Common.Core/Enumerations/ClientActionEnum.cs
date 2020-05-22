using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum ClientAction
    {
        creation = 0,
        cancellation = 1,
        suspension = 2,
        deletion = 3,
        update = 4,
        associateNewCart = 5,
        removeCard = 6,
        increasePoint = 7,
        decreasePoint = 8,
        transfertPoint = 9
    }
}
