using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;
using IMS.Common.Core.Enumerations;
using System.Runtime.ExceptionServices;
using IMS.Common.Core.Slack;

namespace IMS.Common.Core.Services
{
    public class NotificationService
    {
        IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Method that add a notification to a member account for the specified notificationTypeId
        /// </summary>
        /// <param name="userId">Identification of the user to add the notification</param>
        /// <param name="notificationTypeId">Identification of the notification type</param>
        /// <returns>The added notification</returns>
        public async Task<Notification> AddNotification(string userId, int notificationTypeId) 
        {
            Notification exist = await db.Notifications.FirstOrDefaultAsync(a => a.AspNetUser.Id == userId && a.NotificationTypeId == notificationTypeId && a.NotificationStatusId == (int)NotificationStatusEnum.READY);

            Notification notification;

            if (exist != null) 
            {
                return exist;
            }

            try
            {
                notification = new Notification();
                notification.UserId = userId;
                notification.NotificationTypeId = notificationTypeId;
                notification.CreationDate = DateTime.Now;
                notification.Removed = false;
                notification.NotificationStatusId = (int)NotificationStatusEnum.READY;

                db.Notifications.Add(notification);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("NotificationService - AddNotification - Error Adding Notification - UserId {0} Exception {1}", userId, ex.ToString());
                throw new Exception(string.Format("NotificationService - AddNotification - Error Adding Notification - UserId {0} Exception {1}", userId, ex.ToString()));
            }

            return notification;
        }

