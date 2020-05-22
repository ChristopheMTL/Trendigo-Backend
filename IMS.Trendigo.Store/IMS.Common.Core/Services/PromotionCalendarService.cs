using IMS.Common.Core.Data;
using IMS.Common.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IMS.Common.Core.Services
{
    public class PromotionCalendarService
    {
        private static IMSEntities db = new IMSEntities();
        readonly protected log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        public async Task<PromotionCalendar> CreateDefaultPromotionCalendar(String PromotionCalendarName, Int64 EnterpriseId, Int32 PromotionCalendarTypeId, Double Monday, Double Tuesday, Double Wednesday, Double Thursday, Double Friday, Double Saturday, Double Sunday, long? LocationId)
        {
            PromotionCalendar pc = new PromotionCalendar();

            pc.CreationDate = DateTime.Now;
            pc.Description = PromotionCalendarName;
            pc.EnterpriseId = EnterpriseId;
            pc.IsActive = true;
            pc.PromotionCalendarTypeId = PromotionCalendarTypeId;

            pc.PromotionCalendarDetails = new List<PromotionCalendarDetail>();

            if (Monday > 0)
            {
                PromotionCalendarDetail _Monday = new PromotionCalendarDetail();
                _Monday.DayOfWeekId = 1;
                _Monday.IsActive = true;
                _Monday.CreationDate = DateTime.Now;
                _Monday.ModificationDate = DateTime.Now;
                _Monday.Value = Convert.ToDouble(Monday > 1 ? Monday / 100 : Monday);
                pc.PromotionCalendarDetails.Add(_Monday);
            }

            if (Tuesday > 0)
            {
                PromotionCalendarDetail _Tuesday = new PromotionCalendarDetail();
                _Tuesday.DayOfWeekId = 2;
                _Tuesday.IsActive = true;
                _Tuesday.CreationDate = DateTime.Now;
                _Tuesday.ModificationDate = DateTime.Now;
                _Tuesday.Value = Convert.ToDouble(Tuesday > 1 ? Tuesday / 100 : Tuesday);
                pc.PromotionCalendarDetails.Add(_Tuesday);
            }

            if (Wednesday > 0)
            {
                PromotionCalendarDetail _Wednesday = new PromotionCalendarDetail();
                _Wednesday.DayOfWeekId = 3;
                _Wednesday.IsActive = true;
                _Wednesday.CreationDate = DateTime.Now;
                _Wednesday.ModificationDate = DateTime.Now;
                _Wednesday.Value = Convert.ToDouble(Wednesday > 1 ? Wednesday / 100 : Wednesday);
                pc.PromotionCalendarDetails.Add(_Wednesday);
            }

            if (Thursday > 0)
            {
                PromotionCalendarDetail _Thursday = new PromotionCalendarDetail();
                _Thursday.DayOfWeekId = 4;
                _Thursday.IsActive = true;
                _Thursday.CreationDate = DateTime.Now;
                _Thursday.ModificationDate = DateTime.Now;
                _Thursday.Value = Convert.ToDouble(Thursday > 1 ? Thursday / 100 : Thursday);
                pc.PromotionCalendarDetails.Add(_Thursday);
            }

            if (Friday > 0)
            {
                PromotionCalendarDetail _Friday = new PromotionCalendarDetail();
                _Friday.DayOfWeekId = 5;
                _Friday.IsActive = true;
                _Friday.CreationDate = DateTime.Now;
                _Friday.ModificationDate = DateTime.Now;
                _Friday.Value = Convert.ToDouble(Friday > 1 ? Friday / 100 : Friday);
                pc.PromotionCalendarDetails.Add(_Friday);
            }

            if (Saturday > 0)
            {
                PromotionCalendarDetail _Saturday = new PromotionCalendarDetail();
                _Saturday.DayOfWeekId = 6;
                _Saturday.IsActive = true;
                _Saturday.CreationDate = DateTime.Now;
                _Saturday.ModificationDate = DateTime.Now;
                _Saturday.Value = Convert.ToDouble(Saturday > 1 ? Saturday / 100 : Saturday);
                pc.PromotionCalendarDetails.Add(_Saturday);
            }

            if (Sunday > 0)
            {
                PromotionCalendarDetail _Sunday = new PromotionCalendarDetail();
                _Sunday.DayOfWeekId = 0;
                _Sunday.IsActive = true;
                _Sunday.CreationDate = DateTime.Now;
                _Sunday.ModificationDate = DateTime.Now;
                _Sunday.Value = Convert.ToDouble(Sunday > 1 ? Sunday / 100 : Sunday);
                pc.PromotionCalendarDetails.Add(_Sunday);
            }

            if (LocationId.HasValue)
            {
                LocationPromotionCalendar lpc = new LocationPromotionCalendar();
                lpc.CreationDate = DateTime.Now;
                lpc.ModificationDate = DateTime.Now;
                lpc.Locationid = LocationId.Value;
                lpc.IsActive = true;
                pc.LocationPromotionCalendars.Add(lpc);
            }

            try
            {
                await CreatePromotionCalendar(pc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return pc;
        }

        public async Task<PromotionCalendar> CreatePromotionCalendar(PromotionCalendar newPromotionCalendar)
        {
            try
            {
                db.PromotionCalendars.Add(newPromotionCalendar);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("PromotionCalendarService - CreatePromotionCalendar - Exception {0}", ex.ToString());

                throw new Exception(string.Format("PromotionCalendarService - CreatePromotionCalendar - Exception {0}", ex.ToString()));
            }

            return newPromotionCalendar;
        }

        public async Task<PromotionCalendar> UpdatePromotionCalendar(Int64 promotionCalendarId, Boolean isActive, DateTime modificationDate)
        {
            PromotionCalendar pc = await db.PromotionCalendars.FirstOrDefaultAsync(a => a.Id == promotionCalendarId);

            if (pc == null)
            {
                throw new Exception(string.Format("Promotion Calendar not found for id {0}", promotionCalendarId));
            }

            try
            {
                pc.IsActive = isActive;

                db.Entry(pc).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("PromotionCalendarService - UpdatePromotionCalendar - Exception {0}", ex.ToString());
                throw new Exception(ex.ToString());
            }

            return pc;
        }

        public async Task<PromotionCalendar> UpdatePromotionCalendar(PromotionCalendar promotionCalendar)
        {
            try
            {
                db.Entry(promotionCalendar).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("PromotionCalendarService - UpdatePromotionCalendar - Exception {0}", ex.ToString());

                throw new Exception(string.Format("PromotionCalendarService - UpdatePromotionCalendar - Exception {0}", ex.ToString()));
            }

            return promotionCalendar;
        }

        public async Task DeletePromotionCalendar(Int64 PromotionCalendarId)
        {
            PromotionCalendar PromotionCalendarExist = await db.PromotionCalendars.FirstOrDefaultAsync(a => a.Id == PromotionCalendarId);

            if (PromotionCalendarExist != null)
            {
                PromotionCalendarExist.IsActive = false;

                LocationPromotionCalendar lpc = PromotionCalendarExist.LocationPromotionCalendars.FirstOrDefault(a => a.IsActive == true);
                if (lpc != null)
                {
                    lpc.IsActive = false;
                    db.Entry(lpc).State = EntityState.Modified;
                }

                db.Entry(PromotionCalendarExist).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("DeletePromotionCalendard Exception {0}", ex.ToString()));
                }
            }
        }

        public async Task<LocationPromotionCalendar> UpdateLocationPromotionCalendar(Int64 PromotionCalendarId, Int64 LocationId, Boolean isActive, DateTime modificationDate)
        {
            LocationPromotionCalendar lpc = await db.LocationPromotionCalendars.FirstOrDefaultAsync(a => a.Locationid == LocationId && a.PromotionCalendarId == PromotionCalendarId);

            if (lpc == null)
            {
                throw new Exception(string.Format("Location Promotion Calendar not found for Location Id {0} and Promotion Calendar Id {1}", LocationId, PromotionCalendarId));
            }

            try
            {
                lpc.IsActive = isActive;

                db.Entry(lpc).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("PromotionCalendarService - UpdateLocationPromotionCalendar - Exception {0}", ex.ToString());
                throw new Exception(ex.ToString());
            }

            return lpc;
        }

        public async Task<PromotionCalendarNotification> CreatePromotionCalendarNotification(Int64 Locationid, DateTime TargetedPeriod, String Notification, DateTime SentDate, DateTime WarningDate, IMSEntities db)
        {
            PromotionCalendarNotification pcn = new PromotionCalendarNotification();
            pcn.TargetedDate = TargetedPeriod;
            pcn.LocationId = Locationid;
            pcn.NotificationNewsletter = Notification;
            pcn.NotificationDate = SentDate;
            pcn.WarningDate = WarningDate;
            db.PromotionCalendarNotifications.Add(pcn);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return pcn;

        }

        public async Task<PromotionCalendarNotification> UpdatePromotionCalendarWarning(Int64 PromotionCalendarNotificationId, String Notification, DateTime SentDate)
        {
            PromotionCalendarNotification pcn = await db.PromotionCalendarNotifications.FirstOrDefaultAsync(a => a.Id == PromotionCalendarNotificationId);

            if (pcn == null)
            {
                throw new Exception(string.Format("Promotion Calendar Notification not found for Id {0}", PromotionCalendarNotificationId));
            }

            pcn.WarningDate = SentDate;
            pcn.WarningNewsletter = Notification;

            db.Entry(pcn).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return pcn;

        }

        public async Task<PromotionCalendarNotification> CreatePromotionCalendarSelected(Int64 PromotionCalendarNotificationId, String SelectedNewsletter, DateTime SelectedDate, Int64 SelectedPromotionCalendarId, IMSUser User)
        {
            PromotionCalendarNotification pcn = await db.PromotionCalendarNotifications.FirstOrDefaultAsync(a => a.Id == PromotionCalendarNotificationId);

            if (pcn == null)
            {
                throw new Exception(string.Format("Promotion Calendar Notification not found for Id {0}", PromotionCalendarNotificationId));
            }

            pcn.SelectedDate = SelectedDate;
            pcn.SelectedBy = User.AspNetUser.Id;
            pcn.SelectedNewsletter = SelectedNewsletter;
            pcn.SelectedPromoCalendarId = SelectedPromotionCalendarId;

            db.Entry(pcn).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return pcn;

        }

        public String BuildPromotionCalendarNotification(Int32 promotionCalendardNotificationType, string filePath, List<PromotionCalendar> templateList, Location location, PromotionCalendarNotification pcn, PromotionCalendar defaultPromotionCalendar, IMSUser user)
        {
            #region Declaration Section

            string language = "en";

            if (user.Language != null)
                language = user.Language.ISO639_1;

            string emailBodyTemplateName = "";
            string htmlBody;
            string calendars = "";
            string textDefaultCalendar = IMSMessages.ResourceManager.GetString("TextDefaultCalendar_" + language);
            string textLinkDefault = IMSMessages.ResourceManager.GetString("PromotionCalendarLinkDefault_" + language);
            string textSundayShort = IMSMessages.ResourceManager.GetString("TextSundayShort_" + language);
            string textMondayShort = IMSMessages.ResourceManager.GetString("TextMondayShort_" + language);
            string textTuesdayShort = IMSMessages.ResourceManager.GetString("TextTuesdayShort_" + language);
            string textWednesdayShort = IMSMessages.ResourceManager.GetString("TextWednesdayShort_" + language);
            string textThursdayShort = IMSMessages.ResourceManager.GetString("TextThursdayShort_" + language);
            string textFridayShort = IMSMessages.ResourceManager.GetString("TextFridayShort_" + language);
            string textSaturdayShort = IMSMessages.ResourceManager.GetString("TextSaturdayShort_" + language);
            string adminURI = ConfigurationManager.AppSettings["EnvironmentAdminURI"];
            PromotionCalendar selectedCalendar = null;

            #endregion

            #region Initialization Section

            switch (promotionCalendardNotificationType)
            {
                case (int)Enumerations.PromotionCalendarNotificationType.NOTIFICATION:
                    //emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, string.Format("Email/PromotionCalendarNotification.{0}.html", language));
                    emailBodyTemplateName = Path.Combine(filePath, string.Format("PromotionCalendarNotification.{0}.html", language));
                    break;
                case (int)Enumerations.PromotionCalendarNotificationType.WARNING:
                    //emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, string.Format("Email/PromotionCalendarWarning.{0}.html", language));
                    emailBodyTemplateName = Path.Combine(filePath, string.Format("PromotionCalendarWarning.{0}.html", language));
                    break;
                case (int)Enumerations.PromotionCalendarNotificationType.SELECTED:
                    emailBodyTemplateName = Path.Combine(filePath, string.Format("PromotionCalendarSelection.{0}.html", language));
                    //emailBodyTemplateName = Path.Combine("../", string.Format("Views/Email/PromotionCalendarSelection.{0}.html", language));
                    selectedCalendar = templateList.FirstOrDefault();
                    break;
                case (int)Enumerations.PromotionCalendarNotificationType.SELECTED_AUTOMATICALLY:
                    emailBodyTemplateName = Path.Combine(filePath, string.Format("PromotionCalendarSelection.{0}.html", language));
                    //emailBodyTemplateName = Path.Combine("../../", string.Format("Email/PromotionCalendarSelection.{0}.html", language));
                    selectedCalendar = templateList.FirstOrDefault();
                    break;
                default:
                    throw new Exception("Unrecognized notification type");
            }

            #endregion

            #region Default Calendar Section

            if (promotionCalendardNotificationType != (int)Enumerations.PromotionCalendarNotificationType.SELECTED && promotionCalendardNotificationType != (int)Enumerations.PromotionCalendarNotificationType.SELECTED_AUTOMATICALLY) // || (promotionCalendardNotificationType == (int)Enumerations.PromotionCalendarNotificationType.SELECTED && selectedCalendar != null) //&& selectedCalendar.Id == defaultPromotionCalendar.Id
            {
                //Format link for default calendar
                MvcHtmlString linkDefault = EncryptionManager.EncodedActionLink(textLinkDefault, adminURI, "CalendarValidation", "PromotionCalendar", new { promotionCalendarNotificationId = pcn.Id, promotionCalendarId = defaultPromotionCalendar.Id, userId = user.Id }, null);

                calendars = string.Format("<p style='font-family:Verdana, Helvetica, Arial, sans-serif; color:#5E6166; font-size:12px; margin:0px 0px 10px 0px; mso-line-height-rule:exactly; line-height:16px;'><b>{0}</b>&nbsp;&nbsp;{1}</p>", textDefaultCalendar, linkDefault);

                //Week Calendar
                calendars += "<p style='font-family:Verdana, Helvetica, Arial, sans-serif; color:#5E6166; font-size:12px; margin:0px 0px 10px 0px; mso-line-height-rule:exactly; line-height:16px;'><table><tr>";

                var _Sunday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Sunday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textSaturdayShort, _Sunday != null ? string.Format("{0} %", _Sunday.Value * 100) : "N/A");

                var _Monday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Monday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textMondayShort, _Monday != null ? string.Format("{0} %", _Monday.Value * 100) : "N/A");

                var _Tuesday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Tuesday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textTuesdayShort, _Tuesday != null ? string.Format("{0} %", _Tuesday.Value * 100) : "N/A");

                var _Wednesday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Wednesday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textWednesdayShort, _Wednesday != null ? string.Format("{0} %", _Wednesday.Value * 100) : "N/A");

                var _Thursday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Thursday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textThursdayShort, _Thursday != null ? string.Format("{0} %", _Thursday.Value * 100) : "N/A");

                var _Friday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Friday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textFridayShort, _Friday != null ? string.Format("{0} %", _Friday.Value * 100) : "N/A");

                var _Saturday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Saturday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textSaturdayShort, _Saturday != null ? string.Format("{0} %", _Saturday.Value * 100) : "N/A");

                calendars += "</tr></table><br></p>";
            }

            #endregion

            #region Template Calendar Section

            foreach (PromotionCalendar pc in templateList)
            {
                //Calendar name
                PromotionCalendarTranslation pct = pc.PromotionCalendarTranslations.Where(a => a.Language.ISO639_1 == language).FirstOrDefault();
                string calendarName = (templateList.Count == 1 && templateList.First().Id == location.LocationPromotionCalendars.FirstOrDefault(a => a.IsActive == true).Id) ? textDefaultCalendar : pct != null ? pct.Description : pc.Description;

                if (promotionCalendardNotificationType != (int)Enumerations.PromotionCalendarNotificationType.SELECTED)
                {
                    //Format link for default calendar
                    MvcHtmlString linkTemplate = EncryptionManager.EncodedActionLink(textLinkDefault, adminURI, "CalendarValidation", "PromotionCalendar", new { promotionCalendarNotificationId = pcn.Id, promotionCalendarId = pc.Id, userId = user.Id }, null);
                    calendars += string.Format("<p style='font-family:Verdana, Helvetica, Arial, sans-serif; color:#5E6166; font-size:12px; margin:0px 0px 10px 0px; mso-line-height-rule:exactly; line-height:16px;'><b>{0}</b>&nbsp;&nbsp;{1}</p>", calendarName, linkTemplate);
                }
                else
                {
                    calendars += string.Format("<p style='font-family:Verdana, Helvetica, Arial, sans-serif; color:#5E6166; font-size:12px; margin:0px 0px 10px 0px; mso-line-height-rule:exactly; line-height:16px;'><b>{0}</b></p>", calendarName);
                }

                calendars += "<p style='font-family:Verdana, Helvetica, Arial, sans-serif; color:#5E6166; font-size:12px; margin:0px 0px 10px 0px; mso-line-height-rule:exactly; line-height:16px;'><table><tr>";

                var oSunday = pc.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Sunday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textSundayShort, oSunday != null ? string.Format("{0} %", oSunday.Value * 100) : "N/A");

                var oMonday = pc.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Monday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textMondayShort, oMonday != null ? string.Format("{0} %", oMonday.Value * 100) : "N/A");

                var oTuesday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Tuesday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textTuesdayShort, oTuesday != null ? string.Format("{0} %", oTuesday.Value * 100) : "N/A");

                var oWednesday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Wednesday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textWednesdayShort, oWednesday != null ? string.Format("{0} %", oWednesday.Value * 100) : "N/A");

                var oThursday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Thursday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textThursdayShort, oThursday != null ? string.Format("{0} %", oThursday.Value * 100) : "N/A");

                var oFriday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Friday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textFridayShort, oFriday != null ? string.Format("{0} %", oFriday.Value * 100) : "N/A");

                var oSaturday = defaultPromotionCalendar.PromotionCalendarDetails.FirstOrDefault(a => a.DayOfWeekId == (int)DayOfWeek.Saturday);
                calendars += string.Format("<td><table width ='50px'><tr><td>{0}</td></tr><tr><td>{1}</td></tr></table></td>", textSaturdayShort, oSaturday != null ? string.Format("{0} %", oSaturday.Value * 100) : "N/A");

                calendars += "</tr></table><br></p>";
            }

            #endregion

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = user.FirstName + " " + user.LastName;
                string merchant = location.Merchant.Name;
                string address = location.Address.StreetAddress;
                string calendar = calendars;

                htmlBody = String.Format(htmlBody, merchant, address, fullname, calendars);
            }

            return htmlBody;
        }

        public static String GetPromotionCalendarName(Int64 promotionCalendarId, String Language)
        {

            PromotionCalendar pc = db.PromotionCalendars.FirstOrDefault(a => a.Id == promotionCalendarId);

            if (pc == null)
                return "promotion calendar not found";

            if (string.IsNullOrEmpty(Language))
                return pc.Description;

            PromotionCalendarTranslation pct = pc.PromotionCalendarTranslations.FirstOrDefault(a => a.Language.ISO639_1 == Language);

            return pct != null ? pct.Description : pc.Description;
        }
    }
}
