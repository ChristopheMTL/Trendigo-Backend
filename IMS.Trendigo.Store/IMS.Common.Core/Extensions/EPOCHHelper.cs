using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Store.Common.Extensions
{
    public class EPOCHHelper
    {
        public double ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0));

            //return the total seconds (which is a UNIX timestamp)
            return Math.Round((double)span.TotalSeconds, 0);
        }

        public DateTime ConvertToDateTime(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }
    }
}
