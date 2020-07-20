using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geocoding.Google;
using Geocoding;
using Geocoding.Microsoft.Json;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Data.SqlTypes;
using IMS.Common.Core.Data;
using IMS.Common.Core.Services;
using IMS.Common.Core.Enumerations;
using System.Globalization;

namespace IMS.Common.Core.Utilities
{
    public class UtilityManager
    {
        IMSEntities context = new IMSEntities();

        //public String FormatUsernameWithEmail(String email, Int64 enterpriseID)
        //{
        //    return email + "." + enterpriseID.ToString();
        //}

        #region Conversion Section

        public static double ConvertMilesToKilometers(double miles)
        {
            //
            // Multiply by this constant and return the result.
            //
            return miles * 1.609344;
        }

        public static double ConvertKilometersToMiles(double kilometers)
        {
            //
            // Multiply by this constant.
            //
            return kilometers * 0.621371192;
        }

        public static double ConvertDecimalToDouble(decimal value)
        {
            return Convert.ToDouble(value);
        }

        #endregion

        #region GeoLocation Section

        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name="pos1">Location 1</param>
        /// <param name="pos2">Location 2</param>
        /// <param name="unit">Miles or Kilometers</param>
        /// <returns>Distance in the requested unit</returns>
        public double HaversineDistance(double pos1_longitude, double pos1_latitude, decimal pos2_longitude, decimal pos2_latitude, DistanceUnit unit)
        {
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = (Math.PI / 180) * ((double)pos2_latitude - pos1_latitude);
            var lng = (Math.PI / 180) * ((double)pos2_longitude - pos1_longitude);
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos((Math.PI / 180) * pos1_latitude) * Math.Cos((Math.PI / 180) * (double)pos2_latitude) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }

        #endregion

        public Geocoding.Address getCoordinateWithAddress(String fullAddress)
        {
            String GeoApiKey = "AIzaSyDVi-distTEh6bjEM1yYVyt8oKaDRnt77M"; //"AIzaSyAMKtf -vyo8MQNv3BLetgYKPUb92e52Xpc"; //"AIzaSyCR9l-L2mj1vSHF55-SKRiNOQZzJKhmpFo"; //ConfigurationManager.AppSettings["GeoCodingApiKey"];

            try
            {
                IGeocoder geocoder = new GoogleGeocoder() { ApiKey = GeoApiKey };

                IEnumerable<Geocoding.Address> addresses = geocoder.Geocode(fullAddress);
                return addresses.FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<Geocoding.Google.GoogleAddress> getAddresses(String fullAddress)
        {
            String GeoApiKey = "AIzaSyDVi-distTEh6bjEM1yYVyt8oKaDRnt77M"; //"AIzaSyAMKtf-vyo8MQNv3BLetgYKPUb92e52Xpc"; //"AIzaSyCR9l-L2mj1vSHF55-SKRiNOQZzJKhmpFo"; //ConfigurationManager.AppSettings["GeoCodingApiKey"];
            GoogleGeocoder geocoder = new GoogleGeocoder() { ApiKey = GeoApiKey };
            IEnumerable<GoogleAddress> addresses = geocoder.Geocode(fullAddress);
            return addresses;
        }

        public string FormatMultilingualForTransax(Dictionary<string, string> listOfText)
        {
            string response = "";

            if (listOfText.Count > 0) 
            {
                foreach (KeyValuePair<string, string> kvp in listOfText)
                {
                    response = response + FormatMultilingual(kvp.Key, kvp.Value);
                }

                return response.Substring(0, (response.Length) - 4);
            }

            return response;
        }

        private string FormatMultilingual(string language, string valueToFormat)
        {
            return language + "#:#" + valueToFormat + "#;# ";
        }

        public List<Language> getLanguage() 
        {
            return context.Languages.ToList();
        }

        public int getLanguageId(string language)
        {
            string languageToReturn = "en";

            Language lang = context.Languages.Where(a => a.ISO639_1 == language).FirstOrDefault();

            if (lang != null)
                languageToReturn = lang.ISO639_1.ToLower();

            return context.Languages.Where(a => a.ISO639_1.ToLower() == languageToReturn).Select(b => b.Id).FirstOrDefault();
        }

        public int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        public string GetMonthName(DateTime d, CultureInfo ci)
        {
            return ci.DateTimeFormat.GetMonthName(d.Month);
        }

        public string MaskCardNumber(string cardNumber, bool withSpaces = false)
        {
            if (cardNumber.Length < 8)
                return cardNumber;
                
            var firstDigits = cardNumber.Substring(0, 4);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('*', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);

            return withSpaces ? Regex.Replace(maskedString, ".{4}", "$0 ") : maskedString;
        }

        [Obsolete("This method was created when the LocalDateTime was in format yyMMdd hhmmss. Now the LocalDateTime is a DateTime field")]
        public DateTime ConvertTransactionLocalDateTimeToDateTime(String localDateTime) 
        {
            if (String.IsNullOrEmpty(localDateTime)) 
            {
                throw new NotImplementedException();
            }

            localDateTime = localDateTime.Replace(" ", "");
            if (localDateTime.Length < 12)
            {
                throw new NotImplementedException();
            }

            if (!CheckDate("20" + localDateTime.Substring(0, 2) + "/" + localDateTime.Substring(2, 2) + "/" + localDateTime.Substring(4, 2) + " " + localDateTime.Substring(6, 2) + ":" + localDateTime.Substring(8, 2) + ":" + localDateTime.Substring(10, 2)))
            {
                throw new NotImplementedException();
            }
            return Convert.ToDateTime("20" + localDateTime.Substring(0, 2) + "/" + localDateTime.Substring(2, 2) + "/" + localDateTime.Substring(4, 2) + " " + localDateTime.Substring(6, 2) + ":" + localDateTime.Substring(8, 2) + ":" + localDateTime.Substring(10, 2));
        }

        protected bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public int GetDayOfWeek(string day)
        {
            switch (day)
            {
                case "Sunday":
                    return 0;
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                default:
                    throw new Exception("Value not found");
            }
        }

        public TimeZoneInfo getTimeZoneInfoForEntity(Data.Location location, IMSEntities db)
        {
            if (location == null) 
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }

            if (location.Address == null) 
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }

            if (location.Address.Latitude == 0 || location.Address.Longitude == 0)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }

