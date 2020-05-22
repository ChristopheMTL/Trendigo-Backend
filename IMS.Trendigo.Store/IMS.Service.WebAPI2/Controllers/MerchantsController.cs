using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
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
using IMS.Utilities.PaymentAPI.Client;
using IMS.Utilities.PaymentAPI.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("merchants")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class MerchantsController : ApiController
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

        public MerchantsController()
        {
        }

        public MerchantsController(ApplicationUserManager userManager,
                    ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        #endregion

        #region Merchant Section

        [HttpPost]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddMerchant([FromBody] MerchantRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = new Common.Core.Data.Merchant();
            Common.Core.Data.Location location = new Common.Core.Data.Location();
            IMSUser imsUser = null;
            MerchantRS merchantRS = null;
            MerchantLocationRS locationRS = null;

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
            merchant.ShortDescription = model.shortDesc;
            //TODO need to revisit that parameter
            merchant.TaxableProduct = true;
            merchant.IsActive = true;
            merchant.CreationDate = DateTime.Now;
            merchant.IMSUsers.Add(imsUser);
            merchant.Status = MerchantStatus.PENDING.ToString();

            Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();

            var command = DataCommandFactory.AddMerchantCommand(merchant, enterprise.TransaxId, db);

            var result = await command.Execute();

            if (result != DataCommandResult.Success)
            {
                logger.ErrorFormat(string.Format("AddMerchantCommand - result {0} merchant {1} merchantAdminId {2}", result, model.name, model.merchantAdminId));
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddMerchant_", locale));
            }

            imsUser.Merchants.Add(merchant);

            var userCommand = DataCommandFactory.UpdateIMSUserCommand(imsUser, "ADMIN", db);

            var userResult = await command.Execute();

            if (result != DataCommandResult.Success)
            {
                logger.ErrorFormat(string.Format("UpdateUserCommand - result {0} merchantAdminId {1}", result, model.merchantAdminId));
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
            location.Address.Longitude = Convert.ToDecimal(model.longitude);
            location.Address.Latitude = Convert.ToDecimal(model.latitude);

            string timeZoneName = new UtilityManager().getTimeZoneInfoForEntity(location, db).StandardName;
            location.TimeZone = db.TimeZones.Where(a => a.Value == timeZoneName).FirstOrDefault();

            var command2 = DataCommandFactory.AddLocationCommand(location, db);

            var result2 = await command2.Execute();

            if (result2 != DataCommandResult.Success)
            {
                if (result == DataCommandResult.IMSFailed)
                {
                    var commandDelete = DataCommandFactory.DeleteMerchantCommand(merchant, merchant.TransaxId, db);
                    var resultDelete = await commandDelete.Execute();
                }

                logger.ErrorFormat(string.Format("AddLocationCommand - result {0} merchant {1} merchantAdminId {2}", result, model.name, model.merchantAdminId));
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddMerchant_", locale));
            }

            Common.Core.Data.Merchant merchantToReturn = await db.Merchants.Where(a => a.Id == merchant.Id).FirstOrDefaultAsync();
            merchantRS = Mapper.Map<Common.Core.Data.Merchant, MerchantRS>(merchantToReturn);

            merchantRS.locations.Add(locationRS);

            return Content(HttpStatusCode.OK, merchantRS);
        }

        [HttpGet]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMerchants([FromBody] getMerchantRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            List<Common.Core.Data.Location> locations = new List<Common.Core.Data.Location>();
            List<MerchantRS> merchantsRS = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            #endregion

            var coord = new GeoCoordinate(Convert.ToDouble(model.latitude), Convert.ToDouble(model.longitude));

            if (coord == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("NoLocalisationFound_", locale));
            }

            var addresses = await db.Addresses
                .Where(a => a.Locations.Any(b => b.Merchant.IsActive == true) && 
                 a.Locations.Any(c => c.Merchant.Status == MerchantStatus.ACTIVE.ToString() || 
                 c.Merchant.Status == MerchantStatus.PENDING.ToString()))
                .Select(x => new GeoCoordinate(Convert.ToDouble(x.Latitude), Convert.ToDouble(x.Longitude)))
                .OrderBy(x => x.GetDistanceTo(coord))
                .Take(model.limit).ToListAsync();

            if (addresses.Any())
            {
                locations = db.Locations
                 .Where(x => addresses.Any(s => x.AddressId.ToString().Equals(s)))
                 .ToList();
            }

            List<Common.Core.Data.Merchant> merchants = locations.Select(a => a.Merchant).ToList();

            merchantsRS = Mapper.Map<List<Common.Core.Data.Merchant>, List<MerchantRS>>(merchants);

            return Content(HttpStatusCode.OK, merchantsRS);
        }

        [HttpGet]
        [Route("{merchantId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMerchant(long merchantId, [fromHeader] string locale = "en")
        {

            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            MerchantRS merchantRS = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            merchantRS = Mapper.Map<Common.Core.Data.Merchant, MerchantRS>(merchant);

            return Content(HttpStatusCode.OK, merchantRS);
        }

        [HttpPut]
        [Route("{merchantId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateMerchant(long merchantId, [FromBody] UpdateMerchantRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();

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

            #region Merchant Update Section

            merchant.Name = model.name;
            merchant.LogoPath = model.logo;
            merchant.TaxableProduct = true;
            merchant.ModificationDate = DateTime.Now;

            var command = DataCommandFactory.UpdateMerchantCommand(merchant, enterprise.TransaxId, db);

            var result = await command.Execute();

            if (result != DataCommandResult.Success)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateMerchant_", locale));
            }

            #endregion

            #region Location Section

            foreach (UpdateMerchantLocationRQ loc in model.locations)
            {
                Common.Core.Data.Location location = await db.Locations.Where(a => a.MerchantId == merchantId && a.IsActive == true).FirstOrDefaultAsync();

                location.Address.StreetAddress = loc.streetAddress;
                location.Address.City = loc.city;
                location.Address.StateId = loc.stateId;
                location.Address.CountryId = loc.countryId;
                location.Address.Zip = loc.zip;
                location.Telephone = loc.phone;

                var commandLocation = DataCommandFactory.UpdateLocationCommand(location, db);

                var resultLocation = await commandLocation.Execute();

                if (resultLocation != DataCommandResult.Success)
                {
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateMerchant_", locale));
                }
            }

            #endregion

            #region Merchant Translation Section

            if (model.merchantLocale != null)
            {
                foreach (UpdateMerchantLocaleRQ merchantLocale in model.merchantLocale)
                {
                    if (merchantLocale.merchantLocaleId == null)
                    {
                        merchant_translations mtExist = await db.merchant_translations.Where(a => a.merchant_id == merchantId && a.locale == merchantLocale.language).FirstOrDefaultAsync();

                        if (mtExist == null)
                        {
                            merchant_translations mt = new merchant_translations();
                            mt.Name = model.name;
                            mt.ShortDescription = merchantLocale.description;
                            mt.locale = merchantLocale.language;
                            mt.created_at = DateTime.Now;
                            mt.updated_at = DateTime.Now;
                            mt.merchant_id = merchant.Id;
                            db.Entry(mt).State = EntityState.Added;
                            try
                            {
                                await db.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                logger.ErrorFormat("Add Merchant Locale - MerchantId {0} MerchantLocale {1} Exception {2} InnerException {3}", merchantId, mt.ToString(), ex.ToString(), ex.InnerException.ToString());
                                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateMerchant_", locale));
                            }
                        }
                    }
                    else
                    {
                        merchant_translations mt = await db.merchant_translations.Where(a => a.id == merchantLocale.merchantLocaleId).FirstOrDefaultAsync();

                        if (mt == null)
                        {
                            logger.ErrorFormat("UpdateMerchant - Merchant_Translation does not exist for merchantId {0} merchantLocaleId {1}",merchantId,  merchantLocale.merchantLocaleId);
                            return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UnableToUpdateMerchant_", locale));
                        }

                        mt.ShortDescription = merchantLocale.description;

                        try
                        {
                            db.Entry(mt).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("UpdateMerchant Merchant_Translation SaveChangesAsync failed MerchantId {0} MerchantTranslationId {1} Exception {2} InnerException {3}", merchantId, mt.id, ex.ToString(), ex.InnerException.ToString());
                            return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateMerchant_", locale));
                        }
                    }
                }
            }

            #endregion

            #region Image Section

            if (model.images != null)
            {
                foreach (UpdateMerchantImagesRQ img in model.images)
                {
                    if (img.imageId.HasValue)
                    {
                        MerchantImage imgExist = await db.MerchantImages.Where(a => a.Id == img.imageId.Value).FirstOrDefaultAsync();

                        if (imgExist == null)
                        {
                            return Content(HttpStatusCode.NotFound, MessageService.GetMessage("ImageNotFound_", locale));
                        }

                        imgExist.ImagePath = img.path;

                        try
                        {
                            db.Entry(imgExist).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Update Image - ImageId {0} Exception {1} InnerException {2}", imgExist.Id, ex.ToString(), ex.InnerException.ToString());
                            return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UnableToUpdateImage_", locale));
                        }
                    }
                    else
                    {
                        if (merchant.MerchantImages.Count() == 5)
                        {
                            return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("CannotAddNewImageQuotaReached_", locale));
                        }

                        MerchantImage mi = new MerchantImage();
                        mi.MerchantId = merchantId;
                        mi.CreationDate = DateTime.Now;
                        mi.ImagePath = img.path;
                        mi.IsActive = true;
                        mi.IsPublished = true;
                        mi.Weight = Convert.ToInt16(merchant.MerchantImages.Count() + 1);

                        try
                        {
                            db.Entry(mi).State = EntityState.Added;
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("CannotAddNewImage_", locale));
                        }
                    }
                }
            }

            #endregion

            #region Category Section

            if (model.categoryId.HasValue)
            {
                tagging tagExist = await new TagService().GetMerchantCategory(merchantId);

                if (tagExist != null && tagExist.tag_id != model.categoryId.Value)
                {
                    try
                    {
                        await new TagService().UpdateMerchantCategory(merchantId, model.categoryId.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("UpdateMerchant - Unable to update category MerchantId {0} Exception {1} InnerException {2} ", merchantId, ex.ToString(), ex.InnerException.ToString());
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateCategory_", locale));
                    }
                }
                else if (tagExist == null)
                {
                    try
                    {
                        await new TagService().AddMerchantCategory(merchantId, model.categoryId.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("UpdateMerchant - Unable to add category MerchantId {0} Exception {1} InnerException {2} ", merchantId, ex.ToString(), ex.InnerException.ToString());
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddCategory_", locale));
                    }
                }
            }

            #endregion

            #region Promotion Section

            if (model.baseReward.HasValue && merchant.Locations.FirstOrDefault().Promotions.Count == 0)
            {
                Common.Core.Data.Promotion promotion = new Common.Core.Data.Promotion();

                promotion.IsActive = true;
                promotion.Locations.Add(merchant.Locations.FirstOrDefault());
                promotion.ModificationDate = DateTime.Now;
                promotion.PromotionTypeId = (int)IMSPromotionType.BONIFICATION;
                promotion.Title = merchant.Name;
                promotion.Value = model.baseReward.Value / 100;

                var commandReward = DataCommandFactory.AddPromotionCommand(promotion, db);

                var resultReward = await commandReward.Execute();

                if (resultReward != DataCommandResult.Success)
                {
                    logger.ErrorFormat("UpdateMerchant - Unable to add promotion MerchantId {0} Result {1} ", merchantId, result.ToString());
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddBaseReward_", locale));
                }

                // Start date for the promotion calendar is DateTime Now + 1 day
                DateTime startDate = DateTime.Now.AddDays(1);
                // End date for the promotion calendar is if date is higher than 15, we expend the end date thru the end of the next month otherwise we stick to the actual month
                DateTime targetEndDate = startDate.Day > 15 ? startDate.AddMonths(1) : startDate;
                DateTime endDate = new DateTime(targetEndDate.Year, targetEndDate.Month, DateTime.DaysInMonth(targetEndDate.Year, targetEndDate.Month));

                try
                {
                    await new PromotionManager().CreateBasePromotionSchedule(promotion, promotion.Value, startDate, endDate, db);
                }
                catch(Exception ex)
                {
                    logger.ErrorFormat("UpdateMerchant - CreateBasePromotionSchedule MerchantId {0} PromotionId {1} Exception {2} InnerException {3}", merchantId, promotion.Id, ex.ToString(), ex.InnerException.ToString());
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddBaseRewardCalendar_", locale));
                }
            }

            #endregion

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyUpdated_", locale));
        }

        [HttpDelete]
        [Route("{merchantId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteMerchant(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, "Merchant not found");
            }

            #endregion

            var command = DataCommandFactory.DeleteMerchantCommand(merchant, merchant.TransaxId, db);

            var result = await command.Execute();

            if (result != DataCommandResult.Success)
            {
                logger.ErrorFormat(string.Format("DeleteMerchantCommand - result {0} merchantId {1}", result, merchant.Id));
                return Content(HttpStatusCode.InternalServerError, "Unable to delete merchant");
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("MerchantDeleted_", locale));
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login([FromBody] LoginMerchantRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            TokenDTO token = new TokenDTO();
            IMSUser imsUser = new IMSUser();

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            var _user = await db.AspNetUsers.FirstOrDefaultAsync(a => a.UserName == model.email);
            var user = await UserManager.FindByIdAsync(_user.Id);

            if (user == null)
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("InvalidUsernameOrPassword_", locale));
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("AccountLocked_", locale));
            }

            //if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            //{
            //    return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("EmailNotValidated_", locale));
            //}

            var result = await UserManager.CheckPasswordAsync(user, model.password);

            if (!result)
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("InvalidUsernameOrPassword_", locale));
            }

            if (_user.IMSUsers == null)
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("UserNotFound_", locale));
            }

            if (_user.AspNetRoles.FirstOrDefault().Name == IMSRole.MerchantUser.ToString() && string.IsNullOrEmpty(model.deviceId))
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("UnauthorizedToLogin_", locale));
            }

            #endregion

            #region JWToken Section

            try
            {
                token = await TokenManager.generateToken(model.deviceId);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Login GenerateToken - Email {0} Exception {1} InnerException {2}", model.email, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToLogin_", locale));
            }

            #endregion

            #region PaymentAPI admin section

            IMS.Utilities.PaymentAPI.Model.AuthenticationData TransaxEntity = new IMS.Utilities.PaymentAPI.Model.AuthenticationData();
            TransaxEntity.DeviceId = model.deviceId;
            TransaxEntity.Jti = token.jti;

            try
            {
                EntityId response = await new IMS.Utilities.PaymentAPI.Api.UsersApi().UserLogin(Convert.ToInt32(_user.IMSUsers.FirstOrDefault().TransaxId), TransaxEntity);
            }
            catch (ApiException ex)
            {
                logger.ErrorFormat("Login PaymentAPI - Email {0} Exception {1} InnerException {2}", model.email, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToLogin_", locale));
            }

            #endregion

            LoginMerchantRS loginMerchantRS = new LoginMerchantRS();
            loginMerchantRS.userId = _user.IMSUsers.FirstOrDefault().Id;
            loginMerchantRS.sessionToken = token.signedJwt;

            return Content(HttpStatusCode.OK, loginMerchantRS);
        }

        [HttpPost]
        [Route("{userId:long}/emailValidation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> emailValidation(long userId, [FromBody] EmailValidationRQ model, [fromHeader] string locale = "en")
        {
            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            Common.Core.Data.IMSUser user = await db.IMSUsers.FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UserNotFound_", locale));

            var result = await UserManager.ConfirmEmailAsync(user.AspNetUser.Email, model.code);

            if (!result.Succeeded)
            {
                model.code = HttpUtility.UrlDecode(model.code);

                result = await UserManager.ConfirmEmailAsync(user.UserId, model.code);

                if (!result.Succeeded)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("UnableToValidateEmailAddress_", locale));
                }
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("EmailAddressValidated_", locale));
        }

        [HttpPost]
        [Route("getMerchantByName")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMerchantByName([FromBody] getMerchantByNameRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            List<Common.Core.Data.Location> locations = new List<Common.Core.Data.Location>();
            List<MerchantRS> merchantsRS = new List<MerchantRS>();

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            var coord = new GeoCoordinate(Convert.ToDouble(model.latitude), Convert.ToDouble(model.longitude));

            if (coord == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("NoLocalisationFound_", locale));
            }

            #endregion

            #region Query Section

            List<long> locs = (from l in db.Locations
                               join m in db.Merchants on l.MerchantId equals m.Id
                               where (m.Name.Contains(model.name) && m.Status == MerchantStatus.ACTIVE.ToString())
                               select l.Id).Distinct().ToList();

            List<long> locs2 = (from l in db.Locations
                                join tg in db.taggings on l.MerchantId equals tg.taggable_id
                                join ta in db.tags on tg.tag_id equals ta.id
                                join tt in db.tag_translations on ta.id equals tt.tag_id
                                where (tt.name.Contains(model.name) && ta.ParentId != null && l.Merchant.Status == MerchantStatus.ACTIVE.ToString())
                                select tg.taggable_id.Value).Distinct().ToList();

            var genLocs = locs.Union(locs2).ToList();

            List<Common.Core.Data.Address> addresses = db.Addresses.Where(a => a.Locations.Any(b => genLocs.Contains(b.MerchantId))).ToList()
                                     .Select(x => new { Org = x, LocCoord = new GeoCoordinate((double)x.Latitude, (double)x.Longitude) })
                                     .OrderBy(z => z.LocCoord.GetDistanceTo(coord))
                                     .Select(w => w.Org)
                                     .Skip(model.start)
                                     .Take(model.limit)
                                     .ToList();

            locations = addresses.Select(a => a.Locations.FirstOrDefault()).ToList();
            List<Common.Core.Data.Merchant> merchants = locations.Select(a => a.Merchant).ToList();

            #endregion

            #region Mapping Section

            merchantsRS = Mapper.Map<List<Common.Core.Data.Merchant>, List<MerchantRS>>(merchants);
            
            #endregion

            return Content(HttpStatusCode.OK, merchantsRS);
        }

        [HttpPost]
        [Route("getMerchantByCategory")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMerchantsByCategory([FromBody] getMerchantByCategoryRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            List<Common.Core.Data.Location> locations = new List<Common.Core.Data.Location>();
            List<MerchantRS> merchantsRS = new List<MerchantRS>();

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            #endregion

            #region Query Section

            var coord = new GeoCoordinate(Convert.ToDouble(model.latitude), Convert.ToDouble(model.longitude));

            if (coord == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("NoLocalisationFound_", locale));
            }

            tag categoryTag = await db.tags.FirstOrDefaultAsync(a => a.id == model.categoryId);
            var listOfMerchantIdInCategory = await db.taggings.Where(a => a.tag_id == model.categoryId).Select(b => b.taggable_id).Distinct().ToListAsync();

            List<long> locs = await db.Locations.Where(a => listOfMerchantIdInCategory.Contains(a.MerchantId)).Select(b => b.Id).ToListAsync();

            List<Common.Core.Data.Address> addresses = db.Addresses.Where(a => a.Locations.Any(b => locs.Contains(b.MerchantId) && b.Merchant.Status == MerchantStatus.ACTIVE.ToString())).ToList()
                                     .Select(x => new { Org = x, LocCoord = new GeoCoordinate((double)x.Latitude, (double)x.Longitude) })
                                     .OrderBy(z => z.LocCoord.GetDistanceTo(coord))
                                     .Select(w => w.Org)
                                     .Skip(model.start)
                                     .Take(model.limit)
                                     .ToList();

            locations = addresses.Select(a => a.Locations.FirstOrDefault()).ToList();
            List<Common.Core.Data.Merchant> merchants = locations.Select(a => a.Merchant).ToList();

            #endregion

            #region Mapping

            merchantsRS = Mapper.Map<List<Common.Core.Data.Merchant>, List<MerchantRS>>(merchants);
            
            #endregion

            return Content(HttpStatusCode.OK, merchantsRS);
        }

        [HttpPost]
        [Route("admin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> AddMerchantAdmin([FromBody] addUserRQ model, [fromHeader] string locale = "en")
        {
            ApplicationUser user = null;
            Language language = null;
            Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();

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

            #endregion

            user = new ApplicationUser
            {
                UserName = model.email,
                Email = model.email
            };

            AspNetUser aspNetUser = await db.AspNetUsers.FirstOrDefaultAsync(a => a.UserName == model.email);

            if (aspNetUser != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("EmailAlreadyUsed_", locale));
            }

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

                var callbackUrl = String.Format(ConfigurationManager.AppSettings["IMS.Service.WebAPI.User.EmailValidation.CallbackUrl"], newUser.Id, HttpUtility.UrlEncode(emailValidationToken), locale);

                var subject = Messages.ResourceManager.GetString("CourrielEmailValidationSubject_" + locale);
                var textLink = Messages.ResourceManager.GetString("CourrielEmailValidationTextLink_" + locale);

                await new EmailService().SendConfirmEmailAddressEmail(newUser, model.email, subject, textLink, callbackUrl);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("SendWelcomeEmail - UserId {0} Exception {1} Inner Exception {2}", newUser.Id, ex.ToString(), ex.InnerException.ToString());
            }

            #endregion

            addUserRS userRS = new addUserRS();
            userRS.userId = newUser.Id;
            userRS.firstName = model.firstName;
            userRS.lastName = model.lastName;
            userRS.language = newUser.Language.ISO639_1;
            userRS.email = model.email;

            return Content(HttpStatusCode.OK, userRS);
        }

        [HttpPut]
        [Route("{merchantId:long}/admin/{userId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateMerchantAdmin(long merchantId, long userId, [FromBody] UpdateUserRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            UserDTO userDTO = new UserDTO();
            Common.Core.Data.IMSUser imsUser = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            try
            {
                imsUser = await db.IMSUsers.FirstOrDefaultAsync(a => a.Id == userId && a.IsActive == true);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetMerchantAdmin - UserId {0} Exception {1} InnerException {2}", userId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UserInfoNotAccessible_", locale));
            }

            if (imsUser == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UserNotFound_", locale));
            }

            if (imsUser.Merchants != null && imsUser.Merchants.FirstOrDefault().Id != merchantId)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            imsUser = Mapper.Map<UpdateUserRQ, IMSUser>(model);
            imsUser.AspNetUser.Email = model.email;
            imsUser.AspNetUser.UserName = model.email;

            var cmd = DataCommandFactory.UpdateIMSUserCommand(imsUser, IMSRole.MerchantAdmin.ToString(), db);

            var resultCmd = await cmd.Execute();

            if (resultCmd != DataCommandResult.Success)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateUser_", locale));
            }

            userDTO = Mapper.Map<IMSUser, UserDTO>(imsUser);

            return Content(HttpStatusCode.OK, userDTO);
        }

        [HttpPost]
        [Route("{merchantId:long}/notification/{userId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddUserNotification(long merchantId, long userId, [FromBody] NotificationRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.AspNetUser user = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            user = await db.AspNetUsers.FirstOrDefaultAsync(a => a.IMSUsers.Any(b => b.Id == userId));

            #endregion

            if (user.UserNotifications == null)
            {
                UserNotification notification = new UserNotification();
                notification.DeviceId = model.deviceId;
                notification.AspNetUser = user;
                notification.NotificationToken = model.notificationToken;

                try
                {
                    var addUserCmd = DataCommandFactory.AddUserNotificationCommand(notification, Convert.ToInt32(user.IMSUsers.FirstOrDefault().TransaxId), db);
                    var transaxResult = await addUserCmd.Execute();

                    if (transaxResult != DataCommandResult.Success)
                    {
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddNotification_", locale));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);

                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddNotification_", locale));
                }
            }
            else
            {
                UserNotification notification = user.UserNotifications.FirstOrDefault();
                notification.DeviceId = model.deviceId;
                notification.NotificationToken = model.notificationToken;

                try
                {
                    var updateUserCmd = DataCommandFactory.AddUserNotificationCommand(notification, Convert.ToInt32(user.IMSUsers.FirstOrDefault().TransaxId), db);
                    var transaxResult = await updateUserCmd.Execute();

                    if (transaxResult != DataCommandResult.Success)
                    {
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddNotification_", locale));
                    }

                    //TODO send email for change in deviceId
                }
                catch (Exception ex)
                {
                    logger.Error(ex);

                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddNotification_", locale));
                }
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("NotificationAddedSuccessfully_", locale));
        }

        #endregion

        #region Business Hour Section

        [HttpPost]
        [Route("{merchantId:long}/businessHours")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddBusinessHour(long merchantId, [FromBody] List<AddBusinessHourRQ> models, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<BusinessHourRS> addBusinessHoursRS = new List<BusinessHourRS>();
            BusinessHourRS addBusinessHourRS = null;
            LocationBusinessHour businessHour = new LocationBusinessHour();
            LocationBusinessHour businessHourExist = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || models == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            foreach (AddBusinessHourRQ model in models)
            {
                businessHourExist = merchant.Locations.FirstOrDefault().LocationBusinessHours.FirstOrDefault(a => a.DayOfWeekID == model.dayOfWeek);

                if (businessHourExist != null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("BusinessHourExistForThatDay_", locale));
                }

                businessHour.LocationId = merchant.Locations.FirstOrDefault().Id;
                businessHour.DayOfWeekID = model.dayOfWeek;
                businessHour.OpeningHour = TimeSpan.Parse(model.openingHour);
                businessHour.ClosingHour = TimeSpan.Parse(model.closingHour);

                db.LocationBusinessHours.Add(businessHour);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("AddBusinessHour - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddBusinessHour_", locale));
                }

                addBusinessHourRS = Mapper.Map<LocationBusinessHour, BusinessHourRS>(businessHour);

                addBusinessHoursRS.Add(addBusinessHourRS);
            }

            return Content(HttpStatusCode.OK, addBusinessHoursRS);
        }

        [HttpGet]
        [Route("{merchantId:long}/businessHours")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetBusinessHours(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<BusinessHourRS> getBusinessHoursRS = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            getBusinessHoursRS = Mapper.Map<List<LocationBusinessHour>, List<BusinessHourRS>>(merchant.Locations.FirstOrDefault().LocationBusinessHours.ToList());

            return Content(HttpStatusCode.OK, getBusinessHoursRS);
        }

        [HttpGet]
        [Route("{merchantId:long}/businessHours/{businessHourId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetBusinessHour(long merchantId, long businessHourId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            LocationBusinessHour businessHour = null;
            BusinessHourRS getBusinessHourRS = null;

            #endregion

            #region Validation Section

            businessHour = await db.LocationBusinessHours.Where(a => a.Id == businessHourId).FirstOrDefaultAsync();

            if (businessHour == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("BusinessHourNotFound_", locale));
            }

            if (businessHour.Location.MerchantId != merchantId)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            getBusinessHourRS = Mapper.Map<LocationBusinessHour, BusinessHourRS>(businessHour);

            return Content(HttpStatusCode.OK, getBusinessHourRS);
        }

        [HttpPut]
        [Route("{merchantId:long}/businessHours/{businessHourId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateBusinessHour(long merchantId, long businessHourId, UpdateBusinessHourRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            BusinessHourRS businessHourRS = null;
            LocationBusinessHour businessHour = new LocationBusinessHour();

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

            businessHour = await db.LocationBusinessHours.Where(a => a.Id == businessHourId).FirstOrDefaultAsync(); ;

            if (businessHour == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("BusinessHourNotFound_", locale));
            }

            #endregion

            businessHour.OpeningHour = TimeSpan.Parse(model.openingHour);
            businessHour.ClosingHour = TimeSpan.Parse(model.closingHour);

            try
            {
                db.Entry(businessHour).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdateBusinessHour businessHourId {0} Exception {1} InnerException {2}", businessHourId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateBusinessHour_", locale));
            }

            //businessHourRS = Mapper.Map<LocationBusinessHour, BusinessHourRS>(businessHour);

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyUpdated_", locale));
        }

        [HttpDelete]
        [Route("{merchantId:long}/businessHours/{businessHourId:long}")]
        //[JwtAuthentication]
        public async Task<IHttpActionResult> deleteBusinessHour(long merchantId, long businessHourId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.LocationBusinessHour businessHour = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            businessHour = await db.LocationBusinessHours.Where(a => a.Id == businessHourId).FirstOrDefaultAsync();

            if (businessHour == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("BusinessHourNotFound_", locale));
            }

            if (businessHour.Location.MerchantId != merchant.Id)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            try
            {
                db.Entry(businessHour).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeleteBusinessHour - BusinessHourId {0} Exception {1} InnerException {2}", businessHourId, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteBusinessHour_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("BusinessHourDeleted_", locale));
        }

        #endregion

        #region Bank Account Section

        [HttpGet]
        [Route("{merchantId:long}/bankAccount/{bankAccountId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetBankAccount(long merchantId, long bankAccountId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            BankAccountRS bankAccountRS = new BankAccountRS();

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            if (merchant.Locations.FirstOrDefault().BankingInfo == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("BankingInfoNotFound_", locale));
            }

            if (merchant.Locations.FirstOrDefault().BankingInfoId != bankAccountId)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("BankingInfoNotFound_", locale));
            }

            #endregion

            bankAccountRS = Mapper.Map<BankingInfo, BankAccountRS>(merchant.BankingInfo.Locations.FirstOrDefault().BankingInfo);

            return Content(HttpStatusCode.OK, bankAccountRS);
        }

        [HttpPost]
        [Route("{merchantId:long}/bankAccount")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddBankAccount(long merchantId, [FromBody] addBankAccountRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.BankingInfo bankingInfo = null;
            BankAccountRS bankAccountRS = null;

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

            if (merchant.Locations.FirstOrDefault().BankingInfoId != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("BankingInfoExist_", locale));
            }

            #endregion

            bankingInfo = new BankingInfo();

            bankingInfo = Mapper.Map<addBankAccountRQ, BankingInfo>(model);

            bankingInfo.Locations.Add(merchant.Locations.FirstOrDefault());
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

            bankAccountRS = Mapper.Map<BankingInfo, BankAccountRS>(bankingInfo);

            return Content(HttpStatusCode.OK, bankAccountRS);
        }

        [HttpPut]
        [Route("{merchantId:long}/bankAccount/{bankAccountId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateBankAccount(long merchantId, long bankAccountId, [FromBody] UpdateBankAccountRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            BankAccountRS bankAccountRS = null;
            BankingInfo bankingInfo = new BankingInfo();

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

            bankingInfo = await db.BankingInfos.Where(a => a.Id == bankAccountId).FirstOrDefaultAsync();

            if (bankingInfo == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("BankingInfoNotFound_", locale));
            }

            if (merchant.Id != bankingInfo.Locations.FirstOrDefault().MerchantId)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("BankingInfoNotFound_", locale));
            }

            #endregion

            bankingInfo.AccountName = model.accountName;
            bankingInfo.Branch = model.branch;
            bankingInfo.Transit = model.transit;
            bankingInfo.Account = model.account;
            bankingInfo.ModificationDate = DateTime.Now;
            bankingInfo.SpecimenPath = model.specimenPath;

            try
            {
                db.Entry(bankingInfo).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdateBankAccount bankAccountId {0} Exception {1} InnerException {2}", bankAccountId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateBankingInfo_", locale));
            }

            bankAccountRS = Mapper.Map<BankingInfo, BankAccountRS>(bankingInfo);

            return Content(HttpStatusCode.OK, bankAccountRS);
        }

        #endregion

        #region Clerk Section

        [HttpPost]
        [Route("{merchantId:long}/clerks")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddClerk(long merchantId, [FromBody] addUserRQ model, [fromHeader] string locale = "en")
        {
            ApplicationUser user = null;
            Language language = null;
            Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();
            Common.Core.Data.Merchant merchant = null;

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

            if (string.IsNullOrEmpty(model.language))
                model.language = locale;

            language = await db.Languages.Where(a => a.ISO639_1 == model.language).FirstOrDefaultAsync();

            if (language == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("LanguageNotFound_", locale));
            }

            #endregion

            user = new ApplicationUser
            {
                UserName = model.email,
                Email = model.email
            };

            AspNetUser exist = await db.AspNetUsers.FirstOrDefaultAsync(a => a.UserName == model.email);

            if (exist != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("EmailAlreadyUsed_", locale));
            }

            IdentityResult result;
            result = await UserManager.CreateAsync(user, model.password);

            if (!result.Succeeded == true)
            {
                logger.ErrorFormat("AddMerchantAdmin - UserId {0}", user.Id);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddUser_", locale));
            }

            //Add role member to new user
            result = UserManager.AddToRole(user.Id, IMSRole.MerchantUser.ToString());

            if (!result.Succeeded == true)
            {
                logger.ErrorFormat("AddToRole Not Created - UserId {0}", user.Id);
                UserManager.Delete(user);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddUser_", locale));
            }

            IMS.Common.Core.Data.IMSUser newUser = new Common.Core.Data.IMSUser();
            newUser.FirstName = model.firstName;
            newUser.LastName = model.lastName;
            newUser.AspNetUser = new AspNetUser();
            newUser.AspNetUser.Email = model.email;
            newUser.LanguageId = language.Id;
            newUser.UserId = user.Id;
            newUser.EnterpriseId = enterprise.Id;
            newUser.Merchants.Add(merchant);

            var cmd = DataCommandFactory.AddIMSUserCommand(newUser, "USER", db);

            var resultCmd = await cmd.Execute();

            if (resultCmd != DataCommandResult.Success)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddUser_", locale));
            }

            #region Email Validation Section

            try
            {
                var emailValidationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                var callbackUrl = String.Format(ConfigurationManager.AppSettings["IMS.Service.WebAPI.User.EmailValidation.CallbackUrl"], newUser.Id, HttpUtility.UrlEncode(emailValidationToken), locale);

                var subject = Messages.ResourceManager.GetString("CourrielEmailValidationSubject_" + locale);
                var textLink = Messages.ResourceManager.GetString("CourrielEmailValidationTextLink_" + locale);

                await new EmailService().SendConfirmEmailAddressEmail(newUser, model.email, subject, textLink, callbackUrl);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("SendWelcomeEmail - UserId {0} Exception {1} Inner Exception {2}", newUser.Id, ex.ToString(), ex.InnerException.ToString());
            }

            #endregion

            addClerkRS clerkRS = new addClerkRS();
            clerkRS.clerkId = newUser.Id;

            return Content(HttpStatusCode.OK, clerkRS);
        }

        [HttpGet]
        [Route("{merchantId:long}/clerks")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetClerks(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            List<UserDTO> usersDTO = new List<UserDTO>();
            List<IMSUser> imsUsers = new List<IMSUser>();

            #endregion

            #region Validation Section

            try
            {
                imsUsers = await db.IMSUsers.Where(a => a.Merchants.Any(b => b.Id == merchantId) && a.IsActive == true && a.AspNetUser.AspNetRoles.Any(c => c.Name == IMSRole.MerchantUser.ToString())).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetClerks - MerchantId {0} Exception {1} InnerException {2}", merchantId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, "Unable to retrieve clerks from merchant");
            }

            #endregion

            usersDTO = Mapper.Map<List<IMSUser>, List<UserDTO>>(imsUsers);

            return Content(HttpStatusCode.OK, usersDTO);
        }

        [HttpGet]
        [Route("{merchantId:long}/clerks/{clerkId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetClerk(long merchantId, long clerkId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            UserDTO userDTO = new UserDTO();
            IMSUser imsUser = new IMSUser();

            #endregion

            #region Validation Section

            try
            {
                imsUser = await db.IMSUsers.Where(a => a.Id == clerkId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetClerk - UserId {0} Exception {1} InnerException {2}", clerkId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveUser_", locale));
            }

            if (imsUser == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UserNotFound_", locale));
            }

            try
            {
                merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetClerk - MerchantId {0} Exception {1} InnerException {2}", merchantId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveUser_", locale));
            }

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            if (!merchant.IMSUsers.Any(a => a.Id == imsUser.Id))
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("UserNotFound_", locale));
            }

            #endregion

            userDTO = Mapper.Map<IMSUser, UserDTO>(imsUser);
            //userDTO.language = imsUser.Language.ISO639_1;
            userDTO.notifications = new List<UserNotificationDTO>();

            return Content(HttpStatusCode.OK, userDTO);
        }

        [HttpDelete]
        [Route("{merchantId:long}/clerks/{clerkId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteClerk(long merchantId, long clerkId, [fromHeader] string locale = "en")
        {
            Common.Core.Data.IMSUser imsUser = null;

            #region Validation Section

            imsUser = await db.IMSUsers.FirstOrDefaultAsync(a => a.Id == clerkId);

            if (imsUser == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UserNotFound_", locale));
            }

            #endregion

            try
            {
                var deleteUserCmd = DataCommandFactory.DeleteIMSUserCommand(imsUser, IMSRole.MerchantUser.ToString(), db);
                var result = await deleteUserCmd.Execute();

                if (result != DataCommandResult.Success)
                {
                    logger.ErrorFormat("DeleteClerk - UserId {0} TransaxResult {1}", clerkId, result);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteUser_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeleteClerk - UserId {0} Exception {1} InnerException {2}", clerkId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteUser_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("UserDeleted_", locale));
        }

        [HttpPut]
        [Route("{merchantId:long}/clerks/{clerkId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateClerk(long merchantId, long clerkId, [FromBody] UpdateUserRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            UserDTO userDTO = new UserDTO();
            Common.Core.Data.IMSUser imsUser = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            try
            {
                imsUser = await db.IMSUsers.FirstOrDefaultAsync(a => a.Id == clerkId && a.IsActive == true);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetMerchantAdmin - UserId {0} Exception {1} InnerException {2}", clerkId, ex, ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveUser_", locale));
            }

            if (imsUser == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UserNotFound_", locale));
            }

            if (imsUser.Merchants != null && imsUser.Merchants.FirstOrDefault().Id != merchantId)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            imsUser = Mapper.Map<UpdateUserRQ, IMSUser>(model);
            imsUser.AspNetUser.Email = model.email;
            imsUser.AspNetUser.UserName = model.email;

            var cmd = DataCommandFactory.UpdateIMSUserCommand(imsUser, IMSRole.MerchantAdmin.ToString(), db);

            var resultCmd = await cmd.Execute();

            if (resultCmd != DataCommandResult.Success)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateUser_", locale));
            }

            userDTO = Mapper.Map<IMSUser, UserDTO>(imsUser);

            return Content(HttpStatusCode.OK, userDTO);
        }

        #endregion

        #region Community Section

        [HttpPost]
        [Route("{merchantId:long}/communities")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddCommunity(long merchantId, [FromBody] AddCommunityRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.Program program = null;
            CommunityRS communityRS = null;

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

            if (merchant.ProgramId != null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantAlreadyInCommunity_", locale));
            }

            #endregion

            program = new Common.Core.Data.Program();

            program = Mapper.Map<AddCommunityRQ, Common.Core.Data.Program>(model);

            program.ProgramTypeId = (int)ProgramTypeEnum.Local;
            program.CreationDate = DateTime.Now;
            program.LoyaltyCostUsingPoints = 100;
            program.LoyaltyValueGainingPoints = 100;
            program.FidelityRewardPercent = 0;
            program.WithoutPromoAllowed = false;
            program.ExpirationInMonth = 12;
            program.IsActive = true;
            program.CardTypeId = (int)IMSCardType.MembershipCard;

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
                logger.ErrorFormat("AddBankingInfo - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddCommunity_", locale));
            }

            communityRS = Mapper.Map<Common.Core.Data.Program, CommunityRS>(program);

            return Content(HttpStatusCode.OK, communityRS);
        }

        [HttpPost]
        [Route("{merchantId:long}/joinCommunity/{communityId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> JoinCommunity(long merchantId, long communityId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.Program program = null;
            CommunityRS communityRS = null;

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

            Enterprise enterprise = await db.Enterprises.Where(a => a.Name.ToLower().Contains("trendigo")).FirstOrDefaultAsync();

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

            communityRS = Mapper.Map<Common.Core.Data.Program, CommunityRS>(program);

            return Content(HttpStatusCode.OK, communityRS);

        }

        [HttpPut]
        [Route("{merchantId:long}/communities/{communityId}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateCommunity(long merchantId, long communityId, [FromBody] UpdateCommunityRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.Program program = null;
            CommunityRS communityRS = null;

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

            program = await db.Programs.Where(a => a.Id == communityId).FirstOrDefaultAsync();

            if (program == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("CommunityNotFound_", locale));
            }

            if (merchant.ProgramId != program.Id)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("UnableToJoinCommunity_", locale));
            }

            #endregion

            program.ShortDescription = model.name;
            program.Description = model.name;

            try
            {
                var cmd = DataCommandFactory.UpdateProgramCommand(program, db);
                var result = await cmd.Execute();

                if (result != DataCommandResult.Success)
                {

                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateCommunity_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdateProgramCommand - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateCommunity_", locale));
            }

            communityRS = Mapper.Map<Common.Core.Data.Program, CommunityRS>(program);

            return Content(HttpStatusCode.OK, communityRS);
        }

        [HttpGet]
        [Route("{merchantId:long}/communities")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetMerchantCommunity(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            CommunityRS communityRS = new CommunityRS();

            #endregion

            #region Validation Section

            try
            {
                merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheCommunity_", locale));
            }

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            if (merchant.ProgramId == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("CommunityNotFound_", locale));
            }

            #endregion

            communityRS.communityId = merchant.ProgramId.Value;
            communityRS.communityTypeId = merchant.Program.ProgramTypeId.Value;
            communityRS.name = merchant.Program.ShortDescription;

            return Content(HttpStatusCode.OK, communityRS);
        }

        #endregion

        #region Holiday Section

        [HttpPost]
        [Route("{merchantId:long}/holidays")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddHoliday(long merchantId, [FromBody] List<AddHolidayRQ> models, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<HolidayRS> addHolidaysRS = new List<HolidayRS>();
            HolidayRS addHolidayRS = null;
            LocationHoliday holiday = new LocationHoliday();
            LocationHoliday holidayExist = null;

            DateTime from;
            DateTime to;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || models == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            foreach (AddHolidayRQ model in models)
            {
                if (!DateTime.TryParse(model.startDate, out from))
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("StartingDateNotAValidDate_", locale));
                }

                if (!DateTime.TryParse(model.endDate, out to))
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("EndingDateNotAValidDate_", locale));
                }

                holidayExist = merchant.Locations.FirstOrDefault().LocationHolidays.FirstOrDefault(a => a.FromDate <= from && a.ToDate >= from);

                if (holidayExist == null)
                {
                    holidayExist = merchant.Locations.FirstOrDefault().LocationHolidays.FirstOrDefault(a => a.FromDate <= to && a.ToDate >= to);
                }

                if (holidayExist != null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("HolidayAlreadyExist_", locale));
                }

                holiday.Location = merchant.Locations.FirstOrDefault();
                holiday.Name = model.name;
                holiday.FromDate = from;
                holiday.ToDate = to;
                holiday.CreationDate = DateTime.Now;
                holiday.ModificationDate = holiday.CreationDate;
                holiday.IsActive = true;

                db.LocationHolidays.Add(holiday);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("AddHoliday - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddHoliday_", locale));
                }

                addHolidayRS = Mapper.Map<LocationHoliday, HolidayRS>(holiday);

                addHolidaysRS.Add(addHolidayRS);
            }

            return Content(HttpStatusCode.OK, addHolidaysRS);
        }

        [HttpGet]
        [Route("{merchantId:long}/holidays")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetHolidays(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<HolidayRS> holidaysRS = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            holidaysRS = Mapper.Map<List<LocationHoliday>, List<HolidayRS>>(merchant.Locations.FirstOrDefault().LocationHolidays.Where(a => a.IsActive == true).ToList());

            return Content(HttpStatusCode.OK, holidaysRS);
        }

        [HttpGet]
        [Route("{merchantId:long}/holidays/{holidayId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetHoliday(long merchantId, long holidayId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.LocationHoliday holiday = null;
            HolidayRS holidayRS = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            holiday = await db.LocationHolidays.Where(a => a.Id == holidayId).FirstOrDefaultAsync();

            if (holiday == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("HolidayNotFound_", locale));
            }

            if (holiday.Location.MerchantId != merchant.Id)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            holidayRS = Mapper.Map<LocationHoliday, HolidayRS>(holiday);

            return Content(HttpStatusCode.OK, holidayRS);
        }

        [HttpDelete]
        [Route("{merchantId:long}/holidays/{holidayId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteHoliday(long merchantId, long holidayId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.LocationHoliday holiday = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            holiday = await db.LocationHolidays.Where(a => a.Id == holidayId).FirstOrDefaultAsync();

            if (holiday == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("HolidayNotFound_", locale));
            }

            if (holiday.Location.MerchantId != merchant.Id)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            try
            {
                db.Entry(holiday).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeleteHoliday - HolidayId {0} Exception {1} InnerException {2}", holidayId, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteHoliday_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("HolidayDeleted_", locale));
        }

        [HttpPut]
        [Route("{merchantId:long}/holidays/{holidayId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateHoliday(long merchantId, long holidayId, [FromBody] UpdateHolidayRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.LocationHoliday holiday = null;
            HolidayRS holidayRS = null;

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

            holiday = await db.LocationHolidays.Where(a => a.Id == holidayId).FirstOrDefaultAsync();

            if (holiday == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("HolidayNotFound_", locale));
            }

            if (holiday.Location.MerchantId != merchant.Id)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            try
            {
                db.Entry(holiday).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("UpdateHoliday HolidayId {0} Exception {1} InnerException {2}", holidayId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateHoliday_", locale));
            }

            //holidayRS = Mapper.Map<LocationHoliday, HolidayRS>(holiday);

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyUpdated_", locale));
        }


        #endregion

        #region Transaction Section

        [HttpGet]
        [Route("{merchantId:long}/transactions")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> TransactionHistory(long merchantId, [FromBody] transactionHistoryRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<int> LineItemIds = new List<int> { 7, 8, 9 };
            List<MerchantTransactionHistoryRS> result = new List<MerchantTransactionHistoryRS>();

            #endregion

            #region Validation Section

            if (!ModelState.IsValid || model == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            int start = model.start.HasValue ? model.start.Value : 0;
            int length = model.length.HasValue ? model.length.Value : 1000;

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            if (model.fromDate.HasValue && model.toDate.HasValue)
            {
                if (model.fromDate.Value > model.toDate.Value)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("StartingDateOlderThanEndingDate_", locale));
                }
            }

            #endregion

            try
            {
                result = (
                          from ft in db.TrxFinancialTransactions
                          join nft in db.TrxNonFinancialTransactions on ft.legTransaction equals nft.Id
                          join l in db.Locations on nft.entityId equals l.TransaxId
                          join m in db.Merchants on l.MerchantId equals m.Id
                          join u in db.IMSUsers on nft.vendorId equals u.TransaxId
                          join d in db.IMS_Detail on ft.Id equals d.TransactionId
                          join h in db.IMS_Header on d.HeaderId equals h.Id
                          where m.Id == merchant.Id
                          && ft.localDateTime >= (model.fromDate ?? DateTime.MinValue)
                          && ft.localDateTime <= (model.toDate ?? DateTime.MaxValue)
                          where d.LineItemId == 700
                            select new MerchantTransactionHistoryRS
                            {
                                transactionId = ft.Id,
                                date = ft.localDateTime ?? DateTime.Now,
                                //collaborator = string.Concat(u.FirstName, " ", u.LastName),
                                amount = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0),
                                tip = ft.tip ?? 0,
                                fees = d.Amount,
                                reward = nft.pointsGained == null ? 0 : nft.pointsGained.Value,
                                pointUsed = nft.pointsExpended == null ? 0 : nft.pointsExpended.Value,
                                toBePaid = ((ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0)) - d.Amount,
                                paymentStatus = h.PaymentStatu.Description

                            }).Distinct().OrderByDescending(a => a.date).Skip(start).Take(length).ToList();
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("TransactionsHistory - MerchantId {0}, Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToGetTransactionHistory_", locale));
            }

            //if (model.fromDate.HasValue && model.toDate.HasValue)
            //{
            //    result = result.Where(a => a.date >= model.fromDate.Value && a.date <= model.toDate.Value).ToList();
            //}

            return Content(HttpStatusCode.OK, result);
        }

        #endregion

        #region Statistics Section

        [HttpGet]
        [Route("{merchantId:long}/stats")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetStats(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<TransactionStatsRS> result = new List<TransactionStatsRS>();
            StatsRS statRS = new StatsRS();

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion


            try
            {
                result = (from ft in db.TrxFinancialTransactions
                          join nft in db.TrxNonFinancialTransactions on ft.legTransaction equals nft.Id
                          join l in db.Locations on ft.entityId equals l.TransaxId
                          join m in db.Merchants on l.MerchantId equals m.Id
                          where m.Id == merchantId
                          select new TransactionStatsRS
                          {
                              transactionId = ft.Id,
                              date = ft.localDateTime ?? DateTime.Now,
                              amount = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0),
                              reward = nft.pointsGained == null ? 0 : nft.pointsGained.Value
                          }).ToList();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetInvoice - MerchantId {0}, Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToGetStatistic_", locale));
            }

            if (result != null && result.Count > 0)
            {
                statRS.averagePerTransaction = Convert.ToInt32(result.Average(a => a.amount));
                statRS.totalTransaction = result.Count();
                decimal totReward = result.Sum(a => a.reward) / 100;
                statRS.totalReward = Convert.ToInt32(Math.Ceiling(totReward));
                statRS.totalAmount = Convert.ToInt32(result.Sum(a => a.amount));
            }

            return Content(HttpStatusCode.OK, statRS);
        }

        #region Rating Section

        [HttpPost]
        [Route("{merchantId:long}/ratings")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddRating(long merchantId, [FromBody] ratingRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Rating rating = new Rating();

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

            try
            {
                rating.FinancialTransactionId = Convert.ToInt64(model.transactionId);
                rating.Value = model.rating;
                merchant.Ratings.Add(rating);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddRating_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("RatingAddedSuccessfully_", locale));
        }

        #endregion



        #endregion

        #region QRCode Section

        [HttpPost]
        [Route("{merchantId:long}/qrCode")]
        public async Task<IHttpActionResult> GenerateQRCode(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            var token = Request.Headers.GetValues("sessionToken");

            #endregion

            #region Validation Section

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            if (string.IsNullOrEmpty(token.First()))
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            Payment.PaymentAPI.Model.QrcodeGenerationRequest qrCodeRQ = new Payment.PaymentAPI.Model.QrcodeGenerationRequest();
            qrCodeRQ.Amount = 0;
            qrCodeRQ.Clerk = "";
            qrCodeRQ.Currency = merchant.Locations.FirstOrDefault().Address.Country.Currency.Code;
            qrCodeRQ.LocationId = Convert.ToInt64(merchant.Locations.FirstOrDefault().TransaxId);
            qrCodeRQ.OrderNumber = "";
            qrCodeRQ.PayWithPoints = merchant.Locations.FirstOrDefault().PayWithPoints.HasValue ? merchant.Locations.FirstOrDefault().PayWithPoints.Value : true;
            qrCodeRQ.SupportTips = merchant.Locations.FirstOrDefault().EnableTips.HasValue ? merchant.Locations.FirstOrDefault().EnableTips.Value : true;
            qrCodeRQ.QrcodeInformation = new Payment.PaymentAPI.Model.QrcodeInformation();
            qrCodeRQ.QrcodeInformation.ImageSize = 256;
            qrCodeRQ.QrcodeInformation.ImageFormat = "PNG";

            try
            {
                var qrCode = new Payment.PaymentAPI.Api.QrcodesApi().QrcodeGeneration(token.First(), "0", qrCodeRQ, locale);

                MemoryStream stream = new MemoryStream();
                stream.Write(qrCode, 0, qrCode.Length);

                var response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new ByteArrayContent(qrCode);
                response.Content.LoadIntoBufferAsync(qrCode.Length).Wait();
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                //response.Content.Headers.ContentLength = stream.Length;

                return ResponseMessage(response);
            }
            catch(Payment.PaymentAPI.Client.ApiException apiEx)
            {
                logger.ErrorFormat("QrcodeGeneration - ErrorCode {0}", apiEx.ErrorCode);
                logger.ErrorFormat("QrcodeGeneration - ErrorContent {0}", apiEx.ErrorContent);
                logger.ErrorFormat("QrcodeGeneration - Data {0}", apiEx.Data);
                logger.ErrorFormat("QrcodeGeneration - Message {0}", apiEx.Message);
                logger.ErrorFormat("QrcodeGeneration - InnerException {0}", apiEx.InnerException);
                logger.ErrorFormat("QrcodeGeneration - StackTrace {0}", apiEx.StackTrace);
                logger.ErrorFormat("QrcodeGeneration - TargetSite {0}", apiEx.TargetSite);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToGenerateQrCode_", locale));
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("GetQRCode - MerchantId {0} Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToGenerateQrCode_", locale));
            }
        }

        #endregion

        #region Invoice Section

        [HttpGet]
        [Route("{merchantId:long}/invoices/{invoiceId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetInvoice(long merchantId, long invoiceId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Location location = null;
            TrxFinancialTransaction financialTransaction = null;
            MemberInvoiceTransactionRS result = null;
            Common.Core.Data.Merchant merchant = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            financialTransaction = await db.TrxFinancialTransactions.Where(a => a.Id == invoiceId).FirstOrDefaultAsync();

            if (financialTransaction == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("TransactionNotFound_", locale));
            }

            if (merchant.Locations.FirstOrDefault().TransaxId != financialTransaction.entityId)
            {
                return Content(HttpStatusCode.Unauthorized, MessageService.GetMessage("TransactionNotFound_", locale));
            }

            #endregion

            try
            {
                result = (from ft in db.TrxFinancialTransactions
                          join nft in db.TrxNonFinancialTransactions on ft.legTransaction equals nft.Id
                          join l in db.Locations on nft.entityId equals l.TransaxId
                          join m in db.Merchants on l.MerchantId equals m.Id
                          join u in db.IMSUsers on ft.vendorId equals u.TransaxId
                          join cc in db.CreditCards on ft.creditCardId.Value.ToString() equals cc.TransaxId into ncc
                          from ccardInfo in ncc.DefaultIfEmpty()
                          where ft.Id == invoiceId
                          select new MemberInvoiceTransactionRS
                          {
                              merchantName = m.Name,
                              merchantAddress = l.Address.StreetAddress,
                              merchantCity = l.Address.City,
                              merchantState = l.Address.State.Name,
                              merchantZip = l.Address.Zip,
                              merchantPhone = l.Telephone,
                              invoiceId = ft.Id,
                              orderNumber = nft.orderNumber,
                              transactionTypeId = ft.transactionTypeId.Value,
                              collaborator = ft.description,
                              //collaborator = string.Concat(u.FirstName, " ", u.LastName),
                              date = ft.localDateTime ?? DateTime.Now,
                              amount = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0) - ft.tip ?? 0,
                              tip = ft.tip ?? 0,
                              total = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0),
                              reward = nft.pointsGained == null ? 0 : nft.pointsGained.Value,
                              pointUsed = nft.pointsExpended == null ? 0 : nft.pointsExpended.Value,
                              cardNumber = ccardInfo.CardNumber ?? string.Empty,
                              cardType = ccardInfo.CreditCardType.Description ?? string.Empty,
                              authorization = ft.merchantResponseMessage.Replace(" ", "").Replace("APPROVED", ""),
                              cardTransactionAmount = (ft.baseAmount ?? 0) + (ft.aditionalAmount ?? 0) - (nft.pointsExpended.HasValue ? nft.pointsExpended.Value / 100 : 0)
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetInvoice - MerchantId {0}, Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToGetInvoice_", locale));
            }

            if (result == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("InvoiceNotFound_", locale));
            }

            return Content(HttpStatusCode.OK, result);
        }

        #endregion

        #region Reward Section

        [HttpGet]
        [Route("{merchantId:long}/rewards")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetRewards(long merchantId, [FromBody] getRewardRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<getRewardRS> rewards = new List<getRewardRS>();
            List<Promotion_Schedules> promotions = new List<Promotion_Schedules>();

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

            try
            {
                promotions = await db.Promotion_Schedules.Where(a => a.Promotion.Locations.FirstOrDefault().MerchantId == merchantId && a.IsActive == true && (a.StartDate.Year == model.year && a.StartDate.Month == model.month)).ToListAsync();
            }
            catch(Exception ex)
            {
                logger.ErrorFormat("GetRewards MerchantId {0} Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToGetReward_", locale));
            }

            rewards = Mapper.Map<List<Promotion_Schedules>, List<getRewardRS>>(promotions);

            return Content(HttpStatusCode.OK, rewards);
        }

        [HttpPost]
        [Route("{merchantId:long}/rewards")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddReward(long merchantId, [FromBody] List<addRewardRQ> model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.Promotion promotion = null;
            Common.Core.Data.Promotion_Schedules promo_schedule = null;
            List<addRewardRS> rewardsRS = new List<addRewardRS>();
            addRewardRS rewardRS = new addRewardRS();
            var dayOfWeek = 0;

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

            promotion = merchant.Locations.FirstOrDefault().Promotions.FirstOrDefault(a => a.IsActive);

            if (promotion == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("RewardNotSetForThatMerchant_", locale));
            }

            foreach(addRewardRQ reward in model)
            {
                Promotion_Schedules promoExist = await db.Promotion_Schedules.Where(a => a.Promotion.Locations.FirstOrDefault().MerchantId == merchantId && a.StartDate == reward.rewardDate && a.IsActive == true).FirstOrDefaultAsync();

                if (promoExist != null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("RewardExistForThatDate_", locale));
                }

                dayOfWeek = (int)Convert.ToDateTime(reward.rewardDate).DayOfWeek;

                long locationId = merchant.Locations.FirstOrDefault().Id;
                LocationBusinessHour bhAvail = await db.LocationBusinessHours.Where(a => a.LocationId == locationId && a.DayOfWeekID == dayOfWeek).FirstOrDefaultAsync();

                if (bhAvail == null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("NotOpenBusinessDay_", locale));
                }

                LocationHoliday holExist = await db.LocationHolidays.Where(a => a.LocationId == locationId && a.IsActive == true && a.FromDate <= reward.rewardDate && a.ToDate >= reward.rewardDate).FirstOrDefaultAsync();

                if (holExist != null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("NotOpenHolidayPeriod_", locale));
                }
            }

            #endregion

            #region Promotion Schedule Section

            foreach (addRewardRQ reward in model)
            {
                dayOfWeek = (int)Convert.ToDateTime(reward.rewardDate).DayOfWeek;
                var businessHour = promotion.Locations.FirstOrDefault(a => a.IsActive == true).LocationBusinessHours.FirstOrDefault(x => x.DayOfWeekID == dayOfWeek);

                TimeSpan? locationOpeningHour = businessHour != null ? businessHour.OpeningHour : (TimeSpan?)null;
                TimeSpan? locationClosingHour = businessHour != null ? businessHour.ClosingHour : (TimeSpan?)null;

                if (locationOpeningHour == null || locationClosingHour == null)
                {
                    return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotOpenOnThatDate_", locale));
                }

                promo_schedule = new Promotion_Schedules();

                //Promotion Start Date Time
                promo_schedule.StartDate = DateTime.Parse(reward.rewardDate.ToString(), null, DateTimeStyles.RoundtripKind);
                promo_schedule.StartTime = new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["Promotion.Start.Time"]), 0, 0);
                //Promotion End Date Time
                promo_schedule.EndDate = Convert.ToDateTime(reward.rewardDate).AddDays(1);
                promo_schedule.EndTime = new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["Promotion.End.Time"]), 0, 0);

                promo_schedule.Value = reward.rewardPercentage >= 1 ? reward.rewardPercentage / 100 : reward.rewardPercentage;
                promo_schedule.Promotion = promotion;

                try
                {
                    var command2 = DataCommandFactory.AddPromotionScheduleCommand(promo_schedule, db);
                    var result2 = await command2.Execute();
                    if (result2 != DataCommandResult.Success)
                    {
                        logger.ErrorFormat("AddPromotionSchedule Result {0} MerchantId {1} Date {2} Percentage {3}", result2.ToString(), merchantId, reward.rewardDate, reward.rewardPercentage);
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddReward_", locale));
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("AddPromotionSchedule MerchantId {0} PromotionId {1} Date {2} Percentage {3}", merchantId, promotion.Id, reward.rewardDate, reward.rewardPercentage);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddReward_", locale));
                }

                #endregion

                rewardRS.rewardId = promo_schedule.Id;
                rewardRS.merchantId = merchantId;
                rewardRS.rewardDate = reward.rewardDate;
                rewardRS.rewardPercentage = reward.rewardPercentage;

                rewardsRS.Add(rewardRS);
            }

            return Content(HttpStatusCode.OK, rewardsRS);
        }

        [HttpPut]
        [Route("{merchantId:long}/rewards/{rewardId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> UpdateReward(long merchantId, long rewardId, [FromBody] updateRewardRQ model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.Promotion_Schedules promotion = null;

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

            promotion = await db.Promotion_Schedules.Where(a => a.Id == rewardId && a.IsActive == true && a.StartDate == model.rewardDate).FirstOrDefaultAsync();

            if (promotion == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("RewardNotFound_", locale));
            }

            if (promotion.StartDate.Date <= DateTime.Now.Date)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("UnableToUpdateReward_", locale));
            }

            #endregion

            promotion.Value = model.rewardPercentage;

            try
            {
                var command = DataCommandFactory.UpdatePromotionScheduleCommand(promotion, db);
                var result = await command.Execute();
                if (result != DataCommandResult.Success)
                {
                    logger.ErrorFormat("UpdatePromotionSchedule Result {0} MerchantId {1} Date {2} Percentage {3}", result.ToString(), merchantId, model.rewardDate, model.rewardPercentage);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateReward_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("UpdatePromotionSchedule MerchantId {0} PromotionId {1} Date {2} Percentage {3}", merchantId, promotion.Id, model.rewardDate, model.rewardPercentage);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateReward_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("SuccessfullyUpdated_", locale));
        }

        [HttpDelete]
        [Route("{merchantId:long}/rewards/{rewardId}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteReward(long merchantId, long rewardId, [fromHeader] string locale = "en")
        {

            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.Promotion_Schedules promotion = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            promotion = await db.Promotion_Schedules.Where(a => a.Id == rewardId && a.IsActive == true).FirstOrDefaultAsync();

            if (promotion == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("RewardNotFound_", locale));
            }

            if (promotion.StartDate.Date <= DateTime.Now.Date)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("UnableToDeleteReward_", locale));
            }

            #endregion

            try
            {
                var command = DataCommandFactory.DeletePromotionScheduleCommand(promotion, db);
                var result = await command.Execute();
                if (result != DataCommandResult.Success)
                {
                    logger.ErrorFormat("DeletePromotionSchedule Result {0} MerchantId {1} RewardId {2}", result.ToString(), merchantId, rewardId);
                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteReward_", locale));
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeletePromotionSchedule MerchantId {0} RewardId {1} Exception {2} InnerException {3}", merchantId, rewardId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteReward_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("RewardDeleted_", locale));
        }

        #endregion

        #region Tag Section

        [HttpGet]
        [Route("{merchantId:long}/tags")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetTags(long merchantId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            List<MerchantTagRS> tagsRS = new List<MerchantTagRS>();
            List<MerchantTag> tags = new List<MerchantTag>();

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            try
            {
                tags = await db.MerchantTags.Where(a => 
                a.MerchantId == merchantId && 
                a.tag.ParentId.HasValue).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("GetTags MerchantId {0} Exception {1} InnerException {2}", merchantId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheTag_", locale));
            }

            tagsRS = Mapper.Map<List<MerchantTag>, List<MerchantTagRS>>(tags);

            return Content(HttpStatusCode.OK, tagsRS);
        }

        [HttpGet]
        [Route("{merchantId:long}/tags/{taggingId:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetTag(long merchantId, long taggingId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.MerchantTag merchantTag = null;
            MerchantTagRS tagRS = new MerchantTagRS();

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            merchantTag = await db.MerchantTags.Where(a => a.MerchantId == merchant.Id).FirstOrDefaultAsync();

            if (merchantTag == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantTagNotFound_", locale));
            }

            #endregion

            tagRS = Mapper.Map<MerchantTag, MerchantTagRS>(merchantTag);

            return Content(HttpStatusCode.OK, tagRS);
        }

        [HttpPost]
        [Route("{merchantId:long}/tags")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> AddTag(long merchantId, [FromBody] List<AddTagRQ> model, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            MerchantTagRS tagRS = null;
            List<MerchantTagRS> tagsRS = new List<MerchantTagRS>();

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

            foreach (AddTagRQ tag in model)
            {
                MerchantTag tagExist = await db.MerchantTags.Where(a => a.MerchantId == merchant.Id && a.TagId == tag.tagId).FirstOrDefaultAsync();

                if (tagExist == null)
                {
                    MerchantTag newTag = new MerchantTag();
                    newTag.TagId = tag.tagId;
                    newTag.MerchantId = merchant.Id;

                    try
                    {
                        db.Entry(newTag).State = EntityState.Added;
                        await db.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        logger.ErrorFormat("AddTag MerchantId {0} Tag {1} Exception {2} InnerException {3}", merchantId, tag.tagId, ex.ToString(), ex.InnerException.ToString());
                        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToAddTagToMerchant_", locale));
                    }

                    tagRS = Mapper.Map<MerchantTag, MerchantTagRS>(newTag);
                    
                    tagsRS.Add(tagRS);
                }
            }

            #endregion

            return Content(HttpStatusCode.OK, tagsRS);
        }

        [HttpDelete]
        [Route("{merchantId:long}/tags/{taggingId}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteTag(long merchantId, long taggingId, [fromHeader] string locale = "en")
        {

            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            MerchantTag merchantTag = null;

            #endregion

            #region Validation Section

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MissingParameter_", locale));
            }

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            merchantTag = await db.MerchantTags.Where(a => a.Id == taggingId).FirstOrDefaultAsync();

            if (merchantTag == null)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantTagNotFound_", locale));
            }

            if (merchantTag.MerchantId != merchant.Id)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            try
            {
                db.Entry(merchantTag).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeleteTag MerchantId {0} TaggingId {1} Exception {2} InnerException {3}", merchantId, taggingId, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRemoveMerchantTag_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("MerchantTagRemoved_", locale));
        }

        #endregion

        #region Report Section

        #endregion

        #region Image Section

        [HttpDelete]
        [Route("{merchantId:long}/images/{merchantImageId}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteImage(long merchantId, string merchantImageId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.Merchant merchant = null;
            Common.Core.Data.MerchantImage image = null;

            #endregion

            #region Validation Section

            merchant = await db.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            image = await db.MerchantImages.Where(a => a.Id == Guid.Parse(merchantImageId)).FirstOrDefaultAsync();

            if (image == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("ImageNotFound_", locale));
            }

            if (image.MerchantId != merchant.Id)
            {
                return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("MerchantNotFound_", locale));
            }

            #endregion

            try
            {
                db.Entry(image).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeleteMerchantImage - MerchantImageId {0} Exception {1} InnerException {2}", merchantImageId, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteImage_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("ImageDeleted_", locale));
        }

        #endregion

    }
}