        /// <summary>
        /// Method that add a notification to a member account for the specified notificationTypeId and Campaign
        /// </summary>
        /// <param name="userId">Identification of the user to add the notification</param>
        /// <param name="notificationTypeId">Identification of the notification type</param>
        /// <param name="campaignId">Identification of the campaign targeted in that notification</param>
        /// <returns>The added notification</returns>
        public async Task<Notification> AddNotification(string userId, int notificationTypeId, int campaignId)
        {
            Notification exist = await db.Notifications.FirstOrDefaultAsync(a => a.AspNetUser.Id == userId && a.NotificationTypeId == notificationTypeId && a.NotificationStatusId == (int)NotificationStatusEnum.READY);

            Notification notification;

            if (exist != null)
            {
                return exist;
            }

            try
            {
                notification = new Notification();
                notification.UserId = userId;
                notification.NotificationTypeId = notificationTypeId;
                notification.CreationDate = DateTime.Now;
                notification.Removed = false;
                notification.NotificationStatusId = (int)NotificationStatusEnum.READY;
                notification.CampaignId = campaignId;

                db.Notifications.Add(notification);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("NotificationService - AddNotification - Error Adding Notification - UserId {0} Exception {1}", userId, ex.InnerException.ToString());
                throw new Exception(string.Format("NotificationService - AddNotification - Error Adding Notification - UserId {0} Exception {1}", userId, ex.InnerException.ToString()));
            }

            return notification;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="notificationTypeId"></param>
        /// <returns></returns>
        public async Task<Boolean> CanSendNotification(string userId, int notificationTypeId) 
        {
            Boolean canSendNotification = false;

            NotificationType notificationType = await db.NotificationTypes.FirstOrDefaultAsync(a => a.Id == notificationTypeId);

            if (notificationType == null)
                return canSendNotification;

            List<Notification> notifications = await db.Notifications.Where(a => a.UserId == userId && a.NotificationTypeId == notificationTypeId).ToListAsync();

            if (notificationType.Frequency > 0 && (notifications == null || notificationType.Frequency > notifications.Count))
                canSendNotification = true;

            return canSendNotification;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="visibleOnly"></param>
        /// <returns></returns>
        public async Task<List<NotificationMessageTranslation>> GetBannerNotifications(string userId, bool? visibleOnly = true) 
        {
            List<NotificationMessageTranslation> notifications = await db.NotificationMessageTranslations.Where(a => a.NotificationMessage.NotificationTypes.FirstOrDefault().Notifications.FirstOrDefault().UserId == userId && a.NotificationMessage.NotificationTypes.FirstOrDefault().DisplayBanner == true).ToListAsync();

            if (visibleOnly.Value == true) 
            {
                notifications = notifications.Where(a => a.NotificationMessage.NotificationTypes.FirstOrDefault().Notifications.FirstOrDefault().Removed == false).ToList();
            }

            return notifications;
        }

        #region CardActivation Section

        /// <summary>
        /// Method that send a card activation notification to a member. This will change the status of the notification to PROCESSED or ERROR depending of the process completion
        /// </summary>
        /// <param name="notification">This is the notification information</param>
        /// <param name="subject">This is the subject of the email</param>
        /// <param name="db">This is the data context to use</param>
        /// <returns>Nothing is returned</returns>
        public async Task CardActivationNotification(Notification notification, String language, String subject, IMSEntities db)
        {
            ExceptionDispatchInfo capturedException = null;

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                logger.ErrorFormat("NotificationService - CardActivationNotification - Notification not ready : NotificationId {0}", notification.Id);
                await ErrorNotification(notification.Id);
                return;
            }

            if (notification.CreationDate.AddDays(notification.NotificationType.Interval).Date > DateTime.Now.Date)
            {
                logger.ErrorFormat("NotificationService - CardActivationNotification - Wrong Date to notify : NotificationId {0} NotificationDate {1}", notification.Id, notification.CreationDate.AddDays(notification.NotificationType.Interval).Date.ToString());
                await ErrorNotification(notification.Id);
                return;
            }

            AspNetUser user = await db.AspNetUsers.FirstOrDefaultAsync(a => a.Id == notification.UserId);

            if (user == null)
            {
                logger.ErrorFormat("NotificationService - CardActivationNotification - User not found : NotificationId {0} UserId {1}", notification.Id, notification.UserId);
                await ErrorNotification(notification.Id);
                return;
            }
            
            Member member = user.Members.FirstOrDefault();

            if (member == null)
            {
                logger.ErrorFormat("NotificationService - CardActivationNotification - Member not found : NotificationId {0} UserId {1}", notification.Id, notification.UserId);
                await ErrorNotification(notification.Id);
                return;
            }

            IMSCard cardWaitingForActivation = await db.IMSCards.FirstOrDefaultAsync(a => a.MemberId == member.Id && a.CardStatusId == (int)IMSCardStatus.SHIPPED);

            if (cardWaitingForActivation != null)
            {
                try
                {
                    await new EmailService().SendCardNotificationEmail(notification, member, language, subject);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("NotificationService - CardActivationNotification - MemberId {0} Exception {1} InnerException {2}", member.Id, ex.ToString(), ex.InnerException.ToString());
                    capturedException = ExceptionDispatchInfo.Capture(ex);
                }

                if (capturedException != null)
                {
                    await ErrorNotification(notification.Id);
                    return;
                }
            }

            await SuccessNotification(notification.Id);
        }

        #endregion

        #region CreditCardExpiration Section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="subject"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task CreditCardExpirationNotification(Notification notification, String language, String subject, IMSEntities db)
        {
            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                logger.ErrorFormat("NotificationService - CreditCardExpirationNotification - Notification not ready : NotificationId {0}", notification.Id);
                await ErrorNotification(notification.Id);
                return;
            }

            AspNetUser user = await db.AspNetUsers.FirstOrDefaultAsync(a => a.Id == notification.UserId);

            if (user == null)
            {
                logger.ErrorFormat("NotificationService - CreditCardExpirationNotification - User not found : NotificationId {0} UserId {1}", notification.Id, notification.UserId);
                await ErrorNotification(notification.Id);
                return;
            }

            Member member = user.Members.FirstOrDefault();

            if (member == null)
            {
                logger.ErrorFormat("NotificationService - CreditCardExpirationNotification - Member not found : NotificationId {0} UserId {1}", notification.Id, notification.UserId);
                await ErrorNotification(notification.Id);
                return;
            }

            CreditCard expiringCard = null;

            foreach (CreditCard cc in member.CreditCards)
            {
                if (new CreditCardService().ExpiringThisMonth(cc.ExpiryDate))
                {
                    expiringCard = cc;
                    break;
                }
            }

            if (expiringCard == null)
            {
                logger.ErrorFormat("NotificationService - CreditCardExpirationNotification - Expiring Card not found : NotificationId {0} UserId {1}", notification.Id, notification.UserId);
                await ErrorNotification(notification.Id);
                return;
            }

            ExceptionDispatchInfo capturedException = null;

            try
            {
                await new EmailService().SendCreditCardExpirationEmail(member, expiringCard, language, subject);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("NotificationService - CreditCardExpirationNotification - MemberId {0} Exception {1} InnerException {2}", member.Id, ex.ToString(), ex.InnerException.ToString());
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        #endregion

        #region NoTransaction Section

        /// <summary>
        /// Method that send a survey notification to a member. This will change the status of the notification to PROCESSED or ERROR depending of the process completion.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="subject"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task NoTransactionNotification(Notification notification, String language, String subject, IMSEntities db)
        {
            #region Declaration Section

            Member member = null;
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                logger.ErrorFormat("NotificationService - NoTransactionNotification - Notification not ready : NotificationId {0}", notification.Id);
                await ErrorNotification(notification.Id);
                return;
            }

            try
            {
                member = await new MemberManager().GetMemberWithUserId(notification.UserId);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("NotificationService - NoTransactionNotification - Exception {0} : NotificationId {1} UserId {2}", ex.ToString(), notification.Id, notification.UserId);
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendSurveyNoTransactionEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("NotificationService - NoTransactionNotification - MemberId {0} Exception {1} InnerException {2}", member.Id, ex.ToString(), ex.InnerException.ToString());
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public async Task AddNoTransactionNotification(DateTime targetDate)
        //{
        //    List<String> financialCards = new List<String>();
        //    List<long> memberIds = new List<long>();

        //    try
        //    {
        //        financialCards = await db.TrxNonFinancialTransactions.Select(a => a.cardNonFinancialId).Distinct().ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("AddNoTransactionNotification - financialCards Exception {0} InnerException {1}", ex.ToString(), ex.InnerException.ToString()));
        //        return;
        //    }

        //    try
        //    {
        //        List<string> userIds = new List<string>();
        //        userIds = await db.Notifications.Where(a => a.NotificationTypeId == (int)NotificationTypeEnum.SurveyNoTransaction).Select(b => b.UserId).ToListAsync();
        //        memberIds = await db.Members.Where(a => userIds.Contains(a.AspNetUser.Id)).Select(b => b.Id).ToListAsync();
        //        //memberIds = await db.Notifications.Where(a => a.NotificationTypeId == (int)NotificationTypeEnum.SurveyNoTransaction).Select(b => b.MemberId).ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("AddNoTransactionNotification - memberIds Exception {0} InnerException {1}", ex.ToString(), ex.InnerException.ToString()));
        //        return;
        //    }

        //    List<IMSCard> cards = await db.IMSCards.Where(
        //        a => !financialCards.Contains(a.TransaxId) &&
        //        a.IsVirtual == false &&
        //        a.CardStatusId == (int)IMSCardStatus.ACTIVATED &&
        //        a.ActivationDate <= targetDate &&
        //        !memberIds.Contains(a.MemberId.Value)
        //        ).ToListAsync();

        //    foreach (IMSCard card in cards)
        //    {
        //        try
        //        {
        //            Notification notification = new Notification();
        //            notification.UserId = card.Member.AspNetUser.Id;
        //            notification.NotificationTypeId = (int)NotificationTypeEnum.SurveyNoTransaction;
        //            notification.NotificationStatusId = (int)NotificationStatusEnum.READY;
        //            notification.Removed = false;
        //            notification.CreationDate = DateTime.Now;

        //            db.Notifications.Add(notification);
        //            await db.SaveChangesAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("AddNoTransactionNotification - AddNotification MemberId {0} Exception {1} InnerException {2}", card.MemberId.Value, ex.ToString(), ex.InnerException.ToString()));
        //        }
        //    }
        //}

        #endregion

        #region CalendarRenewal Section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetMonth"></param>
        /// <returns></returns>
        //public async Task AddCalendarRenewalNotification(DateTime targetMonth) 
        //{
        //    DateTime firstDayOfMonth = new DateTime(targetMonth.Year, targetMonth.Month, 1);
        //    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        //    DateTime firstDayCurrentMonth = firstDayOfMonth.AddMonths(-1);
        //    DateTime lastDayCurrentMonth = firstDayCurrentMonth.AddMonths(1).AddDays(-1);

        //    List<Int64> merchantwithForecast = await db.PromotionScheduleForecasts.Where(a => a.StartDate >= firstDayOfMonth && a.EndDate <= lastDayOfMonth).Select(b => b.Promotion.Locations.FirstOrDefault().MerchantId).ToListAsync();
        //    List<Int64> merchantWithPromos = await db.Promotion_Schedules.Where(a => a.StartDate >= firstDayCurrentMonth && a.EndDate <= lastDayCurrentMonth).Select(b => b.Promotion.Locations.FirstOrDefault().MerchantId).ToListAsync();

        //    List<Merchant> merchants = await db.Merchants.Where(a => !merchantwithForecast.Contains(a.Id) && merchantWithPromos.Contains(a.Id) && a.IsActive == true).ToListAsync();

        //    foreach (Merchant merchant in merchants) 
        //    {
        //        IMSUser imsuser = merchant.IMSUsers.Where(a => a.AspNetUser.AspNetRoles.FirstOrDefault().Name == IMSRole.MerchantAdmin.ToString()).FirstOrDefault();

        //        if (imsuser == null) 
        //        {
        //            new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("AddCalendarRenewalNotification - IMSUser not found for MerchantId {0}", merchant.Id));
        //            continue;
        //        }

        //        List<Promotion_Schedules> PromosOfCurrentMonth = await db.Promotion_Schedules.Where(a => a.Promotion.Locations.FirstOrDefault().MerchantId == merchant.Id && a.StartDate >= firstDayCurrentMonth && a.EndDate <= lastDayCurrentMonth).OrderBy(b => b.StartDate).ToListAsync();

        //        if (PromosOfCurrentMonth != null) 
        //        {
        //            for (int i = firstDayCurrentMonth.Day; i <= lastDayOfMonth.Day; i++) 
        //            { 
        //                DateTime targetDate = new DateTime(firstDayCurrentMonth.Year, firstDayCurrentMonth.Month, i);
        //                Promotion_Schedules schedule = PromosOfCurrentMonth.Where(a => a.StartDate == targetDate).FirstOrDefault();

        //                if (schedule != null) 
        //                {
        //                    PromotionScheduleForecast forecast = new PromotionScheduleForecast();
        //                    forecast.PromotionId = schedule.PromotionId;
        //                    forecast.PromotionScheduleForecastStatusId = (int)PromotionScheduleForecastEnum.WaitingForApproval;
        //                    forecast.Value = schedule.Value;
        //                    forecast.StartDate = schedule.StartDate.AddMonths(1);
        //                    forecast.StartTime = schedule.StartTime;
        //                    forecast.EndDate = forecast.StartDate.AddDays(1);
        //                    forecast.EndTime = schedule.EndTime;
        //                    forecast.CreationDate = DateTime.Now;

        //                    db.PromotionScheduleForecasts.Add(forecast);
        //                    try 
        //                    {
        //                        await db.SaveChangesAsync();
        //                    }
        //                    catch (Exception ex) 
        //                    {
        //                        new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("AddCalendarRenewalNotification - AddPromotionScheduleForecast PromotionId {0} Exception {1} InnerException {2}", schedule.PromotionId.ToString(), ex.ToString(), ex.InnerException.ToString()));
        //                    }
        //                }
        //                else 
        //                {
        //                    Promotion_Schedules _schedule = PromosOfCurrentMonth.Where(a => a.StartDate.DayOfWeek == targetDate.DayOfWeek).FirstOrDefault();

        //                    if (_schedule != null)
        //                    {
        //                        PromotionScheduleForecast forecast = new PromotionScheduleForecast();
        //                        forecast.PromotionId = _schedule.PromotionId;
        //                        forecast.PromotionScheduleForecastStatusId = (int)PromotionScheduleForecastEnum.WaitingForApproval;
        //                        forecast.Value = _schedule.Value;
        //                        forecast.StartDate = _schedule.StartDate.AddMonths(1);
        //                        forecast.StartTime = _schedule.StartTime;
        //                        forecast.EndDate = _schedule.EndDate = schedule.EndDate;
        //                        forecast.EndTime = _schedule.EndTime;
        //                        forecast.CreationDate = DateTime.Now;

        //                        db.PromotionScheduleForecasts.Add(forecast);
        //                        try
        //                        {
        //                            await db.SaveChangesAsync();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("AddCalendarRenewalNotification - AddPromotionScheduleForecast _PromotionId {0} Exception {1} InnerException {2}", _schedule.PromotionId.ToString(), ex.ToString(), ex.InnerException.ToString()));
        //                        }
        //                    }
        //                }
        //            }

        //            try
        //            {
        //                Notification notification = new Notification();
        //                notification.UserId = imsuser.AspNetUser.Id;
        //                notification.NotificationTypeId = (int)NotificationTypeEnum.CalendarRenewal;
        //                notification.NotificationStatusId = (int)NotificationStatusEnum.READY;
        //                notification.Removed = false;
        //                notification.CreationDate = DateTime.Now;

        //                db.Notifications.Add(notification);
        //                await db.SaveChangesAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("AddCalendarRenewalNotification - AddNotification IMSUserId {0} Exception {1} InnerException {2}", imsuser.Id, ex.ToString(), ex.InnerException.ToString()));
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="subject"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task CalendarRenewalNotification(Notification notification, String language, String subject, IMSEntities db)
        {
            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                logger.ErrorFormat("NotificationService - CalendarRenewal - Notification not ready : NotificationId {0}", notification.Id);
                await ErrorNotification(notification.Id);
                return;
            }

            AspNetUser user = await db.AspNetUsers.FirstOrDefaultAsync(a => a.Id == notification.UserId);

            if (user == null)
            {
                logger.ErrorFormat("NotificationService - CalendarRenewalNotification - User not found : NotificationId {0} UserId {1}", notification.Id, notification.UserId);
                await ErrorNotification(notification.Id);
                return;
            }

            IMSUser IMSUser = user.IMSUsers.FirstOrDefault();

            if (IMSUser == null)
            {
                logger.ErrorFormat("NotificationService - CalendarRenewalNotification - IMSUser not found : NotificationId {0} IMSUserId {1}", notification.Id, notification.UserId);
                await ErrorNotification(notification.Id);
                return;
            }

            ExceptionDispatchInfo capturedException = null;

            try
            {
                await new EmailService().SendCalendarRenewalEmail(IMSUser, language, subject);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("NotificationService - CalendarRenewalNotification - IMSUserId {0} Exception {1} InnerException {2}", IMSUser.Id, ex.ToString(), ex.InnerException.ToString());
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        #endregion

        #region HalfMember Section

        public async Task AddHalfMemberFirstNotification(List<Member> members, NotificationType notificationType) 
        {
            await AddHalfMemberNotification(members, notificationType);
        }

        public async Task SendHalfMemberFirstNotification(Notification notification, String language, String subject) 
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendHalfMemberFirstNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null) 
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendHalfMemberFirstNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendHalfMemberFirstNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendHalfMemberFirstNotification Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddHalfMemberSecondNotification(List<Member> members, NotificationType notificationType)
        {
            await AddHalfMemberNotification(members, notificationType);
        }

        public async Task SendHalfMemberSecondNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendHalfMemberSecondNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendHalfMemberSecondNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendHalfMemberSecondNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendHalfMemberSecondNotification Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddHalfMemberSurveyNotification(List<Member> members, NotificationType notificationType)
        {
            await AddHalfMemberNotification(members, notificationType);
        }

        public async Task SendHalfMemberSurveyNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendHalfMemberSurveyNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendHalfMemberSurveyNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendHalfMemberSurveyNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendHalfMemberSurveyNotification Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        private async Task AddHalfMemberNotification(List<Member> members, NotificationType notificationType) 
        {
            List<String> notificationAlreadySent = await db.Notifications.Where(a => a.NotificationTypeId == notificationType.Id).Select(b => b.UserId).ToListAsync();
            DateTime targetDate = notificationType.Interval > 0 ? DateTime.Now.AddDays(notificationType.Interval * -1) : DateTime.Now;
            members = members.Where(a => !notificationAlreadySent.Contains(a.UserId) && DbFunctions.TruncateTime(a.CreationDate) >= DbFunctions.TruncateTime(targetDate)).ToList();

            foreach (Member member in members)
            {
                try
                {
                    Notification notification = await AddNotification(member.AspNetUser.Id, notificationType.Id, notificationType.Id);
                }
                catch (Exception ex)
                {
                    new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("{0} - MemberId {1} Exception {2} InnerException {3}", notificationType.Description, member.Id, ex.ToString(), ex.InnerException.ToString()));
                }
            }
        }

        #endregion

        #region FullMember Section

        public async Task AddFullMemberCardWaitingNotification(List<Member> members, NotificationType notificationType)
        {
            await AddFullMemberNotification(members, notificationType);
        }

        public async Task SendFullMemberCardWaitingNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardWaitingNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardWaitingNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendFullMemberCardWaitingNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendFullMemberCardWaitingNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddFullMemberCardReceivedNotification(List<Member> members, NotificationType notificationType)
        {
            await AddFullMemberNotification(members, notificationType);
        }

        public async Task SendFullMemberCardReceivedNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardReceivedNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardReceivedNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendFullMemberCardReceivedNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendFullMemberCardReceivedNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddFullMemberCardNotActivatedNotification(List<Member> members, NotificationType notificationType)
        {
            await AddFullMemberNotification(members, notificationType);
        }

        public async Task SendFullMemberCardNotActivatedNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardNotActivatedNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardNotActivatedNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendFullMemberCardNotActivatedNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendFullMemberCardNotActivatedNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddFullMemberCardActivatedNotification(List<Member> members, NotificationType notificationType)
        {
            await AddFullMemberNotification(members, notificationType);
        }

        public async Task SendFullMemberCardActivatedNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardActivatedNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFullMemberCardActivatedNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendFullMemberCardActivatedNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendFullMemberCardActivatedNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        private async Task AddFullMemberNotification(List<Member> members, NotificationType notificationType)
        {
            List<String> notificationAlreadySent = await db.Notifications.Where(a => a.NotificationTypeId == notificationType.Id).Select(b => b.UserId).ToListAsync();
            DateTime targetDate = notificationType.Interval > 0 ? DateTime.Now.AddDays(notificationType.Interval * -1) : DateTime.Now;
            members = members.Where(a => !notificationAlreadySent.Contains(a.UserId) && DbFunctions.TruncateTime(a.CreationDate) >= DbFunctions.TruncateTime(targetDate)).ToList();

            foreach (Member member in members)
            {
                try
                {
                    Notification notification = await AddNotification(member.AspNetUser.Id, notificationType.Id, notificationType.Id);
                }
                catch (Exception ex)
                {
                    new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("{0} - MemberId {1} Exception {2} InnerException {3}", notificationType.Description, member.Id, ex.ToString(), ex.InnerException.ToString()));
                }
            }
        }

        #endregion

        #region NoTransaction Section

        public async Task AddNoTransactionFirstNotification(List<Member> members, NotificationType notificationType)
        {
            await AddNoTransactionNotification(members, notificationType);
        }

        public async Task SendNoTransactionFirstNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionFirstNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionFirstNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendNoTransactionFirstNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendNoTransactionFirstNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddNoTransactionSecondNotification(List<Member> members, NotificationType notificationType)
        {
            await AddNoTransactionNotification(members, notificationType);
        }

        public async Task SendNoTransactionSecondNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionSecondNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionSecondNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendNoTransactionSecondNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendNoTransactionSecondNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddNoTransactionThirdNotification(List<Member> members, NotificationType notificationType)
        {
            await AddNoTransactionNotification(members, notificationType);
        }

        public async Task SendNoTransactionThirdNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionThirdNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionThirdNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendNoTransactionThirdNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendNoTransactionThirdNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddNoTransactionSurveyNotification(List<Member> members, NotificationType notificationType)
        {
            await AddNoTransactionNotification(members, notificationType);
        }

        public async Task SendNoTransactionSurveyNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionSurveyNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoTransactionSurveyNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendNoTransactionSurveyNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendNoTransactionSurveyNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        private async Task AddNoTransactionNotification(List<Member> members, NotificationType notificationType)
        {
            List<String> notificationAlreadySent = await db.Notifications.Where(a => a.NotificationTypeId == notificationType.Id).Select(b => b.UserId).ToListAsync();
            DateTime targetDate = notificationType.Interval > 0 ? DateTime.Now.AddDays(notificationType.Interval * -1) : DateTime.Now;
            members = members.Where(a => !notificationAlreadySent.Contains(a.UserId) && DbFunctions.TruncateTime(a.CreationDate) >= DbFunctions.TruncateTime(targetDate)).ToList();

            foreach (Member member in members)
            {
                try
                {
                    Notification notification = await AddNotification(member.AspNetUser.Id, notificationType.Id, notificationType.Id);
                }
                catch (Exception ex)
                {
                    new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("{0} - MemberId {1} Exception {2} InnerException {3}", notificationType.Description, member.Id, ex.ToString(), ex.InnerException.ToString()));
                }
            }
        }

