using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    /// <summary>
    /// Apply this way
    /// For Date => filterType:Timestamp,Timestamp
    /// For Hour => filterType:13:30,19:50
    /// For WeekDays => filterType:1,3,5,7
    /// </summary>
    public enum TransaxPromotionFilterType
    {
        DateStartAndFinish = 1,
        HourStartAndEnd = 2,
        WeekDays = 3
    }
}
