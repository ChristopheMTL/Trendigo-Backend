using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Store.Common.Extensions;
using System.Net.Http;
using System.Configuration;
using IMS.Common.Core.Utilities;
using Geocoding.Google;

namespace IMS.Common.Core.Services
{
    public class GeolocationManager
    {
        JSONHelper JHelp = new JSONHelper();

        private async Task<T> GetCities<T>(Dictionary<string, string> parameters = null, Boolean HandleMultipleValueKey = false)
        {
            return await JHelp.CallRestJsonService<T>(ConfigurationManager.AppSettings["IMSAPIAddress"] + "api/v2/cities", parameters, false, HttpMethod.Post);
        }

        public String GetTimeZoneName(string longitude, string latitude)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("location", latitude + "," + longitude);
            parameters.Add("timestamp", new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString());
            parameters.Add("key", ConfigurationManager.AppSettings["GoogleAPIS.TimeZoneAPI.Key"]);

            var tzmRS = GetTimeZoneName(parameters);

            if (tzmRS == null) 
            {
                return "Eastern Standard Time";
            }

            return tzmRS.timeZoneName;
        }

        private TimeZoneModel GetTimeZoneName(Dictionary<string, string> parameters)
        {
            //maps.googleapis.com/maps/api/timezone/json?location=38.908133,-77.047119&timestamp=1458000000&key=[my key]
            //key = AIzaSyAtUBhOxveV60R_00xnULvhyNH2V8LKOHQ

            return JHelp.GetTimeZoneName(ConfigurationManager.AppSettings["GoogleAPIS.TimeZoneAPI"], parameters);
        }

        public Geocoding.Address GetGeoLocation(String streetAddress, String City, String State, String ZipCode)
        {
            Geocoding.Address geolocation;

            geolocation = new UtilityManager().getCoordinateWithAddress(String.Concat(streetAddress, " ", City, " ", State, " ", ZipCode));

            if (geolocation == null || (geolocation.Coordinates.Longitude == 0 || geolocation.Coordinates.Latitude == 0))
            {
                geolocation = new UtilityManager().getCoordinateWithAddress(String.Concat(streetAddress, " ", City, " ", ZipCode));

                if (geolocation == null || (geolocation.Coordinates.Longitude == 0 || geolocation.Coordinates.Latitude == 0))
                {
                    geolocation = new UtilityManager().getCoordinateWithAddress(String.Concat(streetAddress, " ", ZipCode));
                }
            }

            return geolocation;
        }

        public IEnumerable<GoogleAddress> GetAddresses(String streetAddress, String City, String State, String ZipCode)
        {
            IEnumerable<GoogleAddress> addresses;

            addresses = new UtilityManager().getAddresses(String.Concat(streetAddress, " ", City, " ", State, " ", ZipCode));

            if (addresses == null || (addresses.FirstOrDefault().Coordinates.Longitude == 0 || addresses.FirstOrDefault().Coordinates.Latitude == 0))
            {
                addresses = new UtilityManager().getAddresses(String.Concat(streetAddress, " ", City, " ", ZipCode));

                if (addresses == null || (addresses.FirstOrDefault().Coordinates.Longitude == 0 || addresses.FirstOrDefault().Coordinates.Latitude == 0))
                {
                    addresses = new UtilityManager().getAddresses(String.Concat(streetAddress, " ", ZipCode));
                }
            }

            return addresses;
        }
    }

    public class TimeZoneModel
    {
        public int dstOffset { get; set; }
        public int rawOffset { get; set; }
        public string status { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
    }
}
