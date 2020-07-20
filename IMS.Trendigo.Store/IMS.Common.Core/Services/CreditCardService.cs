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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ccDTO"></param>
        /// <returns></returns>
        public async Task<CreditCard> AddCreditCard(CreditCardDTO ccDTO)
        {
            CreditCard cc = new CreditCard();

            using (IMSEntities db = new IMSEntities())
            {
                cc = Mapper.Map<CreditCardDTO, CreditCard>(ccDTO);
                cc.CreationDate = DateTime.Now;
                cc.IsActive = true;

                var command = DataCommandFactory.AddCreditCardCommand(cc, ccDTO.Token, ccDTO.TransaxId, db);

                try
                {
                    var result = await command.Execute();

                    if (result != DataCommandResult.Success)
                    {
                        throw new Exception(string.Format("CreditCardService - AddCreditCard - Cannot insert credit card - {0}", result.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("CreditCardService - AddCreditCard - UserId {0} Exception {1} InnerException {2}", ccDTO.UserId, ex, ex.InnerException);
                    throw new Exception(string.Format("CreditCardService - AddCreditCard - Cannot insert credit card - UserId {0} Exception {1} InnerException {2}", ccDTO.UserId, ex.ToString(), ex.InnerException.ToString()));
                }
            }

            return cc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CreditCard>> GetCreditCards(string userId)
        {
            List<CreditCard> creditCards = null;

            creditCards = await db.CreditCards.Where(a => a.AspNetUser.Id == userId && a.IsActive == true).ToListAsync();

            return creditCards;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="creditCardId"></param>
        /// <returns></returns>
        public async Task<CreditCard> GetCreditCard(string userId, long creditCardId)
        {

            CreditCard ccard = null;

            ccard = await db.CreditCards.Where(a => a.AspNetUser.Id == userId && a.Id == creditCardId && a.IsActive == true).FirstOrDefaultAsync();

            return ccard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public async Task<Boolean> UpdateCreditCard(CreditCardDTO cc)
        {

            CreditCard ccard = await db.CreditCards.FirstOrDefaultAsync(x => x.Id == cc.Id && x.AspNetUser.Id == cc.UserId && x.IsActive == true);

            if (ccard == null)
            {
                return false;
            }

            ccard.CardHolder = cc.CardHolder;
            ccard.ExpiryDate = cc.ExpiryDate;

            var command = DataCommandFactory.UpdateCreditCardCommand(ccard, db);

            try
            {
                var result = await command.Execute();

                if (result != DataCommandResult.Success)
                {
                    logger.ErrorFormat("CreditCardService - UpdateCreditCard - UserId {0} DataCommandResult {1}", cc.UserId, result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("CreditCardService - UpdateCreditCard - UserId {0} Exception {1} InnerException {2}", cc.UserId, ex, ex.InnerException);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ccDTO"></param>
        /// <returns></returns>
        public async Task<Boolean> DeleteCreditCard(string UserId, long creditCardId)
        {
            #region Validation Section

            CreditCard creditCard = await db.CreditCards.FirstOrDefaultAsync(a => a.Id == creditCardId && a.AspNetUser.Id == UserId && a.IsActive == true);

            if (creditCard == null)
                return false;

            #endregion

            var command = DataCommandFactory.DeleteCreditCardCommand(creditCard, db);

            try
            {
                var result = await command.Execute();

                if (result != DataCommandResult.Success)
                {
                    logger.ErrorFormat("CreditCardService - DeleteCreditCard - CreditCarId {0} result {1}", creditCard.Id, result.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("MemberController - DeleteCreditCard - MemberId {0} CardId {1} Exception {2} InnerException {3}", creditCard.AspNetUser.Members.FirstOrDefault().Id, creditCard.Id, ex.ToString(), ex.InnerException.ToString());
                return false;
            }

            return true;
        }

    }
}
