﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMS.Common.Core.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public abstract class IMS_StoreEntities : DbContext
    {
        public IMS_StoreEntities()
            : base("name=IMS_StoreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CardStatus> CardStatuses { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<language_translations> language_translations { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<merchant_translations> merchant_translations { get; set; }
        public virtual DbSet<program_translations> program_translations { get; set; }
        public virtual DbSet<ProgramFee> ProgramFees { get; set; }
        public virtual DbSet<PromotionType> PromotionTypes { get; set; }
        public virtual DbSet<tag_translations> tag_translations { get; set; }
        public virtual DbSet<TelephoneType> TelephoneTypes { get; set; }
        public virtual DbSet<BankingInfo> BankingInfos { get; set; }
        public virtual DbSet<CardType> CardTypes { get; set; }
        public virtual DbSet<PrefixPointValue> PrefixPointValues { get; set; }
        public virtual DbSet<PackageType_translations> PackageType_translations { get; set; }
        public virtual DbSet<IMS_LineItem> IMS_LineItem { get; set; }
        public virtual DbSet<OutsideChannelStyle> OutsideChannelStyles { get; set; }
        public virtual DbSet<ProgramFeature> ProgramFeatures { get; set; }
        public virtual DbSet<promotion_translations> promotion_translations { get; set; }
        public virtual DbSet<IMSMembership> IMSMemberships { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<EntityType> EntityTypes { get; set; }
        public virtual DbSet<IMSCard> IMSCards { get; set; }
        public virtual DbSet<tagging> taggings { get; set; }
        public virtual DbSet<IMSUser> IMSUsers { get; set; }
        public virtual DbSet<LocationBusinessHour> LocationBusinessHours { get; set; }
        public virtual DbSet<EnrollLocation> EnrollLocations { get; set; }
        public virtual DbSet<LocationTax> LocationTaxes { get; set; }
        public virtual DbSet<Enterprise> Enterprises { get; set; }
        public virtual DbSet<MerchantImage> MerchantImages { get; set; }
        public virtual DbSet<program_feature_translations> program_feature_translations { get; set; }
        public virtual DbSet<country_translations> country_translations { get; set; }
        public virtual DbSet<state_translations> state_translations { get; set; }
        public virtual DbSet<CreditCardType> CreditCardTypes { get; set; }
        public virtual DbSet<Location_Terminals> Location_Terminals { get; set; }
        public virtual DbSet<Telephone> Telephones { get; set; }
        public virtual DbSet<MembershipTransactionDetail> MembershipTransactionDetails { get; set; }
        public virtual DbSet<MembershipTransactionHeader> MembershipTransactionHeaders { get; set; }
        public virtual DbSet<GlobalResponseCode> GlobalResponseCodes { get; set; }
        public virtual DbSet<PackageType> PackageTypes { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Promotion_Schedules> Promotion_Schedules { get; set; }
        public virtual DbSet<StateTax> StateTaxes { get; set; }
        public virtual DbSet<SalesRep> SalesReps { get; set; }
        public virtual DbSet<ContractLocation> ContractLocations { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<IMS_Detail> IMS_Detail { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<TrxNonFinancialTransaction> TrxNonFinancialTransactions { get; set; }
        public virtual DbSet<IMSPointTracker> IMSPointTrackers { get; set; }
        public virtual DbSet<ContractCommission> ContractCommissions { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<IMSTransferMessage> IMSTransferMessages { get; set; }
        public virtual DbSet<IMSTransferStatu> IMSTransferStatus { get; set; }
        public virtual DbSet<BankInfo> BankInfos { get; set; }
        public virtual DbSet<PaymentStatu> PaymentStatus { get; set; }
        public virtual DbSet<CardPointHistory> CardPointHistories { get; set; }
        public virtual DbSet<IMSEnterpriseParameterTerminal> IMSEnterpriseParameterTerminals { get; set; }
        public virtual DbSet<IMSEnterpriseParameter> IMSEnterpriseParameters { get; set; }
        public virtual DbSet<BatchCloseLog> BatchCloseLogs { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Terminal> Terminals { get; set; }
        public virtual DbSet<TrxFinancialTransaction> TrxFinancialTransactions { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<MemberPromoCodeHistory> MemberPromoCodeHistories { get; set; }
        public virtual DbSet<TimeZone> TimeZones { get; set; }
        public virtual DbSet<SocialMediaUser> SocialMediaUsers { get; set; }
        public virtual DbSet<DeviceRequestStatu> DeviceRequestStatus { get; set; }
        public virtual DbSet<DeviceRequest> DeviceRequests { get; set; }
        public virtual DbSet<DeviceRequestStatus_translations> DeviceRequestStatus_translations { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<DeviceTypeTranslation> DeviceTypeTranslations { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<EnrollmentStatu> EnrollmentStatus { get; set; }
        public virtual DbSet<EnrollmentProcess> EnrollmentProcesses { get; set; }
        public virtual DbSet<GiftCardTransactionDetail> GiftCardTransactionDetails { get; set; }
        public virtual DbSet<GiftCardTransactionHeader> GiftCardTransactionHeaders { get; set; }
        public virtual DbSet<GiftCardPurchas> GiftCardPurchases { get; set; }
        public virtual DbSet<MonthlyBonusPoint> MonthlyBonusPoints { get; set; }
        public virtual DbSet<CommandStatu> CommandStatus { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<IMS_Header> IMS_Header { get; set; }
        public virtual DbSet<tag> tags { get; set; }
        public virtual DbSet<ReferralCampaign> ReferralCampaigns { get; set; }
        public virtual DbSet<Referral> Referrals { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<EnterpriseStateTaxNumber> EnterpriseStateTaxNumbers { get; set; }
        public virtual DbSet<DirectDepositLog> DirectDepositLogs { get; set; }
        public virtual DbSet<NotificationMessage> NotificationMessages { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<NotificationMessageTranslation> NotificationMessageTranslations { get; set; }
        public virtual DbSet<PromoCode> PromoCodes { get; set; }
        public virtual DbSet<OutsideChannel> OutsideChannels { get; set; }
        public virtual DbSet<ProgramType> ProgramTypes { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
    }
}