            try 
            {
                var tzm = new GeolocationManager().GetTimeZoneName(location.Address.Longitude.ToString(), location.Address.Latitude.ToString());

                if (tzm == null)
                {
                    return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                }

                IMS.Common.Core.Data.TimeZone tz = db.TimeZones.Where(a => a.Value == tzm).FirstOrDefault();

                if (tz == null) 
                {
                    return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                }

                if (tz.Value == "Eastern Daylight Time")
                    tz.Value = "Eastern Standard Time";

                return TimeZoneInfo.FindSystemTimeZoneById(tz.Value);
            }
            catch (Exception ex) 
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
        }

        public Int32 getTimeZoneOffsetValue(DateTime time, TimeZoneInfo timeZone) 
        {
            DateTime convertedTime = time;
            TimeSpan offset;

            if (time.Kind == DateTimeKind.Local && !timeZone.Equals(TimeZoneInfo.Local))
                convertedTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local, timeZone);
            else if (time.Kind == DateTimeKind.Utc && !timeZone.Equals(TimeZoneInfo.Utc))
                convertedTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Utc, timeZone);

            offset = timeZone.GetUtcOffset(time);
            return offset.Hours;
        }

        public String returnOffsetValue(Data.Location location, IMSEntities db)
        {
            UtilityManager utilMgr = new UtilityManager();

            TimeZoneInfo timezoneInfo = utilMgr.getTimeZoneInfoForEntity(location, db);

            return (timezoneInfo.IsDaylightSavingTime(DateTime.Now) ? timezoneInfo.BaseUtcOffset.TotalHours + 1 : timezoneInfo.BaseUtcOffset.TotalHours).ToString();

        }

        public DateTime getLocalTimeWithUTCTime(DateTime dateTime, String locationId, IMSEntities db)
        {
            Data.Location location = db.Locations.FirstOrDefault(a => a.TransaxId == locationId);

            Int32 offsetValue = Convert.ToInt32(returnOffsetValue(location, db));

            dateTime = dateTime.AddHours(offsetValue);

            return dateTime;
        }

        #region Slug Section

        public string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        #endregion

        #region Tax Calculation Section

        public decimal extractTaxAmountFromTotal(decimal total, decimal taxValue) 
        {
            decimal taxAmount = 0;

            if (total == 0)
                return taxAmount;

            if (taxValue == 0)
                return taxAmount;

            taxAmount = total - (total / (1 + taxValue));

            return taxAmount;
        }

        public decimal getTaxForAmount(decimal amount, decimal taxPercent)
        {
            decimal taxAmount = 0;

            if (amount == 0)
                return taxAmount;

            if (taxPercent == 0)
                return taxAmount;

            taxAmount = amount * taxPercent;

            return taxAmount;
        }

        #endregion

        #region Password Section

        public String generateTempPassword(int? length = 8, int? numberOfNonAlphaNumChar = 3) 
        {
            return System.Web.Security.Membership.GeneratePassword(length.Value, numberOfNonAlphaNumChar.Value);
        }

        #endregion

        public string MakeRelativePath(string absolutePath, string pivotFolder)
        {
            //string folder = Path.IsPathRooted(pivotFolder)
            //    ? pivotFolder : Path.GetFullPath(pivotFolder);
            string folder = pivotFolder;
            Uri pathUri = new Uri(absolutePath);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            Uri relativeUri = folderUri.MakeRelativeUri(pathUri);
            return Uri.UnescapeDataString(
                relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public string BuildImageFullPath(int imageTypeId, long identifier, string imageId)
        {
            string sectionPathForImage;

            switch (imageTypeId)
            {
                case 1:
                    sectionPathForImage = "members";
                    sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    sectionPathForImage = sectionPathForImage + "/avatar";
                    break;
                case 2:
                    sectionPathForImage = "users";
                    sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    sectionPathForImage = sectionPathForImage + "/avatar";
                    break;
                case 3:
                    sectionPathForImage = "merchants";
                    sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    sectionPathForImage = sectionPathForImage + "/logo";
                    break;
                case 4:
                    sectionPathForImage = "merchants";
                    sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    sectionPathForImage = sectionPathForImage + "/store";
                    break;
                case 5:
                    sectionPathForImage = "merchants";
                    sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    sectionPathForImage = sectionPathForImage + "/specimen";
                    break;
                default:
                    sectionPathForImage = "";
                    break;
            }

            return sectionPathForImage;
        }
    }

    
}
