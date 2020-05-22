using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Utilities.PaymentAPI;
using IMS.Utilities.PaymentAPI.Client;
using AutoMapper;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.DataCommands;
using System.Data.Entity;
using IMS.Common.Core.DTO;

namespace IMS.Common.Core.Services
{
    public class CreditCardService
    {
        IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IMS.Utilities.PaymentAPI.Api.MembersApi memberApi = new IMS.Utilities.PaymentAPI.Api.MembersApi();
        

        /// <summary>
        /// This method format the credit card number to display according to the number of digits
        /// </summary>
        /// <param name="cardNumber">Card number to format</param>
        /// <returns>Formatted card number</returns>
        public string FormatNumber(Int64 cardNumber) 
        {
            string formattedCardNumber = cardNumber.ToString();

            switch (cardNumber.ToString().Length) 
            { 
                case 16:
                    formattedCardNumber = String.Format("{0:0000 0000 0000 0000}", cardNumber);
                    break;
                case 17:
                    formattedCardNumber = String.Format("{0:00000 0000 0000 0000}", cardNumber);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return formattedCardNumber;
        }

        /// <summary>
        /// This method validate if a credit card is expired according to the expiry date
        /// </summary>
        /// <param name="dateString">Expiry date (MMYY)</param>
        /// <returns>True or False depending if the expiry date is after of before today</returns>
        public bool isExpired(string dateString)
        {
            DateTime dateValue;
            string year = "20" + dateString.Substring(2, 2);
            string month = dateString.Substring(0, 2);
            string expiryDate = year + "/" + month + "/" + DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

            if (DateTime.TryParse(expiryDate, out dateValue))
                if (dateValue >= DateTime.Now)
                    return false;
                else
                    return true;
            else
                return true;
        }

        /// <summary>
        /// This method validate if a credit card is expiring in the actual year and month
        /// </summary>
        /// <param name="dateString">Expiry date (MMYY)</param>
        /// <returns>True or False depending if the expiry date is the same year and month of today</returns>
        public bool ExpiringThisMonth(string dateString) 
        {
            DateTime dateValue;
            string year = "20" + dateString.Substring(2, 2);
            string month = dateString.Substring(0, 2);
            string expiryDate = year + "/" + month + "/" + DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

            if (DateTime.TryParse(expiryDate, out dateValue))
                if (dateValue.Date == DateTime.Now.AddMonths(1).AddDays(-1).Date)
                    return true;
                else
                    return false;
            else
                return true;
        }

        //public async Task<List<CreditCardDTO>> GetTransaxCreditCards(long memberId)
        //{
        //    List<CreditCardDTO> ccards = new List<CreditCardDTO>();
        //    List<IMS.Utilities.PaymentAPI.Model.Creditcard> _ccards = new List<IMS.Utilities.PaymentAPI.Model.Creditcard>();
        //    List<CreditCardType> ccType = await db.CreditCardTypes.ToListAsync();
        //    Member member = await db.Members.FirstOrDefaultAsync(a => a.Id == memberId);

        //    try
        //    {
        //        _ccards = await new IMS.Utilities.PaymentAPI.Api.MembersApi().FindCreditCards(Convert.ToInt32(member.TransaxId));
        //    }
        //    catch (ApiException apiEx)
        //    {
        //        switch (apiEx.ErrorCode)
        //        {
        //            case (int)APIPaymentException.NOTAUTHORIZED:
        //                logger.ErrorFormat("CreditCardService - GetCreditCards - FindCreditCard - transaxMemberId {0} Not Authorized", member.TransaxId);
        //                break;
        //            case (int)APIPaymentException.NOTFOUND:
        //                logger.ErrorFormat("CreditCardService - GetCreditCards - FindCreditCard - transaxMemberId {0} Not Found", member.TransaxId);
        //                break;
        //            case (int)APIPaymentException.OTHER:
        //                logger.ErrorFormat("CreditCardService - GetCreditCards - Unknowned error {0}", apiEx.ErrorContent);
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }

        //        throw new Exception(string.Format("CreditCardService - GetCreditCards - FindCreditCard ErrorCode {0} ErrorContent {1}", apiEx.ErrorCode, apiEx.ErrorContent));
        //    }

        //    if (_ccards.Count() > 0)
        //    {
        //        var map = Mapper.CreateMap<IMS.Utilities.PaymentAPI.Model.Creditcard, CreditCardDTO>();
        //        map.ForMember(x => x.Id, o => o.MapFrom(model => model.CreditCardId.Value));
        //        map.ForMember(x => x.CardNumber, o => o.MapFrom(model => model.PanMask.PadLeft(12, '*')));
        //        map.ForMember(x => x.CardHolder, o => o.MapFrom(model => model.NameOnCard));
        //        map.ForMember(x => x.CreditCardTypeId, o => o.MapFrom(model => model.CardTypeId.Value));
        //        map.ForMember(x => x.CreditCardType, o => o.MapFrom(model => new CreditCardService().GetCreditCardTypeDTO(Convert.ToInt32(model.CardTypeId))));
        //        map.ForMember(x => x.ExpiryDate, o => o.MapFrom(model => model.ExpirationDate));
        //        map.ForMember(x => x.IsActive, o => o.MapFrom(model => true));
        //        map.ForMember(x => x.MemberId, o => o.MapFrom(model => _ccards.FirstOrDefault().MemberId));
        //        map.ForMember(x => x.Token, o => o.MapFrom(model => model.Token));

