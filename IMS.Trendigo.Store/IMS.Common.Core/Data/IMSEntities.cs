using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Mapping;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;


namespace IMS.Common.Core.Data
{
    public class IMSEntities : IMS_StoreEntities
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            modelBuilder.Configurations.Add(new EnterpriseMap());
            modelBuilder.Configurations.Add(new IMSUserMap());
            modelBuilder.Configurations.Add(new MemberMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new EnterpriseStateTaxNumberMap());
            modelBuilder.Configurations.Add(new MerchantMap());
            modelBuilder.Configurations.Add(new AspNetUserMap());
            modelBuilder.Configurations.Add(new TelephoneMap());
            modelBuilder.Configurations.Add(new TelephoneTypeMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new CardStatusesMap());
            modelBuilder.Configurations.Add(new BankingInfoMap());
            modelBuilder.Configurations.Add(new OutsideChannelMap());
            modelBuilder.Configurations.Add(new SalesRepMap());
            modelBuilder.Configurations.Add(new ProgramMap());
            modelBuilder.Configurations.Add(new PromotionMap());
            modelBuilder.Configurations.Add(new PromotionTranslationMap());
            modelBuilder.Configurations.Add(new ContractMap());
            modelBuilder.Configurations.Add(new PromotionScheduleMap());
            modelBuilder.Configurations.Add(new TaggingMap());
            modelBuilder.Configurations.Add(new MerchantTranslationMap());
            modelBuilder.Configurations.Add(new TrxFinancialTransactionMap());
            modelBuilder.Configurations.Add(new TrxNonFinancialTransactionMap());
            modelBuilder.Configurations.Add(new IMSHeaderMap());
            modelBuilder.Configurations.Add(new IMSDetailMap());
            modelBuilder.Configurations.Add(new MembershipTransactionDetailMap());
            modelBuilder.Configurations.Add(new EnrollLocationMap());
            modelBuilder.Configurations.Add(new TagTranslationMap());
            modelBuilder.Configurations.Add(new GlobalResponseCodeMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new BankInfoMap());
            modelBuilder.Configurations.Add(new PaymentStatusMap());
            modelBuilder.Configurations.Add(new CardPointHistoryMap());
            modelBuilder.Configurations.Add(new MemberPromoCodeHistoryMap());
            modelBuilder.Configurations.Add(new SocialMediaUserMap());
            modelBuilder.Configurations.Add(new DeviceTypeTranslationMap());
            modelBuilder.Configurations.Add(new DeviceRequestMap());
            modelBuilder.Configurations.Add(new DeviceRequestStatusTranslationMap());
            modelBuilder.Configurations.Add(new EnrollmentMap());
            modelBuilder.Configurations.Add(new EnrollmentProcessMap());
            modelBuilder.Configurations.Add(new MonthlyBonusPointMap());
            modelBuilder.Configurations.Add(new TagMap());
            modelBuilder.Configurations.Add(new ReferralMap());
            modelBuilder.Configurations.Add(new NotificationMap());
            modelBuilder.Configurations.Add(new PricingCategoryMap());
            modelBuilder.Configurations.Add(new NewsletterMap());
            modelBuilder.Configurations.Add(new UserNotificationMap());
        }

        //private bool IsDirtyProperty(IMSEntities ctx, object entity, string propertyName)
        //{
        //    ObjectStateEntry entry;
            
        //    //http://www.c-sharpcorner.com/UploadFile/ff2f08/working-with-change-tracking-proxy-in-entity-framework-6-0/

        //    var stateManager = ((IObjectContextAdapter)ctx).ObjectContext.ObjectStateManager;
            
        //    if (stateManager.TryGetObjectStateEntry(entity, out entry))
        //    {
        //        int propIndex = this.GetPropertyIndex(entry, propertyName);

        //        if (propIndex != -1)
        //        {
        //            var oldValue = entry.OriginalValues[propIndex];
        //            var newValue = entry.CurrentValues[propIndex];

        //            return !Equals(oldValue, newValue);
        //        }
        //        else
        //        {
        //            throw new ArgumentException(String.Format("Cannot find original value for property '{0}' in entity entry '{1}'",
        //                    propertyName,
        //                    entry.EntitySet.ElementType.FullName));
        //        }
        //    }

        //    return false;
        //}

        //private int GetPropertyIndex(ObjectStateEntry entry, string propertyName)
        //{
        //   OriginalValueRecord record = entry.GetUpdatableOriginalValues();

        //   for (int i = 0; i <> record.FieldCount; i++)
        //   {
        //       FieldMetadata metaData = record.DataRecordInfo.FieldMetadata[i];
        //       if (metaData.FieldType.Name == propertyName)
        //       {
        //           return metaData.Ordinal;
        //       }
        //   }

        //   return -1;
        //}
    }
}
