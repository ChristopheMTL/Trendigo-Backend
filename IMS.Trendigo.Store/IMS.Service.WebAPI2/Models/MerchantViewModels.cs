

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.Service.WebAPI2.Models
{
    #region Merchant Section

    public class merchantCoordinate
    {
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
    }

    public class getMerchantRQ
    {
        [Required]
        public string longitude { get; set; }
        [Required]
        public string latitude { get; set; }
        [Required]
        public int limit { get; set; }
    }

    public class addUserRQ
    {
        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string email { get; set; }

        public string language { get; set; }
    }

    public class addUserRS
    {
        public long userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string language { get; set; }
    }

    public class MerchantRQ
    {
        [Required]
        public long merchantAdminId { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string shortDesc { get; set; }
        [Required]
        public string streetAddress { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public int stateId { get; set; }
        [Required]
        public int countryId { get; set; }
        [Required]
        public string zip { get; set; }
        [Required]
        public string phone { get; set; }
    }

    public class UpdatePersonalInformationRQ
    {
        [Required]
        public string merchantAdminFirstName { get; set; }
        [Required]
        public string merchantAdminLastName { get; set; }
        [Required]
        public string name { get; set; }
        public List<UpdateMerchantLocationRQ> locations { get; set; }
    }

    public class UpdateMerchantInformationRQ
    {
        public List<UpdateMerchantLocaleRQ> merchantLocale { get; set; }
        public bool acceptTips { get; set; }
        public int categoryId { get; set; }
        public List<UpdateMerchantInformationTagRQ> tags { get; set; }
    }

    public class UpdateOperationalHoursRQ
    {
        public List<AddBusinessHourRQ> businessHours { get; set; }
        public List<HolidayRQ> holidays { get; set; }
    }

    public class HolidayRQ
    {
        public long? holidayId { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class UpdateMerchantInformationTagRQ
    {
        public long tagId { get; set; }
    }

    public class UpdateMerchantRQ
    {
        public long merchantId { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public int? baseReward { get; set; }
        public int? categoryId { get; set; }
        public bool acceptTips { get; set; }
        public List<UpdateMerchantImagesRQ> images { get; set; }
        public List<UpdateMerchantLocaleRQ> merchantLocale { get; set; }
        public List<UpdateMerchantLocationRQ> locations { get; set; }
    }

    public class UpdateMerchantImagesRQ
    {
        public Guid imageId { get; set; }
        public string path { get; set; }
    }

    public class UpdateMerchantLocaleRQ
    {
        public long merchantLocaleId { get; set; }
        public string description { get; set; }
        public string language { get; set; }
    }

    public class UpdateMerchantLocationRQ
    {
        public long locationId { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public int stateId { get; set; }
        public int countryId { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
    }

    public class OnboardingMerchantRS
    {
        public long merchantId { get; set; }
    }

    public class MerchantRS
    {
        public long merchantId { get; set; }
        public long transaxId { get; set; }
        public MerchantAdminRS merchantAdmin { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public int reward { get; set; }
        public string status { get; set; }
        public MerchantTagRS category { get; set; }
        public List<merchantImageRS> images { get; set; }
        public List<MerchantLocaleRS> locales { get; set; }
        public List<MerchantLocationRS> locations { get; set; }
        public List<MerchantTagRS> tags { get; set; }
        public List<MerchantClerkRS> clerks { get; set; }
        public CommunityRS community { get; set; }
    }

    public class MerchantByCategoryRS
    {
        public long merchantId { get; set; }
        public long transaxId { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public int promotion { get; set; }
        public string status { get; set; }
        public List<merchantImageRS> images { get; set; }
        public List<MerchantLocaleRS> locales { get; set; }
        public List<MerchantLocationRS> locations { get; set; }
        public List<MerchantTagRS> tags { get; set; }
    }

    public class MerchantAdminRS
    {
        public long merchantAdminId { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public List<NotificationRS> notifications { get; set; }
    }

    public class MerchantClerkRS
    {
        public long clerkId { get; set; }
        public long transaxId { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public List<NotificationRS> notifications { get; set; }
    }

    public class BankAccountRS
    {
        public long bankAccountId { get; set; }
        public string accountName { get; set; }
        public string transit { get; set; }
        public string branch { get; set; }
        public string account { get; set; }
        public string specimenPath { get; set; }
    }

    #endregion

    public class getMerchantByNameRQ
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string longitude { get; set; }

        [Required]
        public string latitude { get; set; }

        [Required]
        public int start { get; set; }

        [Required]
        public int limit { get; set; }
    }

    public class getMerchantByCategoryRQ
    {
        [Required]
        public int categoryId { get; set; }

        [Required]
        public string longitude { get; set; }

        [Required]
        public string latitude { get; set; }

        [Required]
        public int start { get; set; }

        [Required]
        public int limit { get; set; }
    }

    public class getMerchantByLocationRQ
    {
        [Required]
        public string locationName { get; set; }
        [Required]
        public string longitude { get; set; }
        [Required]
        public string latitude { get; set; }
        [Required]
        public int radius { get; set; }
        [Required]
        public string radiusType { get; set; }
        [Required]
        public int limit { get; set; }
    }

    public class merchantImageRS
    {
        public string imageId { get; set; }
        public string path { get; set; }
    }

    public class MerchantLocaleRS
    {
        public long merchantLocaleId { get; set; }
        public string merchantName { get; set; }
        public string merchantDesc { get; set; }
        public string locale { get; set; }
    }

    public class MerchantLocationRS
    {
        public long locationId { get; set; }
        public long transaxId { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public long stateId { get; set; }
        public string state { get; set; }
        public long countryId { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string phone { get; set; }
        public bool enableTips { get; set; }
        public bool payWithPoints { get; set; }
        public BankAccountRS bankingInfo { get; set; }
        public locationCurrencyRS currency { get; set; }
        public List<BusinessHourRS> businessHours { get; set; }
        public List<HolidayRS> holidays { get; set; }
    }

    public class locationCurrencyRS
    {
        public int currencyId { get; set; }
        public int transaxId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string symbol { get; set; }
    }

    public class locationTaxe
    {
        public string code { get; set; }
        public decimal value { get; set; }
        public int priority { get; set; }
    }

    public class locationBusinessHour
    {
        public int dayOfWeek { get; set; }
        public string openingHour { get; set; }
        public string closingHour { get; set; }
    }

    public class MerchantTagRS
    {
        public long TagId { get; set; }
        public long merchantTagId { get; set; }
        public string name { get; set; }
        public List<tagLocale> locale { get; set; }
    }

    public class tagLocale
    {
        public string tagName { get; set; }
        public string locale { get; set; }
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
    }

    public class addClerkRS
    {
        public long clerkId { get; set; }
        public int transaxId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string language { get; set; }
        public List<notificationRQ> notifications { get; set; }
    }

    public class UpdateUserRQ
    {
        public long userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string language { get; set; }
        public List<notificationRQ> notifications { get; set; }
    }

    public class notificationRQ
    {
        public string notificationId { get; set; }
    }

    public class ratingRQ
    {
        [Required]
        public string transactionId { get; set; }

        [Required]
        public int rating { get; set; }
    }

    #region Business hour Section

    public class AddBusinessHourRQ
    {
        public int dayOfWeek { get; set; }
        public string openingHour { get; set; }
        public string closingHour { get; set; }
        public bool isClosed { get; set; }
    }

    public class BusinessHourRS
    {
        public long businessHourId { get; set; }
        public int dayOfWeek { get; set; }
        public string openingHour { get; set; }
        public string closingHour { get; set; }
        public bool isClosed { get; set; }
    }

    public class UpdateBusinessHourRQ
    {
        public long businessHourId { get; set; }
        public int dayOfWeek { get; set; }
        public string openingHour { get; set; }
        public string closingHour { get; set; }
        public bool isClosed { get; set; }
    }

    #endregion

    #region Holiday Section

    public class AddHolidayRQ
    {
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class HolidayRS
    {
        public long holidayId { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class UpdateHolidayRQ
    {
        public long holidayId { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    #endregion

    #region Tag Section

    public class AddTagRQ
    {
        public int tagId { get; set; }
    }

    public class AddTagRS
    {
        public long taggingId { get; set; }
        public int tagId { get; set; }
    }

    #endregion

    #region Community Section

    public class AddCommunityRQ
    {
        public string name { get; set; }
    }

    public class UpdateCommunityRQ
    {
        public long communityId { get; set; }
        public int communityTypeId { get; set; }
        public string name { get; set; }
    }

    public class CommunityRS
    {
        public long communityId { get; set; }
        public int communityTypeId { get; set; }
        public string name { get; set; }
        public decimal fees { get; set; }
        public string currency { get; set; }
        public DateTime startDate { get; set; }
        public DateTime nextBillingDate { get; set; }
    }

    #endregion

    #region Bank Account Section

    public class addBankAccountRQ
    {
        public string accountName { get; set; }
        public string transit { get; set; }
        public string branch { get; set; }
        public string account { get; set; }
    }

    public class UpdateBankAccountRQ
    {
        public string accountName { get; set; }
        public string transit { get; set; }
        public string branch { get; set; }
        public string account { get; set; }
    }

    #endregion

    #region Transaction Section

    public class transactionHistoryRQ
    {
        public DateTime? fromDate { get; set; }

        public DateTime? toDate { get; set; }
        [Required]
        public int? start { get; set; }
        [Required]
        public int? length { get; set; }
    }

    public class MerchantTransactionRS
    {
        public int totalCount { get; set; }
        public List<MerchantTransactionHistoryRS> transactionHistory { get; set; }
    }

    public class MerchantTransactionHistoryRS
    {
        public long transactionId { get; set; }
        public long? transaxId { get; set; }
        public DateTime date { get; set; }
        public string cardNumber { get; set; }
        public string collaborator { get; set; }
        public decimal amount { get; set; }
        public decimal tip { get; set; }
        public decimal fees { get; set; }
        public int reward { get; set; }
        public int pointUsed { get; set; }
        public decimal toBePaid { get; set; }
        public string paymentStatus { get; set; }
    }

    public class TransactionStatsRS
    {
        public long transactionId { get; set; }
        public DateTime date { get; set; }
        public decimal amount { get; set; }
        public int reward { get; set; }
    }

    public class StatsRS
    {
        public int averagePerTransaction { get; set; }
        public int totalTransaction { get; set; }
        public int totalReward { get; set; }
        public int totalAmount { get; set; }
    }

    public class pointBalanceRS
    {
        public string communityType { get; set; }
        public string communityName { get; set; }
        public int points { get; set; }
        public long merchantId { get; set; }
        public string merchantName { get; set; }
    }

    public class memberCommunityPointRS
    {
        public int points { get; set; }
        public int communityTypeId { get; set; }
        public string communityType { get; set; }
    }

    #endregion

    #region QRCode Section

    public class qrCodeGenerationRequest
    {
        public bool supportTips { get; set; }
        public bool payWithPoints { get; set; }
        public string currency { get; set; }
        public qrCodeInformation qrCodeInformation { get; set; }
    }

    public class qrCodeInformation
    {
        [Required]
        public int imageSize { get; set; }
        public string imageFormat { get; set; }
    }

    public class qrCodeRQ
    {
        public int locationId { get; set; }
        public string clerk { get; set; }
        public int amount { get; set; }
        public string orderNumber { get; set; }
        public bool supportTips { get; set; }
        public bool payWithPoints { get; set; }
        public string currency { get; set; }
    }

    #endregion

    public class LoginMerchantRQ
    {
        [Required]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        public string deviceId { get; set; }

        [Required]
        public string notificationToken { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("LoginMerchantRQ {\n");
            sb.Append("  Email: ").Append(email).Append("\n");
            sb.Append("  Password: ").Append(password).Append("\n");
            sb.Append("  DeviceId: ").Append(deviceId).Append("\n");
            sb.Append("  NotificationToken: ").Append(notificationToken).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class LoginMerchantRS
    {
        public long userId { get; set; }
        public string userTranxId { get; set; }
        public string sessionToken { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public long merchantId { get; set; }
        public string merchantName { get; set; }
        public string status { get; set; }
        public int isCompleted { get; set; }
        public string logoFilePath { get; set; }
        public bool isCardExists { get; set; }
        public long locationId { get; set; }
        public string locationTransaxId { get; set; }
        public string locationAddress { get; set; }
        public string locationCity { get; set; }
        public string locationState { get; set; }
        public string locationCountry { get; set; }
        public string locationZip { get; set; }
        public bool enableTips { get; set; }
        public bool payWithPoints { get; set; }
        public string currency { get; set; }
        public int? communityTypeId { get; set; }
    }

    public class invoiceRQ
    {
        public int year { get; set; }
     
        public int month { get; set; }

        [Required]
        public int? start { get; set; }
        [Required]
        public int? length { get; set; }
    }

    public class addRewardRQ
    {
        [Required]
        public DateTime rewardDate { get; set; }

        [Required]
        public int rewardPercentage { get; set; }
    }

    public class addRewardRS
    {
        public long rewardId { get; set; }
        public long merchantId { get; set; }
        public DateTime rewardDate { get; set; }
        public int rewardPercentage { get; set; }
    }

    public class updateRewardRQ
    {
        public DateTime rewardDate { get; set; }
        public int rewardPercentage { get; set; }
    }

    public class getRewardRQ
    {
        [Required]
        public int year { get; set; }

        [Required]
        public int month { get; set; }
    }

    public class getRewardRS
    {
        public long rewardId { get; set; }
        public DateTime rewardDate { get; set; }
        public int rewardPercentage { get; set; }

    }

    public class MerchantSubscriptionsRS
    {
        public int totalCount { get; set; }
        public List<MerchantSubscriptionsInvoiceRS> invoices { get; set; }
    }

    public class MerchantSubscriptionsInvoiceRS
    {
        public long invoiceId { get; set; }
        public long merchantId { get; set; }
        public string merchantName { get; set; }
        public DateTime? date { get; set; }
        public decimal amount { get; set; }

    }

    public class MerchantQRCodeRS
    {
        public string type { get; set; }
        public string imagebase64 { get; set; }
    }

}