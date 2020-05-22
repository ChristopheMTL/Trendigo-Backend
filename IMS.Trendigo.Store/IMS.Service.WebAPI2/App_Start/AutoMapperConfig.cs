using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Services;
using IMS.Common.Core.Utilities;
using IMS.Service.WebAPI2.Models;

namespace IMS.Service.WebAPI2
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MemberProfile());
                cfg.AddProfile(new CreditCardProfile());
                //cfg.AddProfile(new DeviceTypeTranslationProfile());
                cfg.AddProfile(new CitiesProfile());
                cfg.AddProfile(new PromotionProfile());
                //cfg.AddProfile(new ReferralCampaignProfile());
                //cfg.AddProfile(new NotificationProfile());
                cfg.AddProfile(new TaggingProfile());
                //cfg.AddProfile(new PromoCodeProfile());
                cfg.AddProfile(new TagCategoryProfile());
                //cfg.AddProfile(new PricingCategoryProfile());
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new MerchantProfile());
                cfg.AddProfile(new ProgramProfile());
                cfg.AddProfile(new BankingProfile());
                cfg.AddProfile(new TagProfile());
                cfg.AddProfile(new PromotionProfile());
                cfg.AddProfile(new CountryProfile());
                cfg.AddProfile(new CreditCardTypeProfile());
            });
        }

        public class CreditCardTypeProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<CreditCardType, CreditCardTypeRS>();
                map.ForMember(x => x.creditCardTypeId, o => o.MapFrom(model => model.Id));
                map.ForMember(x => x.name, o => o.MapFrom(model => model.Description));
            }
        }

        public class CountryProfile : Profile
        {
            protected override void Configure()
            {
                var map2 = Mapper.CreateMap<country_translations, CountryLocaleDTO>();
                map2.ForMember(x => x.countryLocaleId, o => o.MapFrom(model => model.id));
                map2.ForMember(x => x.language, o => o.MapFrom(model => model.Locale));
                map2.ForMember(x => x.name, o => o.MapFrom(model => model.Name));

                var map3 = Mapper.CreateMap<Currency, CurrencyDTO>();
                map3.ForMember(x => x.currencyId, o => o.MapFrom(model => model.Id));
                map3.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map3.ForMember(x => x.code, o => o.MapFrom(model => model.Code));
                map3.ForMember(x => x.symbol, o => o.MapFrom(model => model.Symbol));

                var map = Mapper.CreateMap<Country, CountryDTO>();
                map.ForMember(x => x.countryId, o => o.MapFrom(model => model.Id));
                map.ForMember(x => x.name, o => o.MapFrom(model => model.Name));
                map.ForMember(x => x.code, o => o.MapFrom(model => model.Alpha3Code));
                map.ForMember(x => x.currency, o => o.MapFrom(model => model.Currency));
                map.ForMember(x => x.locale, o => o.MapFrom(model => model.country_translations));

                var map5 = Mapper.CreateMap<state_translations, StateLocaleDTO>();
                map5.ForMember(x => x.stateLocaleId, o => o.MapFrom(model => model.id));
                map5.ForMember(x => x.language, o => o.MapFrom(model => model.Locale));
                map5.ForMember(x => x.name, o => o.MapFrom(model => model.Name));

                var map4 = Mapper.CreateMap<State, StateDTO>();
                map4.ForMember(x => x.stateId, o => o.MapFrom(model => model.Id));
                map4.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map4.ForMember(x => x.code, o => o.MapFrom(model => model.Alpha2Code));
                map4.ForMember(x => x.locale, o => o.MapFrom(model => model.state_translations));
            }
        }

        public class PromotionProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<Promotion_Schedules, getRewardRS>();
                map.ForMember(x => x.rewardId, o => o.MapFrom(model => model.Id));
                map.ForMember(x => x.rewardDate, o => o.MapFrom(model => model.StartDate));
                map.ForMember(x => x.rewardPercentage, o => o.MapFrom(model => model.Value < 1 ? model.Value * 100 : model.Value));

                var map2 = Mapper.CreateMap<Promotion, PromotionViewModel>();
            }
        }

        public class TagProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<tagging, AddTagRS>();
                map.ForMember(x => x.taggingId, o => o.MapFrom(model => model.id));
                map.ForMember(x => x.tagId, o => o.MapFrom(model => model.tag_id));
            }
        }

        public class UserProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<UserNotification, UserNotificationDTO>();
                map.ForMember(x => x.deviceId, o => o.MapFrom(model => model.DeviceId));
                map.ForMember(x => x.notificationToken, o => o.MapFrom(model => model.NotificationToken));

                var map1 = Mapper.CreateMap<IMSUser, UserDTO>();
                map1.ForMember(x => x.email, o => o.MapFrom(model => model.AspNetUser.Email));
                map1.ForMember(x => x.firstName, o => o.MapFrom(model => model.FirstName));
                map1.ForMember(x => x.lastName, o => o.MapFrom(model => model.LastName));
                map1.ForMember(x => x.merchantId, o => o.MapFrom(model => model.Merchants.FirstOrDefault().Id));
                map1.ForMember(x => x.userId, o => o.MapFrom(model => model.Id));
                map1.ForMember(x => x.language, o => o.MapFrom(model => model.Language.ISO639_1));

                var map2 = Mapper.CreateMap<UpdateUserRQ, IMSUser>();
                map2.ForMember(x => x.FirstName, o => o.MapFrom(model => model.firstName));
                map2.ForMember(x => x.LastName, o => o.MapFrom(model => model.lastName));
                map2.ForMember(x => x.LanguageId, o => o.MapFrom(model => new UtilityManager().getLanguageId(model.language)));
            }
        }

        public class MemberProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<MemberDTO, Member>();
                map.ForMember(x => x.Id, o => o.MapFrom(model => model.memberId));
                map.ForMember(x => x.TransaxId, o => o.MapFrom(model => model.transaxId));
                map.ForMember(x => x.UserId, o => o.MapFrom(model => model.userId));
                map.ForMember(x => x.FirstName, o => o.MapFrom(model => model.firstName));
                map.ForMember(x => x.LastName, o => o.MapFrom(model => model.lastName));
                map.ForMember(x => x.Language, o => o.Ignore());

                var map4 = Mapper.CreateMap<createMemberRQ, Member>();
                map4.ForMember(x => x.FirstName, o => o.MapFrom(model => model.firstName));
                map4.ForMember(x => x.LastName, o => o.MapFrom(model => model.lastName));

                var map5 = Mapper.CreateMap<Member, createMemberRS>();
                map5.ForMember(x => x.memberId, o => o.MapFrom(model => model.Id));
                map5.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map5.ForMember(x => x.firstName, o => o.MapFrom(model => model.FirstName));
                map5.ForMember(x => x.lastName, o => o.MapFrom(model => model.LastName));
                map5.ForMember(x => x.email, o => o.MapFrom(model => model.AspNetUser.Email));
                map5.ForMember(x => x.language, o => o.MapFrom(model => model.Language.ISO639_1));
                map5.ForMember(x => x.uid, o => o.MapFrom(model => model.AspNetUser.SocialMediaUsers.FirstOrDefault().UID));
                map5.ForMember(x => x.provider, o => o.MapFrom(model => model.AspNetUser.SocialMediaUsers.FirstOrDefault().Provider));

                var map2 = Mapper.CreateMap<NotificationMessageTranslation, NotificationTranslationDTO>();
                map2.ForMember(x => x.language, o => o.MapFrom(model => model.Language.ISO639_1));
                map2.ForMember(x => x.message, o => o.MapFrom(model => model.Description));

                var map3 = Mapper.CreateMap<Notification, NotificationDTO>();
                map3.ForMember(x => x.notificationId, o => o.MapFrom(model => model.Id));
                map3.ForMember(x => x.message, o => o.MapFrom(model => model.NotificationType.NotificationMessage));
                map3.ForMember(x => x.locale, o => o.MapFrom(model => model.NotificationType.NotificationMessage.NotificationMessageTranslations));

                var map6 = Mapper.CreateMap<CreditCard, MemberCreditCardDTO>();
                map6.ForMember(vm => vm.creditCardId, o => o.MapFrom(model => model.Id));
                map6.ForMember(vm => vm.transaxId, o => o.MapFrom(model => model.TransaxId));
                map6.ForMember(vm => vm.creditCardTypeId, o => o.MapFrom(model => model.CreditCardTypeId));
                map6.ForMember(vm => vm.cardHolderName, o => o.MapFrom(model => model.CardHolder));
                map6.ForMember(vm => vm.cardNumber, o => o.MapFrom(model => model.CardNumber));
                map6.ForMember(vm => vm.expiryDate, o => o.MapFrom(model => model.ExpiryDate));

                var map7 = Mapper.CreateMap<Member, MemberDTO>();
                map7.ForMember(x => x.memberId, o => o.MapFrom(model => model.Id));
                map7.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map7.ForMember(x => x.firstName, o => o.MapFrom(model => model.FirstName));
                map7.ForMember(x => x.lastName, o => o.MapFrom(model => model.LastName));
                map7.ForMember(x => x.email, o => o.MapFrom(model => model.AspNetUser.Email));
                map7.ForMember(x => x.uid, o => o.MapFrom(model => model.AspNetUser.SocialMediaUsers.FirstOrDefault().UID));
                map7.ForMember(x => x.provider, o => o.MapFrom(model => model.AspNetUser.SocialMediaUsers.FirstOrDefault().Provider));
                map7.ForMember(x => x.language, o => o.MapFrom(model => model.Language.ISO639_1));
                map7.ForMember(x => x.avatar, o => o.MapFrom(model => model.AvatarLink));
                map7.ForMember(x => x.creditCards, o => o.MapFrom(model => model.CreditCards));

            }
        }

        public class CreditCardProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<CreditCardRQ, CreditCardDTO>();
                map.ForMember(x => x.CardHolder, o => o.MapFrom(model => model.cardHolderName));
                map.ForMember(x => x.CardNumber, o => o.MapFrom(model => model.cardNumber));
                map.ForMember(x => x.CreditCardTypeId, o => o.MapFrom(model => model.creditCardTypeId));
                map.ForMember(x => x.Token, o => o.MapFrom(model => model.token));
                map.ForMember(x => x.ExpiryDate, o => o.MapFrom(model => model.expiryDate));

                var map2 = Mapper.CreateMap<CreditCardRQ, CreditCard>();
                map2.ForMember(x => x.MemberId, o => o.MapFrom(model => model.memberId));
                map2.ForMember(x => x.CardHolder, o => o.MapFrom(model => model.cardHolderName));
                map2.ForMember(x => x.CardNumber, o => o.MapFrom(model => model.cardNumber));
                map2.ForMember(x => x.CreditCardTypeId, o => o.MapFrom(model => model.creditCardTypeId));
                map2.ForMember(x => x.Token, o => o.MapFrom(model => model.token));
                map2.ForMember(x => x.ExpiryDate, o => o.MapFrom(model => model.expiryDate));

                var map3 = Mapper.CreateMap<CreditCard, CreditCardRS>();
                map3.ForMember(x => x.cardholderName, o => o.MapFrom(model => model.CardHolder));
                map3.ForMember(x => x.cardNumber, o => o.MapFrom(model => model.CardNumber));
                map3.ForMember(x => x.creditCardId, o => o.MapFrom(model => model.Id));
                map3.ForMember(x => x.creditCardTypeId, o => o.MapFrom(model => model.CreditCardTypeId));
                map3.ForMember(x => x.expiryDate, o => o.MapFrom(model => model.ExpiryDate));
                map3.ForMember(x => x.memberId, o => o.MapFrom(model => model.MemberId));
                map3.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map3.ForMember(x => x.creditCardType, o => o.Ignore());

                var map4 = Mapper.CreateMap<CreditCard, CreditCardDTO>();
                map4.ForMember(x => x.CardHolder, o => o.MapFrom(model => model.CardHolder));
                map4.ForMember(x => x.CardNumber, o => o.MapFrom(model => model.CardNumber));
                map4.ForMember(x => x.Id, o => o.MapFrom(model => model.Id));
                map4.ForMember(x => x.CreditCardTypeId, o => o.MapFrom(model => model.CreditCardTypeId));
                map4.ForMember(x => x.ExpiryDate, o => o.MapFrom(model => model.ExpiryDate));
                map4.ForMember(x => x.MemberId, o => o.MapFrom(model => model.MemberId));
                map4.ForMember(x => x.TransaxId, o => o.MapFrom(model => model.TransaxId));
                map4.ForMember(x => x.CreditCardType, o => o.Ignore());
            }
        }

        public class MerchantProfile : Profile
        {
            protected override void Configure()
            {
                var map12 = Mapper.CreateMap<BankingInfo, BankAccountRS>();
                map12.ForMember(x => x.bankAccountId, o => o.MapFrom(model => model.Id));
                map12.ForMember(x => x.accountName, o => o.MapFrom(model => model.AccountName));
                map12.ForMember(x => x.account, o => o.MapFrom(model => model.Account));
                map12.ForMember(x => x.branch, o => o.MapFrom(model => model.Branch));
                map12.ForMember(x => x.transit, o => o.MapFrom(model => model.Transit));
                map12.ForMember(x => x.specimenPath, o => o.MapFrom(model => model.SpecimenPath));

                var map11 = Mapper.CreateMap<IMSUser, MerchantAdminRS>();
                map11.ForMember(x => x.merchantAdminId, o => o.MapFrom(model => model.Id));
                map11.ForMember(x => x.email, o => o.MapFrom(model => model.AspNetUser.Email));
                map11.ForMember(x => x.firstName, o => o.MapFrom(model => model.FirstName));
                map11.ForMember(x => x.lastName, o => o.MapFrom(model => model.LastName));
                map11.ForMember(x => x.avatar, o => o.MapFrom(model => model.AvatarLink));
                map11.ForMember(x => x.notifications, o => o.MapFrom(model => model.AspNetUser.UserNotifications.ToList()));

                var map10 = Mapper.CreateMap<tag_translations, tagLocale>();
                map10.ForMember(x => x.locale, o => o.MapFrom(model => model.locale));
                map10.ForMember(x => x.tagName, o => o.MapFrom(model => model.name));

                var map9 = Mapper.CreateMap<MerchantTag, MerchantTagRS>();
                map9.ForMember(x => x.merchantTagId, o => o.MapFrom(model => model.Id));
                map9.ForMember(x => x.name, o => o.MapFrom(model => model.tag.name));
                map9.ForMember(x => x.locale, o => o.MapFrom(model => model.tag.tag_translations));

                var map8 = Mapper.CreateMap<merchant_translations, MerchantLocaleRS>();
                map8.ForMember(x => x.merchantLocaleId, o => o.MapFrom(model => model.id));
                map8.ForMember(x => x.merchantName, o => o.MapFrom(model => model.Name));
                map8.ForMember(x => x.merchantDesc, o => o.MapFrom(model => model.ShortDescription));
                map8.ForMember(x => x.locale, o => o.MapFrom(model => model.locale));

                var map7 = Mapper.CreateMap<MerchantImage, merchantImageRS>();
                map7.ForMember(x => x.imageId, o => o.MapFrom(model => model.Id.ToString()));
                map7.ForMember(x => x.path, o => o.MapFrom(model => model.ImagePath));

                var map6 = Mapper.CreateMap<LocationHoliday, HolidayRS>();
                map6.ForMember(x => x.holidayId, o => o.MapFrom(model => model.Id));
                map6.ForMember(x => x.name, o => o.MapFrom(model => model.Name));
                map6.ForMember(x => x.startingDate, o => o.MapFrom(model => model.FromDate));
                map6.ForMember(x => x.endDate, o => o.MapFrom(model => model.ToDate));

                var map5 = Mapper.CreateMap<LocationBusinessHour, BusinessHourRS>();
                map5.ForMember(x => x.businessHourId, o => o.MapFrom(model => model.Id));
                map5.ForMember(x => x.dayOfWeek, o => o.MapFrom(model => model.DayOfWeekID));
                map5.ForMember(x => x.openingHour, o => o.MapFrom(model => model.OpeningHour));
                map5.ForMember(x => x.closingHour, o => o.MapFrom(model => model.ClosingHour));

                var map44 = Mapper.CreateMap<Currency, locationCurrencyRS>();
                map44.ForMember(x => x.currencyId, o => o.MapFrom(model => model.Id));
                map44.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map44.ForMember(x => x.name, o => o.MapFrom(model => model.Name));
                map44.ForMember(x => x.symbol, o => o.MapFrom(model => model.Symbol));
                map44.ForMember(x => x.code, o => o.MapFrom(model => model.Code));

                var map4 = Mapper.CreateMap<Location, MerchantLocationRS>();
                map4.ForMember(x => x.locationId, o => o.MapFrom(model => model.Address.Locations.FirstOrDefault().Id));
                map4.ForMember(x => x.longitude, o => o.MapFrom(model => model.Address.Longitude));
                map4.ForMember(x => x.latitude, o => o.MapFrom(model => model.Address.Latitude));
                map4.ForMember(x => x.city, o => o.MapFrom(model => model.Address.City));
                map4.ForMember(x => x.country, o => o.MapFrom(model => model.Address.Country.Name));
                map4.ForMember(x => x.phone, o => o.MapFrom(model => model.Telephone));
                map4.ForMember(x => x.state, o => o.MapFrom(model => model.Address.State.Name));
                map4.ForMember(x => x.streetAddress, o => o.MapFrom(model => model.Address.StreetAddress));
                map4.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map4.ForMember(x => x.zip, o => o.MapFrom(model => model.Address.Zip));
                map4.ForMember(x => x.businessHours, o => o.MapFrom(model => model.LocationBusinessHours.ToList()));
                map4.ForMember(x => x.holidays, o => o.MapFrom(model => model.LocationHolidays.ToList()));
                map4.ForMember(x => x.enableTips, o => o.MapFrom(model => model.EnableTips));
                map4.ForMember(x => x.payWithPoints, o => o.MapFrom(model => model.PayWithPoints));
                map4.ForMember(x => x.currency, o => o.MapFrom(model => model.Address.Country.Currency));

                var map3 = Mapper.CreateMap<UserNotification, NotificationRS>();
                map3.ForMember(x => x.deviceId, o => o.MapFrom(model => model.DeviceId));
                map3.ForMember(x => x.notificationToken, o => o.MapFrom(model => model.NotificationToken));

                var map2 = Mapper.CreateMap<IMSUser, MerchantClerkRS>();
                map2.ForMember(x => x.clerkId, o => o.MapFrom(model => model.Id));
                map2.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map2.ForMember(x => x.email, o => o.MapFrom(model => model.AspNetUser.Email));
                map2.ForMember(x => x.firstName, o => o.MapFrom(model => model.FirstName));
                map2.ForMember(x => x.lastName, o => o.MapFrom(model => model.LastName));
                map2.ForMember(x => x.notifications, o => o.MapFrom(model => model.AspNetUser.UserNotifications.ToList()));

                var map1 = Mapper.CreateMap<Merchant, MerchantRS>();
                map1.ForMember(x => x.merchantId, o => o.MapFrom(model => model.Id));
                map1.ForMember(x => x.transaxId, o => o.MapFrom(model => model.TransaxId));
                map1.ForMember(x => x.name, o => o.MapFrom(model => model.Name));
                map1.ForMember(x => x.logo, o => o.MapFrom(model => model.LogoPath));
                map1.ForMember(x => x.status, o => o.MapFrom(model => model.Status));
                map1.ForMember(x => x.reward, o => o.MapFrom(model => new PromotionManager().GetPromotionPercentageForMerchant(model.Id, DateTime.Now.Date)));
                map1.ForMember(x => x.locales, o => o.MapFrom(model => model.merchant_translations));
                map1.ForMember(x => x.bankAccount, o => o.MapFrom(model => model.Locations.FirstOrDefault().BankingInfo));
                map1.ForMember(x => x.images, o => o.MapFrom(model => model.MerchantImages));
                map1.ForMember(x => x.locations, o => o.MapFrom(model => model.Locations));
                map1.ForMember(x => x.merchantAdmin, o => o.MapFrom(model => model.IMSUsers.Where(a => a.AspNetUser.AspNetRoles.FirstOrDefault().Name == IMSRole.MerchantAdmin.ToString()).FirstOrDefault()));
                map1.ForMember(x => x.clerks, o => o.MapFrom(model => model.IMSUsers.Where(a => a.AspNetUser.AspNetRoles.FirstOrDefault().Name == IMSRole.MerchantUser.ToString()).ToList()));
                map1.ForMember(x => x.category, o => o.MapFrom(model => model.MerchantTags.Where(a => a.tag.ParentId == null).FirstOrDefault()));
                map1.ForMember(x => x.tags, o => o.MapFrom(model => model.MerchantTags.Where(a => a.tag.ParentId != null).ToList()));
            }
        }

        public class CitiesProfile : Profile
        {
            protected override void Configure()
            {
                var map3 = Mapper.CreateMap<tag_translations, TagTranslationViewModel>();
                map3.ForMember(x => x.id, o => o.MapFrom(model => model.id));
                map3.ForMember(x => x.locale, o => o.MapFrom(model => model.locale));
                map3.ForMember(x => x.name, o => o.MapFrom(model => model.name));

                var map2 = Mapper.CreateMap<tag, TagViewModel>();
                map2.ForMember(x => x.id, o => o.MapFrom(model => model.id));
                map2.ForMember(x => x.CityId, o => o.MapFrom(model => model.CityId));
                map2.ForMember(x => x.name, o => o.MapFrom(model => model.name));
                map2.ForMember(x => x.tag_translations, o => o.MapFrom(model => model.tag_translations));

                //var map = Mapper.CreateMap<City, CitiesToBenefitViewModel>();
                //map.ForMember(x => x.Name, o => o.MapFrom(model => model.Name));
                //map.ForMember(x => x.Longitude, o => o.MapFrom(model => model.Longitude));
                //map.ForMember(x => x.Latitude, o => o.MapFrom(model => model.Latitude));
                //map.ForMember(x => x.tags, o => o.MapFrom(model => model.tags));
            }
        }

        public class TaggingProfile : Profile
        {
            protected override void Configure()
            {
                var map3 = Mapper.CreateMap<tag_translations, TagTranslationViewModel>();
                map3.ForMember(x => x.id, o => o.MapFrom(model => model.id));
                map3.ForMember(x => x.locale, o => o.MapFrom(model => model.locale));
                map3.ForMember(x => x.name, o => o.MapFrom(model => model.name));

                var map2 = Mapper.CreateMap<tag, TagViewModel>();
                map2.ForMember(x => x.id, o => o.MapFrom(model => model.id));
                map2.ForMember(x => x.CityId, o => o.MapFrom(model => model.CityId));
                map2.ForMember(x => x.name, o => o.MapFrom(model => model.name));
                map2.ForMember(x => x.tag_translations, o => o.MapFrom(model => model.tag_translations));

                //var map = Mapper.CreateMap<tagging, CitiesToBenefitViewModel>();
                //map.ForMember(x => x.Name, o => o.MapFrom(model => model.tag.City.Name));
                //map.ForMember(x => x.Longitude, o => o.MapFrom(model => model.tag.City.Longitude));
                //map.ForMember(x => x.Latitude, o => o.MapFrom(model => model.tag.City.Latitude));
                //map.ForMember(x => x.tags, o => o.Ignore());
            }
        }

        public class TagCategoryProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<tag_translations, TagCategoryTranslationViewModel>();
                map.ForMember(x => x.Id, o => o.MapFrom(model => model.id));
                map.ForMember(x => x.Language, o => o.MapFrom(model => model.locale));
                map.ForMember(x => x.Description, o => o.MapFrom(model => model.name));

                var map1 = Mapper.CreateMap<tag, TagCategoryViewModel>();
                map1.ForMember(x => x.Id, o => o.MapFrom(model => model.id));
                map1.ForMember(x => x.Description, o => o.MapFrom(model => model.name));
                map1.ForMember(x => x.Translations, o => o.MapFrom(model => model.tag_translations));
            }
        }

        public class ProgramProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<AddCommunityRQ, Program>();
                map.ForMember(x => x.Id, o => o.MapFrom(model => model.communityId));
                map.ForMember(x => x.ShortDescription, o => o.MapFrom(model => model.name));
                map.ForMember(x => x.Description, o => o.MapFrom(model => model.name));

                var map2 = Mapper.CreateMap<Program, CommunityRS>();
                map2.ForMember(x => x.communityId, o => o.MapFrom(model => model.Id));
                map2.ForMember(x => x.communityTypeId, o => o.MapFrom(model => model.ProgramTypeId));
                map2.ForMember(x => x.name, o => o.MapFrom(model => model.ShortDescription));
            }
        }

        public class BankingProfile : Profile
        {
            protected override void Configure()
            {
                var map = Mapper.CreateMap<addBankAccountRQ, BankingInfo>();
                map.ForMember(x => x.Account, o => o.MapFrom(model => model.account));
                map.ForMember(x => x.AccountName, o => o.MapFrom(model => model.accountName));
                map.ForMember(x => x.Branch, o => o.MapFrom(model => model.branch));
                map.ForMember(x => x.Transit, o => o.MapFrom(model => model.transit));
                map.ForMember(x => x.SpecimenPath, o => o.MapFrom(model => model.specimenPath));
            }
        }

        //public class PricingCategoryProfile : Profile
        //{
        //    protected override void Configure()
        //    {
        //        var map = Mapper.CreateMap<PricingCategory_translations, PricingCategoryTranslationViewModel>();
        //        map.ForMember(x => x.Id, o => o.MapFrom(model => model.Id));
        //        map.ForMember(x => x.Language, o => o.MapFrom(model => model.Language.ISO639_1));
        //        map.ForMember(x => x.Description, o => o.MapFrom(model => model.Description));

        //        var map1 = Mapper.CreateMap<PricingCategory, PricingCategoryViewModel>();
        //        map1.ForMember(x => x.Id, o => o.MapFrom(model => model.Id));
        //        map1.ForMember(x => x.Description, o => o.MapFrom(model => model.Description));
        //        map1.ForMember(x => x.Translations, o => o.MapFrom(model => model.PricingCategory_translations));
        //    }
        //}

        //public class NotificationProfile : Profile
        //{
        //    protected override void Configure()
        //    {
        //        var map = Mapper.CreateMap<NotificationMessageTranslation, NotificationViewModel>();
        //        map.ForMember(x => x.Id, o => o.MapFrom(model => model.NotificationMessage.NotificationTypes.FirstOrDefault().Notifications.FirstOrDefault().Id));
        //        map.ForMember(x => x.Language, o => o.MapFrom(model => model.Language.ISO639_1));
        //        map.ForMember(x => x.Message, o => o.MapFrom(model => model.Description));
        //    }
        //}

        //public class ReferralCampaignProfile : Profile
        //{
        //    protected override void Configure()
        //    {
        //        var map = Mapper.CreateMap<ReferralCampaign, ReferralCampaignViewModel>();
        //    }
        //}

        //public class PromoCodeProfile : Profile
        //{
        //    protected override void Configure()
        //    {
        //        var map = Mapper.CreateMap<PromoCode, PromoCodeResponseViewModel>();
        //    }
        //}
    }
}