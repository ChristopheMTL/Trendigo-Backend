using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class PromoCodeService
    {
        private IMSEntities db = new IMSEntities();
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public async Task<PromoCode> CreateGiftCardPurchasePromoCode(long enterpriseId, long purchaseGiftCardId, int prefixPoints, DateTime startDate, DateTime endDate) 
        //{
        //    if (string.IsNullOrEmpty(purchaseGiftCardId.ToString()) || string.IsNullOrEmpty(prefixPoints.ToString())) 
        //    {
        //        throw new Exception("Missing parameters");
        //    }

        //    Program regularProgram = new ProgramManager().GetRegularProgram(enterpriseId);

        //    if (regularProgram == null) 
        //    {
        //        throw new Exception("Program not found");
        //    }

        //    PromoCode newPromoCode = new PromoCode();
        //    newPromoCode.AlreadyUsed = 0;
        //    newPromoCode.Code = Guid.NewGuid().ToString();
        //    newPromoCode.CreationDate = DateTime.Now;
        //    newPromoCode.Description = string.Format("Purchase Gift Card Id {0}", purchaseGiftCardId);
        //    newPromoCode.ProgramId = regularProgram.Id;
        //    newPromoCode.EndDate = endDate;
        //    newPromoCode.IsActive = true;
        //    newPromoCode.MaxLimit = 1;
        //    newPromoCode.NewMemberOnly = false;
        //    newPromoCode.PrefixPoints = prefixPoints;
        //    newPromoCode.StartDate = startDate;
        //    newPromoCode.Value = 0;
        //    newPromoCode.FromGiftCard = true;

        //    try 
        //    {
        //        db.PromoCodes.Add(newPromoCode);
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex) 
        //    {
        //        _logger.ErrorFormat("CreateGiftCardPurchasePromoCode - Exception {0}", ex.ToString());
        //        throw new Exception(ex.ToString());
        //    }

        //    return newPromoCode;
        //}

        //public void ApplyPromoCode(long promoCodeId, long memberId) 
        //{
        //    IMSUser ims_user = new RegistrationManager().GetUserWithEmail("francois.verdon@trendigo.com");

        //    PromoCode promoCode = db.PromoCodes.FirstOrDefault(a => a.Id == promoCodeId);
        //    Member member = db.Members.FirstOrDefault(a => a.Id == memberId);

        //    CardPointHistory cardHistory = new CardPointHistory();
        //    cardHistory.IMSCard = card;
        //    cardHistory.IMSCardId = card.Id;
        //    cardHistory.Points = promoCode.PrefixPoints;
        //    cardHistory.Reason = "Website promocode insertion";
        //    cardHistory.CreatedBy = ims_user.Id;

        //    var command = DataCommandFactory.AddPointsCardNonFinancialCommand(cardHistory, promoCode.PrefixPoints, member, this.db);

        //    var result = await command.Execute();

        //    if (result != DataCommandResult.Success)
        //    {
        //        throw new Exception(string.Format("AddPointsCardNonFinancialCommand, unable to add points. memberId {0} cardId {1} code {2}", member.Id, card.Id, promoCode.Code));
        //    }

        //    try
        //    {
        //        MemberPromoCodeHistory newMemberPromoCodeHistory = new MemberPromoCodeHistory();
        //        newMemberPromoCodeHistory.MemberId = member.Id;
        //        newMemberPromoCodeHistory.PromocodeId = promoCode.Id;
        //        newMemberPromoCodeHistory.CardId = card.Id;
        //        newMemberPromoCodeHistory.CardPointHistoryId = cardHistory.Id;
        //        newMemberPromoCodeHistory.CreationDate = DateTime.Now;

        //        db.MemberPromoCodeHistories.Add(newMemberPromoCodeHistory);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        //reverse transaction ... remove points
        //        CardPointHistory cardHistory_removed = new CardPointHistory();
        //        cardHistory_removed.IMSCardId = card.Id;
        //        cardHistory_removed.Points = promoCode.PrefixPoints;
        //        cardHistory_removed.Reason = "Website promocode reverse transaction";
        //        cardHistory_removed.CreatedBy = ims_user.Id;

        //        var command2 = DataCommandFactory.RemovePointsCardNonFinancialCommand(cardHistory_removed, promoCode.PrefixPoints, member, this.db);
        //        var result2 = command2.Execute();

        //        _logger.ErrorFormat("Unable to add promo code history. memberId {0} cardId {1} code {2}", member.Id, card.Id, promoCode.Code);
        //        _logger.ErrorFormat("Exception", ex.ToString());
        //        _logger.ErrorFormat("StackTrace", ex.StackTrace);
        //        throw new Exception(string.Format("Unable to add promo code history. memberId {0} cardId {1} code {2}", member.Id, card.Id, promoCode.Code));
        //    }

        //    try
        //    {
        //        promoCode.AlreadyUsed += 1;
        //        db.Entry(promoCode).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.ErrorFormat("Unable to update already used promo code. memberId {0} cardId {1} code {2}", member.Id, card.Id, promoCode.Code);
        //        _logger.ErrorFormat("Exception", ex.ToString());
        //        _logger.ErrorFormat("StackTrace", ex.StackTrace);
        //        throw new Exception(string.Format("Unable to update already used promo code. memberId {0} cardId {1} code {2}", member.Id, card.Id, promoCode.Code));
        //    }
        //}
    }
}
