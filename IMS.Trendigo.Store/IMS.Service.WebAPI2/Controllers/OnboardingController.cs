using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Services;
using IMS.Common.Core.Utilities;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Models;
using IMS.Service.WebAPI2.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("onboarding")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class OnboardingController : ApiController
    {
        private ApplicationUserManager _userManager;
        private IMSEntities db = new IMSEntities();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructor Section

        public ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket> AccessTokenFormat { get; private set; }

        public ApplicationUserManager UserManager
        {
            get
            {
                //return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //if (_userManager == null)
                //{
                //    return new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                //}
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public OnboardingController()
        {
        }

        public OnboardingController(ApplicationUserManager userManager,
                    ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        #endregion

        [HttpPost]
        [AllowAnonymous]
        [Route("addMerchantAdmin")]
        public async Task<IHttpActionResult> AddMerchantAdmin([FromBody] OnboardingUserRQ model, [fromHeader] string locale = "en")
        {
            ApplicationUser user = null;
            Language language = null;

            var enterprise = await new EnterpriseManager().GetTrendigoEnterprise();

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            language = await db.Languages.Where(a => a.ISO639_1 == model.language).FirstOrDefaultAsync();

            if (language == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("LanguageNotFound_", locale));
            }

            user = new ApplicationUser
            {
                UserName = model.email,
                Email = model.email
            };

            AspNetUser aspNetUser = await db.AspNetUsers.FirstOrDefaultAsync(a => a.UserName == model.email);

            if (aspNetUser != null)
            {
                return Content(HttpStatusCode.NotAcceptable, MessageService.GetMessage("EmailAlreadyUsed_", locale));
            }

            #endregion

            IdentityResult result;
            result = await UserManager.CreateAsync(user, model.password);

            if (!result.Succeeded == true)
            {
                logger.ErrorFormat("AddMerchantAdmin - UserId {0}", user.Id);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateUser_", locale));
            }

            //Add role member to new user
            result = UserManager.AddToRole(user.Id, IMSRole.MerchantAdmin.ToString());

            if (!result.Succeeded == true)
            {
                logger.ErrorFormat("AddToRole Not Created - UserId {0}", user.Id);
                UserManager.Delete(user);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateUser_", locale));
            }

            IMS.Common.Core.Data.IMSUser newUser = new Common.Core.Data.IMSUser();
            newUser.FirstName = model.firstName;
            newUser.LastName = model.lastName;
            newUser.AspNetUser = new AspNetUser();
            newUser.AspNetUser.Email = model.email;
            newUser.LanguageId = language.Id;
            newUser.UserId = user.Id;
            newUser.EnterpriseId = enterprise.Id;

            var cmd = DataCommandFactory.AddIMSUserCommand(newUser, "ADMIN", db);

            var resultCmd = await cmd.Execute();

            if (resultCmd != DataCommandResult.Success)
            {
                logger.ErrorFormat("AddIMSUserCommand - result {0}", resultCmd.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotCreateUser_", locale));
            }

            #region Email Validation Section

            try
            {
                var emailValidationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                var callbackUrl = String.Format(ConfigurationManager.AppSettings["IMS.Service.WebAPI.User.EmailValidation.CallbackUrl"], newUser.Id, HttpUtility.UrlEncode(emailValidationToken), model.language);

                var subject = Messages.ResourceManager.GetString("CourrielEmailValidationSubject_" + model.language);
                var textLink = Messages.ResourceManager.GetString("CourrielEmailValidationTextLink_" + model.language);

                await new EmailService().SendConfirmEmailAddressEmail(newUser, model.email, subject, textLink, callbackUrl);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("SendWelcomeEmail - UserId {0} Exception {1} Inner Exception {2}", newUser.Id, ex.ToString(), ex.InnerException.ToString());
            }

            #endregion

            return Content(HttpStatusCode.OK, MessageService.GetMessage("UserAddedSuccessfully_", locale));
        }

        [HttpPost]
        [JwtAuthentication]
        [Route("addMerchant")]
        [SwaggerResponse(HttpStatusCode.OK, "Success", typeof(OnboardingMerchantRS))]
        public async Task<IHttpActionResult> AddMerchant([FromBody] OnboardingMerchantRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Merchant merchant = new Merchant();
            Location location = new Location();
            IMSUser imsUser = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            imsUser = await db.IMSUsers.Where(a => a.Id == model.merchantAdminId).FirstOrDefaultAsync();

            if (imsUser == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("UserNotFound_", locale));
            }

            #endregion

            merchant.Name = model.name;
            merchant.TaxableProduct = true;
            merchant.IsActive = true;
            merchant.CreationDate = DateTime.Now;
            merchant.IMSUsers.Add(imsUser);
            merchant.Status = MerchantStatus.PENDING.ToString();

            var enterprise = await new EnterpriseManager().GetTrendigoEnterprise();

            var command = DataCommandFactory.AddMerchantCommand(merchant, enterprise.TransaxId, db);

            var result = await command.Execute();

            if (result != DataCommandResult.Success)
            {
                logger.ErrorFormat(string.Format("AddMerchantCommand - result {0} merchant {1} merchantAdminId {2}", result, model.name, model.merchantAdminId));
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddMerchant_", locale));
            }

            #region Processor Section

            //ProcessorDTO processor = await new ProcessorManager().GetProcessor((int)ProcessorEnum.GlobalPayment, merchant.Id);
            var processor =  await db.Processors.FirstOrDefaultAsync(a => a.Id == (int)ProcessorEnum.GlobalPayment);
            MerchantProcessor merchantProcessor = new MerchantProcessor();
            merchantProcessor.MerchantId = merchant.Id;
            merchantProcessor.ProcessorId = processor.TransaxId;
            merchantProcessor.IsActive = true;
            merchantProcessor.CreationDate = DateTime.Now;
            merchantProcessor.ModificationDate = DateTime.Now;
            merchantProcessor.Merchant = merchant;
            merchantProcessor.Processor = processor;
            var command2 = DataCommandFactory.AddMerchantProcessorCommand(merchantProcessor, db);

            var result2 = await command2.Execute();

            if (result2 != DataCommandResult.Success)
            {
                logger.ErrorFormat(string.Format("AddMerchantProcessorCommand - result {0} merchantId {1} ProcessorId {2}", result, merchant.Id, processor.TransaxId));
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddMerchant_", locale));
            }

            #endregion

            imsUser.Merchants.Add(merchant);

            var userCommand = DataCommandFactory.UpdateIMSUserCommand(imsUser, "ADMIN", db);

            var userResult = await userCommand.Execute();

            if (userResult != DataCommandResult.Success)
            {
                logger.ErrorFormat(string.Format("UpdateUserCommand - result {0} merchantAdminId {1}", userResult, model.merchantAdminId));
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddMerchant_", locale));
            }

            location.Merchant = merchant;
            location.Name = model.city;
            location.ApplyTaxes = true;
            location.EnableTips = true;
            location.PayWithPoints = true;
            location.Telephone = model.phone;
            location.Address = new Address();
            location.Address.StreetAddress = model.streetAddress;
            location.Address.City = model.city;
            location.Address.StateId = model.stateId;
            location.Address.State = await db.States.Where(a => a.Id == model.stateId).FirstOrDefaultAsync();
            location.Address.CountryId = model.countryId;
            location.Address.Country = await db.Countries.Where(a => a.Id == model.countryId).FirstOrDefaultAsync();
            location.Address.Zip = model.zip;

            #region Geolocation Section

            Geocoding.Address geoLocation = new GeolocationManager().GetGeoLocation(location.Address.StreetAddress, location.Address.City, location.Address.State.Name, location.Address.Country.Name);

            if (geoLocation != null && geoLocation.Coordinates != null)
            {
                location.Address.Longitude = Convert.ToDecimal(geoLocation.Coordinates.Longitude);
                location.Address.Latitude = Convert.ToDecimal(geoLocation.Coordinates.Latitude);
            }

            #endregion

            string timeZoneName = new UtilityManager().getTimeZoneInfoForEntity(location, db).StandardName;
            location.TimeZone = db.TimeZones.Where(a => a.Value == timeZoneName).FirstOrDefault();

            var locationCommand = DataCommandFactory.AddLocationCommand(location, db);

            var locationResult = await locationCommand.Execute();

            if (locationResult != DataCommandResult.Success)
            {
                if (locationResult == DataCommandResult.IMSFailed)
                {
                    var commandDelete = DataCommandFactory.DeleteMerchantCommand(merchant, merchant.TransaxId, db);
                    var resultDelete = await commandDelete.Execute();
                }

                logger.ErrorFormat(string.Format("AddLocationCommand - result {0} merchant {1} merchantAdminId {2}", locationResult, model.name, model.merchantAdminId));
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddMerchant_", locale));
            }

            Merchant merchantToReturn = await db.Merchants.Where(a => a.Id == merchant.Id).FirstOrDefaultAsync();

            OnboardingMerchantRS merchantRS = new OnboardingMerchantRS();
            merchantRS.merchantId = merchantToReturn.Id;

            return Content(HttpStatusCode.OK, merchantRS);
        }

        [HttpPost]
        [Route("{merchantId:long}/addCommunity")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddCommunity(long merchantId, [FromBody] AddCommunityRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Merchant merchant = null;
            Program program = null;

            var enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null || merchant.Locations == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            if (merchant.ProgramId != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantAlreadyInCommunity_", locale));
            }

            Program programExist = await db.Programs.Where(a => a.IsActive == true && a.Description == model.name).FirstOrDefaultAsync();

            if (programExist != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("CommunityAlreadyExist_", locale));
            }

            #endregion

            #region Program Section

            program = new Program();

            program.Description = model.name;
            program.ProgramTypeId = (int)ProgramTypeEnum.Local;
            program.CreationDate = DateTime.Now;
            program.LoyaltyCostUsingPoints = Convert.ToInt32(ConfigurationManager.AppSettings["IMS.Default.Program.LoyaltyCostUsingPoints"]);
            program.LoyaltyValueGainingPoints = Convert.ToInt32(ConfigurationManager.AppSettings["IMS.Default.Program.LoyaltyValueGainingPoints"]);
            program.FidelityRewardPercent = Convert.ToInt32(ConfigurationManager.AppSettings["IMS.Default.Program.FidelityRewardPercent"]);
            program.IsActive = true;
            program.CardTypeId = (int)IMSCardType.MembershipCard;
            program.EnterpriseId = enterprise.Id;
            program.Enterprise = enterprise;
            program.Merchants.Add(merchant);

            try
            {
                var cmd = DataCommandFactory.AddProgramCommand(program, db);
                var result = await cmd.Execute();

                if (result != DataCommandResult.Success)
                {

                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddCommunity_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("AddCommunity - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddCommunity_", locale));
            }

            #endregion

            #region MerchantMember Section

            try
            {
                await CreateMerchantMember(merchantId, locale);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("AddCommunity - CreateMerchantMember - MerchantId {0} Exception {1}", merchant.Id, ex.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddCommunity_", locale));
            }

            #endregion

            return Content(HttpStatusCode.OK, MessageService.GetMessage("CommunityAddedSuccessfully_", locale));
        }

        [HttpPost]
        [Route("{merchantId:long}/joinCommunity/{communityId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> JoinCommunity(long merchantId, long communityId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Merchant merchant = null;
            Program program = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            if (merchant.ProgramId != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantAlreadyInCommunity_", locale));
            }

            program = await db.Programs.Where(a => a.Id == communityId).FirstOrDefaultAsync();

            if (program == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("CommunityNotFound_", locale));
            }

            #endregion

            merchant.ProgramId = communityId;
            merchant.Program = program;

            var enterprise = await new EnterpriseManager().GetTrendigoEnterprise();

            try
            {
                var cmd = DataCommandFactory.UpdateMerchantCommand(merchant, enterprise.TransaxId, db);
                var result = await cmd.Execute();

                if (result != DataCommandResult.Success)
                {

                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToJoinCommunity_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdateMerchantCommand (JoinCommunity) - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToJoinCommunity_", locale));
            }

            if (program.ProgramTypeId == (int)ProgramTypeEnum.Local || program.ProgramTypeId == (int)ProgramTypeEnum.Personnal)
            {
                try
                {
                    await CreateMerchantMember(merchant.Id, locale);
                }
                catch(Exception ex)
                {
                    logger.ErrorFormat("JoinCommunity - CreateMerchantMember - MerchantId {0} Exception {1}", merchant.Id, ex.ToString());
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToJoinCommunity_", locale));
                }
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("CommunityJoinSuccessfully_", locale));
        }

        [HttpPost]
        [JwtAuthentication]
        [Route("{merchantId:long}/addMerchantInformation")]
        public async Task<IHttpActionResult> AddMerchantInformation(long merchantId, [FromBody] OnboardingMerchantInformationRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Merchant merchant = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            merchant.Locations.FirstOrDefault(a => a.IsActive == true).EnableTips = model.acceptTips;

            #region Locale Section

            if (model.locales != null)
            {
                foreach(OnboardingMerchantLocaleRQ loc in model.locales)
                {
                    merchant_translations trans = new merchant_translations();
                    trans.locale = loc.language;
                    trans.ShortDescription = loc.description;
                    trans.created_at = DateTime.Now;
                    trans.updated_at = DateTime.Now;
                    trans.Name = merchant.Name;
                    merchant.merchant_translations.Add(trans);
                }
            }

            #endregion

            #region Categories and Tags
            await new TagService().AddMerchantCategory(merchantId, model.categoryId);
            MerchantTag mt = new MerchantTag();
            mt.TagId = model.categoryId;
            merchant.MerchantTags.Add(mt);

            if (model.tags != null)
            {
                foreach(OnboardingMerchantTagRQ tag in model.tags)
                {
                    MerchantTag t = new MerchantTag();
                    t.TagId = tag.tagId;
                    merchant.MerchantTags.Add(t);
                }
            }

            #endregion

            try
            {
                db.Entry(merchant).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("AddMerchantInformation - Unable to add translations or tags MerchantId {0} Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddMerchantInformation_", locale));
            }

            #region Reward Section

            Promotion promotion = new Promotion();
            promotion.IsActive = true;
            promotion.Locations.Add(merchant.Locations.FirstOrDefault(a => a.IsActive == true));
            promotion.ModificationDate = DateTime.Now;
            promotion.PromotionTypeId = (int)IMSPromotionType.BONIFICATION;
            promotion.Title = merchant.Name;
            promotion.Value = Convert.ToDouble(model.baseReward) / 100;

            var commandReward = DataCommandFactory.AddPromotionCommand(promotion, db);

            var resultReward = await commandReward.Execute();

            if (resultReward != DataCommandResult.Success)
            {
                logger.ErrorFormat("AddMerchantInformation - Unable to add base reward MerchantId {0} Result {1} ", merchantId, resultReward.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddBaseReward_", locale));
            }

            // Start date for the promotion calendar is DateTime Now + 1 day
            DateTime startDate = DateTime.Now;
            // End date for the promotion calendar is if date is higher than 15, we expend the end date thru the end of the next month otherwise we stick to the actual month
            DateTime targetEndDate = startDate.Day > 15 ? DateTime.Now.AddMonths(1) : startDate;
            DateTime endDate = new DateTime(targetEndDate.Year, targetEndDate.Month, DateTime.DaysInMonth(targetEndDate.Year, targetEndDate.Month));

            Promotion addedPromotion = await db.Promotions.Where(a => a.Id == promotion.Id).FirstOrDefaultAsync();

            try
            {
                await new PromotionManager().CreateBasePromotionSchedule(addedPromotion, addedPromotion.Value, startDate, endDate, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdateMerchant - CreateBasePromotionSchedule MerchantId {0} PromotionId {1} Exception {2} InnerException {3}", merchantId, promotion.Id, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddBaseRewardCalendar_", locale));
            }

            #endregion

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyAdded_", locale));
        }

        [HttpPost]
        [JwtAuthentication]
        [Route("{merchantId:long}/addOperationalHours")]
        public async Task<IHttpActionResult> AddBusinessHourInformation(long merchantId, [FromBody] OnboardingBusinessHoursAndHolidaysRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            var enterprise = await new EnterpriseManager().GetTrendigoEnterprise();

            LocationHoliday holiday = null;
            LocationHoliday holidayExist = null;
            DateTime from;
            DateTime to;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            #region Business hours Section

            foreach (OnboardingBusinessHourRQ bhRQ in model.hours)
            {
                LocationBusinessHour businessHour = new LocationBusinessHour();

                businessHour.LocationId = merchant.Locations.FirstOrDefault().Id;
                businessHour.DayOfWeekID = bhRQ.dayOfWeek;
                businessHour.OpeningHour = TimeSpan.Parse(bhRQ.openingHour);
                businessHour.ClosingHour = TimeSpan.Parse(bhRQ.closingHour);
                businessHour.IsClosed = bhRQ.isClosed;

                merchant.Locations.FirstOrDefault(a => a.IsActive == true).LocationBusinessHours.Add(businessHour);
            }

            try
            {
                db.Entry(merchant).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("AddBusinessHour - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddBusinessHour_", locale));
            }

            #endregion

            #region Holidays Section

            foreach (OnboardingHolidayRQ hRQ in model.holidays)
            {
                if (!DateTime.TryParse(hRQ.startDate, out from))
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("StartingDateNotAValidDate_", locale));
                }

                if (!DateTime.TryParse(hRQ.endDate, out to))
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("EndingDateNotAValidDate_", locale));
                }

                holidayExist = merchant.Locations.FirstOrDefault(a => a.IsActive == true).LocationHolidays.FirstOrDefault(a => ((a.FromDate <= from && a.ToDate >= from) || (a.FromDate <= to && a.ToDate >= to)) && a.IsActive == true);

                if (holidayExist == null)
                {
                    holiday = new LocationHoliday();
                    holiday.Name = hRQ.name;
                    holiday.FromDate = from;
                    holiday.ToDate = to;
                    holiday.CreationDate = DateTime.Now;
                    holiday.ModificationDate = DateTime.Now;
                    holiday.IsActive = true;

                    merchant.Locations.FirstOrDefault(a => a.IsActive == true).LocationHolidays.Add(holiday);
                }
            }

            try
            {
                db.Entry(merchant).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("AddHoliday - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddHoliday_", locale));
            }

            #endregion

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyAdded_", locale));
        }

        [HttpPost]
        [Route("{merchantId:long}/addBankingInformation")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddBankingInformation(long merchantId, [FromBody] addBankAccountRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section


            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            Merchant merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            if (merchant.Locations.FirstOrDefault().BankingInfoId != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("BankingInfoExist_", locale));
            }

            #endregion

            BankingInfo bankingInfo = new BankingInfo();

            bankingInfo = Mapper.Map<addBankAccountRQ, BankingInfo>(model);

            bankingInfo.Locations.Add(merchant.Locations.FirstOrDefault(a => a.IsActive == true));
            bankingInfo.IsActive = true;
            bankingInfo.CreationDate = DateTime.Now;
            bankingInfo.ModificationDate = DateTime.Now;

            db.BankingInfos.Add(bankingInfo);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("AddBankingInfo - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddBankingInfo_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyAdded_", locale));
        }

        #region Private Methods

        private async Task<Boolean> CreateMerchantMember(long merchantId, string locale)
        {
            Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();
            Merchant merchant = await db.Merchants.FirstOrDefaultAsync(a => a.Id == merchantId);

            #region MerchantAdmin Member Section

            // This section is for the creation of a member that is related to the merchant because
            // we need to be able to process monthly subscription fees to the merchant and to be able
            // to process those transaction without changing the payment API, we decided to create a member for that merchant.

            #region Member Section

            IMSUser merchantAdminUser = null;

            ApplicationUser user = null;
            try
            {
                merchantAdminUser = new IMSUserManager().GetMerchantAdminUserInfo(merchantId);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("CreateMerchantMember - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                throw new Exception(MessageService.GetMessage("UnableToAddCommunity_", locale));
            }

            MemberDTO merchantMember = new MemberDTO();
            merchantMember.email = string.Concat(merchantAdminUser.AspNetUser.Email, ".", Guid.NewGuid().ToString());
            merchantMember.passwordNotSet = false;
            merchantMember.password = "Tr3nd1g0";
            merchantMember.language = merchantAdminUser.Language.ISO639_1;
            merchantMember.firstName = merchantAdminUser.FirstName;
            merchantMember.lastName = merchantAdminUser.LastName;

            try
            {
                user = new ApplicationUser
                {
                    UserName = merchantMember.email,
                    Email = merchantMember.email,
                    EmailConfirmed = true
                };

                AspNetUser exist = await db.AspNetUsers.FirstOrDefaultAsync(a => a.UserName == merchantMember.email);

                if (exist != null)
                {
                    throw new Exception(MessageService.GetMessage("EmailAlreadyUsed_", locale));
                }

                IdentityResult result;
                //Create AspNetUser
                result = await UserManager.CreateAsync(user, merchantMember.password);

                if (!result.Succeeded == true)
                {
                    logger.ErrorFormat("CreateASync - UserId {0}", user.Id);
                    throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
                }

                //Add role member to new user
                result = UserManager.AddToRole(user.Id, IMSRole.Member.ToString());

                if (!result.Succeeded == true)
                {
                    logger.ErrorFormat("AddToRole Not Created - UserId {0}", user.Id);
                    UserManager.Delete(user);
                    throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
                }
            }
            catch (Exception ex)
            {
                // Rollback identity user creation
                try
                {
                    UserManager.Delete(user);
                }
                catch { }

                logger.ErrorFormat("Create AspNetUser - Exception {0} InnerException {1} MerchantMember info {2}", ex.ToString(), ex.InnerException.ToString(), merchantMember.email);

                throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
            }

            Member member = Mapper.Map<MemberDTO, Common.Core.Data.Member>(merchantMember);

            member.Language = await db.Languages.FirstOrDefaultAsync(x => x.ISO639_1 == merchantMember.language);
            member.UserId = user.Id;
            member.AspNetUser = await db.AspNetUsers.Where(a => a.Id == user.Id).FirstOrDefaultAsync();
            member.EnterpriseId = enterprise.Id;

            //Add new member
            try
            {
                var registerMemberCmd = DataCommandFactory.AddMemberCommand(member, merchantMember.email, merchantMember.password, db);

                var cmdResult = await registerMemberCmd.Execute();

                if (cmdResult != DataCommandResult.Success)
                {
                    logger.ErrorFormat("AddCommunity - RegisterMemberCmd - Result {0} MerchantMember info {1}", cmdResult.ToString(), merchantMember.email);
                    throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
                }
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    //Delete AspNetUserRole
                    IdentityResult deleteRole_result = UserManager.RemoveFromRole(user.Id, IMSRole.Member.ToString());
                    IHttpActionResult deleteRole_errorResult = GetErrorResult(deleteRole_result);

                    if (deleteRole_errorResult != null)
                    {
                        logger.ErrorFormat("CreateMerchantMember - Member Delete Role - Result {0} MerchantMember info {1}", deleteRole_errorResult.ToString(), merchantMember.email);
                    }

                    //Delete AspNetUser
                    IdentityResult delete_result = UserManager.Delete(user);
                    IHttpActionResult delete_errorResult = GetErrorResult(delete_result);

                    if (delete_errorResult != null)
                    {
                        logger.ErrorFormat("CreateMerchantMember - User Manager Delete - Result {0} MerchantMember info {1}", deleteRole_errorResult.ToString(), merchantMember.email);
                    }
                }

                logger.ErrorFormat("CreateMerchantMember - Add Member Command - Exception {0} InnerException {1} MerchantMember info {2}", ex.ToString(), ex.InnerException.ToString(), merchantMember.email);
                throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            #region Notification Section

            IMS.Utilities.PaymentAPI.Model.Notification notification = new IMS.Utilities.PaymentAPI.Model.Notification();
            notification.DeviceId = merchantMember.email;
            notification.NotificationToken = "";

            try
            {
                IMS.Utilities.PaymentAPI.Model.EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().AssociateNotificationToMember(Convert.ToInt32(member.TransaxId), notification);
            }
            catch (IMS.Utilities.PaymentAPI.Client.ApiException ex)
            {
                logger.ErrorFormat("CreateMerchantMember - AssociateNotificationToMember ErrorCode {0} Message {1} Exception {2}", ex.ErrorCode, "Error calling CreateMember: " + ex.ErrorContent, ex.InnerException);
                throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            #region Membership Section

            Program billingProgram = null;

            try
            {
                billingProgram = await new ProgramManager().GetBillingProgram(enterprise.Id, enterprise.Address.Country.Currency.Id, merchant.Id, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("AddCommunity - GetBillingProgram - EntepriseId {0} CurrencyId {1} MerchantId {2} Exception {3}", enterprise.Id, enterprise.Address.Country.CurrencyId, merchant.Id, ex.ToString());
                throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
            }

            if (billingProgram == null)
            {
                logger.ErrorFormat("AddCommunity - GetBillingProgram - Not Found - EnterpriseId {0} CurrencyId {1} MerchantId {2}", enterprise.Id, enterprise.Address.Country.CurrencyId, merchant.Id);
                throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
            }

            try
            {
                IMSMembership membership = new IMSMembership();
                membership.ProgramID = billingProgram.Id;
                membership.Program = billingProgram;
                membership.CreationDate = DateTime.Now;
                membership.IsActive = false;
                membership.MemberID = member.Id;
                membership.Member = member;
                membership.NoShipping = true;

                var registerMembershipCmd = DataCommandFactory.AddMembershipCommand(membership, member.Id.ToString(), 0, db);

                var membershipResult = await registerMembershipCmd.Execute();

                if (membershipResult != DataCommandResult.Success)
                {
                    var deleteCmd = DataCommandFactory.DeleteMemberCommand(member, db);

                    var deleteResult = await deleteCmd.Execute();

                    logger.ErrorFormat("AddCommunity - AddMembershipCommand -  Result {0} Member info {1}", membershipResult.ToString(), member.Id.ToString());
                    throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
                }
            }
            catch (Exception ex)
            {
                //Delete Member
                if (member != null)
                {
                    var deleteMemberCmd = DataCommandFactory.DeleteMemberCommand(member, db);
                    var transaxResult = await deleteMemberCmd.Execute();

                    if (transaxResult != DataCommandResult.Success)
                    {
                        logger.ErrorFormat("CreateMerchantMember - DeleteMemberCommand - Result {0}, MerchantMember info {1}", transaxResult.ToString(), merchantMember.email);
                    }
                }

                if (user != null)
                {
                    //Delete AspNetUserRole
                    IdentityResult deleteRole_result = UserManager.RemoveFromRole(user.Id, IMSRole.Member.ToString());
                    IHttpActionResult deleteRole_errorResult = GetErrorResult(deleteRole_result);

                    if (deleteRole_errorResult != null)
                    {
                        logger.ErrorFormat("CreateMerchantMember - UserManager.RemoveFormRole - Result {0}, MerchantMember info {1}", deleteRole_errorResult.ToString(), merchantMember.email);
                    }

                    //Delete AspNetUser
                    IdentityResult delete_result = UserManager.Delete(user);
                    IHttpActionResult delete_errorResult = GetErrorResult(delete_result);

                    if (delete_errorResult != null)
                    {
                        logger.ErrorFormat("CreateMerchantMember - UserManager.Delete - Result {0}, MerchantMember info {1}", deleteRole_errorResult.ToString(), merchantMember.email);
                    }
                }

                throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            #region Subscription Section

            Subscription subscription = new Subscription();
            subscription.MemberId = member.Id;
            subscription.MerchantId = merchant.Id;
            subscription.LocationId = merchant.Locations.FirstOrDefault(a => a.IsActive == true).Id;
            subscription.ProgramId = billingProgram.Id;
            subscription.Status = SubscriptionStatusEnum.PENDING.ToString();
            subscription.BillingAmount = billingProgram.ProgramFees.FirstOrDefault(a => a.CurrencyId == merchant.Locations.FirstOrDefault(b => b.IsActive == true).Address.Country.CurrencyId).AssociatedFees;
            subscription.CreationDate = DateTime.Now;
            subscription.ModificationDate = DateTime.Now;
            subscription.BillingPeriodId = (int)BillingPeriodEnum.Monthly;

            try
            {
                db.Entry(subscription).State = EntityState.Added;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("AddCommunity - AddSubscription - merchantId {0}, Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException.ToString());
                throw new Exception(MessageService.GetMessage("CannotCreateMember_", locale));
            }

            #endregion

            #endregion

            return true;

        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // Aucune erreur ModelState à envoyer, simple retour d'un BadRequest vide.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion

    }
}