        #endregion

        #region Transaction Section

        public async Task AddFirstTransactionNotification(List<Member> members, NotificationType notificationType)
        {
            await AddTransactionNotification(members, notificationType);
        }

        public async Task SendFirstTransactionNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFirstTransactionNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendFirstTransactionNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendFirstTransactionNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendFirstTransactionNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddSecondTransactionNotification(List<Member> members, NotificationType notificationType)
        {
            await AddTransactionNotification(members, notificationType);
        }

        public async Task SendSecondTransactionNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendSecondTransactionNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendSecondTransactionNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendSecondTransactionNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendSecondTransactionNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddThirdTransactionNotification(List<Member> members, NotificationType notificationType)
        {
            await AddTransactionNotification(members, notificationType);
        }

        public async Task SendThirdTransactionNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendThirdTransactionNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendThirdTransactionNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendThirdTransactionNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendThirdTransactionNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        public async Task AddNoFollowingTransactionNotification(List<Member> members, NotificationType notificationType)
        {
            await AddTransactionNotification(members, notificationType);
        }

        public async Task SendNoFollowingTransactionNotification(Notification notification, String language, String subject)
        {
            #region Declaration Section

            Member member = notification.AspNetUser.Members.FirstOrDefault();
            ExceptionDispatchInfo capturedException = null;

            #endregion

            #region Validation Section

            if (notification.NotificationStatusId != (int)NotificationStatusEnum.READY)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoFollowingTransactionNotification - Notification not ready : NotificationId {0}", notification.Id));
                await ErrorNotification(notification.Id);
                return;
            }

