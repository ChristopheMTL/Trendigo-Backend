using IMS.Common.Core.Data;
using IMS.Common.Core.Entities;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using IMS.Common.Core.DataCommands;
using System.Globalization;

namespace IMS.Common.Core.Services
{
    public class PromotionManager
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<Promotion> GetPromotionListWithRole(IMSUser user, bool ActiveOnly)
        {
            List<Promotion> promotions = new List<Promotion>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                return context.Promotions.ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                promotions = context.Promotions.Where(a => a.Programs.Any(b => b.Enterprise.Id == user.EnterpriseId)).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                promotions = context.Promotions.Where(a => a.Locations.Any(b => b.Merchant.IMSUsers.Any(c => c.Id == user.Id))).Distinct().ToList();
            }

            promotions = promotions.Where(a => a.IsActive == ActiveOnly).ToList();

            return promotions;
        }

        public Promotion GetPromotionWithRole(IMSUser user, string promotionId, bool ActiveOnly)
        {
            List<Promotion> promotions = GetPromotionListWithRole(user, ActiveOnly);
            var listOfPromotionId = promotions.Select(m => m.Id);

            return context.Promotions.Where(a => a.Id.ToString() == promotionId).Where(a => listOfPromotionId.Contains(a.Id)).FirstOrDefault();
        }

        public List<SelectListItem> GetAvailableValuesForPromotion(long enterpriseId, string promotionType, string packageId, string promotionId, string startDate)
        {
            List<SelectListItem> values = new List<SelectListItem>();

            Int32 startValue = 0;

            if (!String.IsNullOrEmpty(promotionId) && !String.IsNullOrEmpty(startDate))
            {
                Int64 promoId = Convert.ToInt64(promotionId);
                DateTime startDt = Convert.ToDateTime(startDate);
                startValue = Convert.ToInt32(context.Promotion_Schedules.Where(a => a.PromotionId == promoId && a.StartDate == startDt && a.IsActive == true).Select(b => b.Value).FirstOrDefault() * 100);
            }
            else
            {
                if (promotionType.ToLower() == "rebate")
                {
                    startValue = Convert.ToInt32(context.Packages.Where(a => a.Id.ToString() == packageId).Select(b => b.MinRebatePercent).FirstOrDefault() * 100);
                }
                else
                {
                    startValue = Convert.ToInt32(context.Packages.Where(a => a.Id.ToString() == packageId).Select(b => b.MinBonusPercent).FirstOrDefault() * 100);
                }
            }

            //Only merchant can add promotion at a different rate
            Int32 endValue = startValue;

            if (promotionType.ToLower() == "rebate")
            {
                //endValue = Convert.ToInt32(context.IMSEnterpriseParameters.Where(a => a.EnterpriseId == enterpriseId).Select(b => b.MaxPercentDiscountPerPromotion).FirstOrDefault() * 100);
                endValue = 50;
            }
            else
            {
                //endValue = Convert.ToInt32(context.IMSEnterpriseParameters.Where(a => a.EnterpriseId == enterpriseId).Select(b => b.MaxPercentAmountPerPromotion).FirstOrDefault() * 100);
                endValue = 50;
            }

            for (int i = startValue; i <= endValue; i += 5)
            {
                values.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return values;
        }

        public async Task CreateMonthlyPromotionScheduleFromCalendar(long locationId, long selectedCalendarId, DateTime targetedYearMonth, long userId)
        {
            #region Declaration Section

            Location location = await context.Locations.FirstOrDefaultAsync(a => a.Id == locationId);
            PromotionCalendar pc = await context.PromotionCalendars.FirstOrDefaultAsync(a => a.Id == selectedCalendarId);
            IMSUser user = await context.IMSUsers.FirstOrDefaultAsync(a => a.Id == userId);

            Promotion promotion = await context.Promotions.FirstOrDefaultAsync(a => a.IsActive == true && a.Package.MerchantId == location.MerchantId);
            int DaysInMonth = targetedYearMonth.AddMonths(1).AddDays(-1).Day;
            DateTime creationDate = DateTime.Now;

            #endregion

            #region Validation Section

            if (location == null)
            {
                throw new Exception(string.Format("Location not found for Id {0}", locationId));
            }

            if (pc == null)
            {
                throw new Exception(string.Format("PromotionCalendar not found for Id {0}", selectedCalendarId));
            }

            if (user == null)
            {
                throw new Exception(string.Format("User not found for Id {0}", userId));
            }

            if (promotion == null)
            {
                throw new Exception(string.Format("Promotion not found for merchantId {0}", location.MerchantId));
            }

            if (location.Merchant.IMSUsers.FirstOrDefault(a => a.Id == userId) == null && user.AspNetUser.AspNetRoles.FirstOrDefault().Name != IMSRole.IMSAdmin.ToString())
            {
                throw new Exception(string.Format("Cannot process calendar for promotionId {0} userId {1}", promotion.Id, userId));
            }

            #endregion

            for (int day = 1; day <= DaysInMonth; day += 1)
            {
                Promotion_Schedules promo_schedule = new Promotion_Schedules();
                String NewEventStart = string.Format("{0}-{1}-{2}", targetedYearMonth.Year, targetedYearMonth.Month, day < 10 ? "0" + day.ToString() : day.ToString());
                DateTime targetDate = Convert.ToDateTime(NewEventStart);

                Promotion_Schedules promoScheduleExist = await context.Promotion_Schedules.FirstOrDefaultAsync(a => a.PromotionId == promotion.Id && DbFunctions.TruncateTime(a.StartDate) == DbFunctions.TruncateTime(targetDate));
                if (promoScheduleExist != null)
                {
                    //logger.InfoFormat("PromotionManager - CreateMonthlyPromotionScheduleFromCalendar - PromotionSchedule already exist for PromotionId {0} and StartDate {1}", promotion.Id, NewEventStart);
                    continue;
                }

                var dayOfWeek = (int)Convert.ToDateTime(NewEventStart).DayOfWeek;
                var businessHour = promotion.Locations.FirstOrDefault(a => a.IsActive == true).LocationBusinessHours.FirstOrDefault(x => x.DayOfWeekID == dayOfWeek);

                TimeSpan? locationOpeningHour = businessHour != null ? businessHour.OpeningHour : (TimeSpan?)null;
                TimeSpan? locationClosingHour = businessHour != null ? businessHour.ClosingHour : (TimeSpan?)null;

                PromotionCalendarDetail pcd = pc.PromotionCalendarDetails.FirstOrDefault(a => a.IsActive == true && a.DayOfWeekId == dayOfWeek);

                //We create a promotion schedule if the location is open on the date selected and a value is present for that day and there is no promotion schedule for that promotion
                if (locationOpeningHour != null && locationClosingHour != null && pcd != null && promoScheduleExist == null)
                {
                    //Promotion Start Date Time
                    promo_schedule.StartDate = DateTime.Parse(NewEventStart, null, DateTimeStyles.RoundtripKind);
                    promo_schedule.StartTime = new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["Promotion.Start.Time"]), 0, 0);
                    //Promotion End Date Time
                    promo_schedule.EndDate = Convert.ToDateTime(NewEventStart).AddDays(1);
                    promo_schedule.EndTime = new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["Promotion.End.Time"]), 0, 0);

                    promo_schedule.Value = pcd.Value >= 1 ? pcd.Value / 100 : pcd.Value;
                    promo_schedule.CreatedBy = user.Id;
                    promo_schedule.IsPrime = false;
                    promo_schedule.PromotionId = promotion.Id;
                    promo_schedule.Promotion = promotion;

                    //Add promo_schedule
                    var command = DataCommandFactory.AddPromotionScheduleCommand(promo_schedule, context);
                    var result = await command.Execute();
                    if (result != DataCommandResult.Success)
                    {
                        logger.ErrorFormat("PromotionManager - CreateMonthlyPromotionScheduleFromCalendar - Unable to AddPromotionSchedule DataCommandResult {0}", result);
                        logger.DebugFormat("LocationId {0}", location.Id);
                        logger.DebugFormat("SelectedCalendarId {0}", pc.Id);
                        logger.DebugFormat("UserId {0}", userId);
                        logger.DebugFormat("TargetedYearMonth {0}", targetedYearMonth);
                        logger.DebugFormat("PromotionId {0}", promotion.Id);
                        logger.DebugFormat("PromotionScheduleDate {0}", NewEventStart);
                        logger.DebugFormat("DataCommandResult {0}", result);
                        continue;
                    }
                }
            }
        }

        public async Task DeletePromotionForMerchant(long MerchantId)
        {
            Package package = await context.Packages.FirstOrDefaultAsync(a => a.MerchantId == MerchantId && a.IsActive);

            if (package == null)
            {
                throw new Exception(string.Format("PromotionManager - DeletePromotionForMerchant - Package not found for MerchantId {0}", MerchantId));
            }

            Promotion promotion = await context.Promotions.FirstOrDefaultAsync(a => a.PackageId == package.Id && a.IsActive == true);

            if (promotion != null)
            {
                promotion.IsActive = false;
                promotion.ComingSoon = false;
                promotion.ModificationDate = DateTime.Now;

                context.Entry(promotion).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("PromotionManager - DeletePromotionForMerchant - MerchantId {0} Exception {1}", MerchantId, ex.ToString()));
                }
            }
        }

        public int GetPromotionPercentageForMerchant(long merchantId, DateTime startDate)
        {
            Location location = context.Locations.Where(a => a.MerchantId == merchantId).FirstOrDefault();

            Promotion promo = location.Promotions.FirstOrDefault(a => a.IsActive == true);

            if (promo == null)
            {
                return 0;
            }

            Promotion_Schedules promoSched = promo.Promotion_Schedules.FirstOrDefault(a => a.IsActive == true && a.StartDate.Date == startDate.Date);

            if (promoSched == null)
            {
                return 0;
            }

            return Convert.ToInt32(promoSched.Value < 1 ? promoSched.Value * 100 : promoSched.Value);
        }

        #region Base Calendar Section

        public async Task<Promotion> CreateBasePromotionSchedule(Promotion promotion, double basePercentage, DateTime startDate, DateTime endDate, IMSEntities db)
        {
            foreach (DateTime day in EachDay(startDate, endDate))
            {
                try
                {
                    Promotion_Schedules schedule = new Promotion_Schedules();

                    var dayOfWeek = (int)day.DayOfWeek;
                    LocationBusinessHour businessHour = promotion.Locations.FirstOrDefault(a => a.IsActive == true).LocationBusinessHours.FirstOrDefault(x => x.DayOfWeekID == dayOfWeek);

                    TimeSpan? locationOpeningHour = businessHour != null ? businessHour.OpeningHour : (TimeSpan?)null;
                    TimeSpan? locationClosingHour = businessHour != null ? businessHour.ClosingHour : (TimeSpan?)null;

                    LocationHoliday holiday = promotion.Locations.FirstOrDefault(a => a.IsActive == true).LocationHolidays.FirstOrDefault(x => x.FromDate.Date <= day.Date && x.ToDate >= day.Date);

                    if (holiday == null && locationOpeningHour != null && locationClosingHour != null)
                    {
                        schedule.StartDate = day;
                        schedule.StartTime = new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["Promotion.Start.Time"]), 0, 0);
                        //Promotion End Date Time
                        schedule.EndDate = day.AddDays(1);
                        schedule.EndTime = new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["Promotion.End.Time"]), 0, 0);

                        schedule.Value = basePercentage >= 1 ? basePercentage / 100 : basePercentage;
                        schedule.CreatedBy = 1;
                        schedule.IsPrime = false;
                        schedule.PromotionId = promotion.Id;

                        //Add promo_schedule
                        var command = DataCommandFactory.AddPromotionScheduleCommand(schedule, db);
                        var result = await command.Execute();
                        if (result == DataCommandResult.Success)
                        {
                            promotion.Promotion_Schedules.Add(schedule);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("CreateBasePromotionSchedule PromotionId {0} Day {1} Exception {2} InnerException {3}", promotion.Id, day.ToString(), ex.ToString(), ex.InnerException.ToString());
                    throw new Exception(ex.ToString());
                }
            }

            return promotion;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        #endregion
    }
}