        //        ccards = Mapper.Map<List<CreditCardDTO>>(_ccards);
        //    }

        //    return ccards;
        //}

        //public async Task<CreditCardDTO> GetTransaxCreditCard(string memberId, string creditCardId)
        //{
        //    CreditCardDTO ccard = new CreditCardDTO();
        //    IMS.Utilities.PaymentAPI.Model.Creditcard _ccard = null;

        //    try
        //    {
        //        _ccard = await new IMS.Utilities.PaymentAPI.Api.MembersApi().FindCreditCard(Convert.ToInt32(memberId), Convert.ToInt32(creditCardId));
        //    }
        //    catch (ApiException apiEx)
        //    {
        //        switch (apiEx.ErrorCode)
        //        {
        //            case (int)APIPaymentException.NOTAUTHORIZED:
        //                logger.ErrorFormat("CreditCardService - GetCreditCard - FindCreditCard - MemberId {0} Not Authorized", memberId.ToString());
        //                break;
        //            case (int)APIPaymentException.NOTFOUND:
        //                logger.ErrorFormat("CreditCardService - GetCreditCard - FindCreditCard - MemberId {0} Not Found", memberId.ToString());
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("CreditCardService - GetCreditCards - MemberId {0} Exception {1} InnerException {2}", memberId.ToString(), ex.ToString(), ex.InnerException.ToString());
        //        throw new Exception(string.Format("Error extracting the credit card info for member {0}", memberId.ToString()));
        //    }

        //    if (_ccard != null)
        //    {
        //        var map = Mapper.CreateMap<IMS.Utilities.PaymentAPI.Model.Creditcard, CreditCardDTO>();
        //        map.ForMember(x => x.TransaxId, o => o.MapFrom(model => model.CreditCardId));
        //        map.ForMember(x => x.CardNumber, o => o.MapFrom(model => model.PanMask));
        //        map.ForMember(x => x.CardHolder, o => o.MapFrom(model => model.NameOnCard));
        //        //map.ForMember(x => x.CreditCardTypeId, o => o.MapFrom(model => model.))
        //        map.ForMember(x => x.ExpiryDate, o => o.MapFrom(model => model.ExpirationDate));
        //        map.ForMember(x => x.IsActive, o => o.MapFrom(model => model.Status.ToLower() == "active" ? true : false));
        //        map.ForMember(x => x.MemberId, o => o.MapFrom(model => model.MemberId));

        //        ccard = Mapper.Map<CreditCardDTO>(_ccard);

        //        ccard.Id = db.CreditCards.Where(a => a.TransaxId == ccard.TransaxId).Select(b => b.Id).FirstOrDefault();
        //    }

        //    return ccard;
        //}

        public async Task<List<CreditCard>> GetCreditCards(long memberId)
        {
            List<CreditCard> ccards = new List<CreditCard>();
            Member member = await db.Members.FirstOrDefaultAsync(a => a.Id == memberId);

            try
            {
                ccards = await db.CreditCards.Where(a => a.MemberId == memberId && a.IsActive == true).ToListAsync();
            }
            catch (ApiException apiEx)
            {
                logger.ErrorFormat("ServerError - CreditCardService - GetCreditCards - MemberId {0}", member.TransaxId);
                throw new Exception(string.Format("CreditCardService - GetCreditCards - FindCreditCard ErrorCode {0} ErrorContent {1}", apiEx.ErrorCode, apiEx.ErrorContent));
            }

            return ccards;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<CreditCard> GetCreditCard(long memberId, long creditCardId)
        {
            CreditCard _ccard = null;

            try
            {
                _ccard = await db.CreditCards.Where(a => a.MemberId == memberId && a.Id == creditCardId && a.IsActive == true).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("CreditCardService - GetCreditCard - MemberId {0} CreditCardId {1} Exception {2} InnerException {3}", memberId.ToString(), creditCardId.ToString(), ex.ToString(), ex.InnerException.ToString());
                throw new Exception(string.Format("Error extracting the credit card info for memberId {0} creditCardId {1}", memberId.ToString(), creditCardId.ToString()));
            }

            return _ccard;
        }

        public CreditCardTypeDTO GetCreditCardTypeDTO(int Id)
        {
            CreditCardTypeDTO dto = new CreditCardTypeDTO();

            CreditCardType exist = db.CreditCardTypes.FirstOrDefault(a => a.Id == Id);

            if (exist != null)
            {
                dto.Id = exist.Id;
                dto.Description = exist.Description;
            }

            return dto;
        }
    }
}