            if (member == null)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService - SendNoFollowingTransactionNotification - Member not found for userid {0}", notification.UserId));
                await ErrorNotification(notification.Id);
                return;
            }

            #endregion

            try
            {
                await new EmailService().SendNoFollowingTransactionNotificationEmail(member, language, subject);
            }
            catch (Exception ex)
            {
                new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("NotificationService SendNoFollowingTransactionNotificationEmail Exception {0} InnerException", notification.Id));
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ErrorNotification(notification.Id);
                return;
            }

            await SuccessNotification(notification.Id);
        }

        private async Task AddTransactionNotification(List<Member> members, NotificationType notificationType)
        {
            List<String> notificationAlreadySent = await db.Notifications.Where(a => a.NotificationTypeId == notificationType.Id).Select(b => b.UserId).ToListAsync();
            DateTime targetDate = notificationType.Interval > 0 ? DateTime.Now.AddDays(notificationType.Interval * -1) : DateTime.Now;
            members = members.Where(a => !notificationAlreadySent.Contains(a.UserId) && DbFunctions.TruncateTime(a.CreationDate) >= DbFunctions.TruncateTime(targetDate)).ToList();

            foreach (Member member in members)
            {
                try
                {
                    Notification notification = await AddNotification(member.AspNetUser.Id, notificationType.Id, notificationType.Id);
                }
                catch (Exception ex)
                {
                    new SlackClient().SlackAlert((int)SlackChannelEnum.NotificationTracker, string.Format("{0} - MemberId {1} Exception {2} InnerException {3}", notificationType.Description, member.Id, ex.ToString(), ex.InnerException.ToString()));
                }
            }
        }

        #endregion

        #region Notification Status Section

        /// <summary>
        /// Change the actual status of notification to ERROR.
        /// </summary>
        /// <param name="notificationId">Identifier of the notification process</param>
        /// <returns>No returned value</returns>
        public async Task ErrorNotification(long notificationId)
        {
            Notification notification = db.Notifications.FirstOrDefault(a => a.Id == notificationId);

            notification.NotificationStatusId = (int)NotificationStatusEnum.ERROR;
            db.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Change the actual status of notification to PROCESSED.
        /// </summary>
        /// <param name="notificationId">Identifier of the notification process</param>
        /// <param name="response">Response returned from the notification process</param>
        /// <returns>No returned value</returns>
        private async Task SuccessNotification(long notificationId)
        {
            Notification notification = db.Notifications.FirstOrDefault(a => a.Id == notificationId);

            notification.NotificationStatusId = (int)NotificationStatusEnum.PROCESSED;
            db.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("NotificationService - SuccessNotification - Unable to change status notificationId {0} exception {1} innerexception {2}", notificationId, ex.ToString(), ex.InnerException.ToString());
            }
        }

        #endregion

        #region Private Section

        

        #endregion
    }
}
