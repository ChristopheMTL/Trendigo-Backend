using IMS.Common.Core.Data;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Models;
using IMS.Service.WebAPI2.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("promoCodes")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class PromoCodeController : ApiController
    {
        //private IMSEntities db = new IMSEntities();

        //[HttpPost]
        //[Route("{memberId:long}/redeem")]
        //[JwtAuthentication]
        //public async Task<IHttpActionResult> RedeemPromoCode(long memberId, [FromBody] PromoCodeRedeemRQ model, [fromHeader] string locale = "en")
        //{
        //    #region Declaration Section

        //    IMS.Common.Core.Data.IMSUser ims_user = new RegistrationManager().GetUserWithTransaxId(ConfigurationManager.AppSettings["IMSUserID"]);

        //    #endregion



        //    #region Validation Section

        //    //1. GetMember
        //    var member = await db.Members.FirstOrDefaultAsync(x => x.Id == memberId);

        //    if (member == null)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("MemberNotFound_", locale));
        //    }

        //    //2. GetPromoCode
        //    PromoCode promoCode = db.PromoCodes.Where(a => a.Code == model.promoCode && a.IsActive == true).FirstOrDefault();
        //    if (promoCode == null)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("PromoCodeNotFound_", locale));
        //    }

        //    //4. GetMembership
        //    IMSMembership active_membership = member.IMSMemberships.OrderByDescending(x => x.CreationDate).FirstOrDefault(a => a.IsActive);
        //    if (active_membership == null)
        //    {
        //        throw new Exception("Membership not found");
        //    }

        //    //5. Is PromoCode applicable for that program
        //    if (promoCode.ProgramId != active_membership.ProgramID)
        //    {
        //        throw new Exception("PromoCode not applicable");
        //    }

        //    //6. Validate expiration or limit reach
        //    if (promoCode.EndDate.Date < DateTime.Now.Date || promoCode.MaxLimit < promoCode.AlreadyUsed)
        //    {
        //        throw new Exception("PromoCode expired or max limit reach");
        //    }

        //    //7. Does PromoCode has prefix points
        //    if (promoCode.PrefixPoints == 0)
        //    {
        //        throw new Exception("PromoCode not applicable");
        //    }

        //    //8. Check if code was used before
        //    MemberPromoCodeHistory promoHistory = db.MemberPromoCodeHistories.FirstOrDefault(a => a.MemberId == member.Id && a.PromocodeId == promoCode.Id);
        //    if (promoHistory != null)
        //    {
        //        throw new Exception("PromoCode already used");
        //    }

        //    //New Member section
        //    if (promoCode.NewMemberOnly)
        //    {
        //        //7. Don't apply if Member creation date was created before Promo Code
        //        if (promoCode.CreationDate.Date > member.CreationDate.Date)
        //        {
        //            //logger.ErrorFormat("Apply to new member only. PromoCode creation date {0} Member creation date {1}", promoCode.CreationDate.Date, member.CreationDate.Date);
        //            throw new Exception("PromoCode not applicable");
        //        }

        //        //8. Check if a code as been already used
        //        if (member.MemberPromoCodeHistories.Count() > 0)
        //        {
        //            throw new Exception("PromoCode not applicable, member already used a promo code for new membership");
        //        }
        //    }

        //    #endregion

        //    #region Add Card Point History and Promo Code History

        //    string reason = (promoCode.FromGiftCard == null || promoCode.FromGiftCard == false ? "Website Promocode " : "Giftcard Promocode ") + promoCode.Code;

        //    CardPointHistory cph = new CardPointHistory();
        //    cph.CreatedBy = ims_user.Id;
        //    cph.CreatedDate = DateTime.Now;
        //    cph.IMSCardId = card.Id;
        //    cph.Points = promoCode.PrefixPoints;
        //    cph.PromoCodeId = promoCode.Id;
        //    cph.Reason = reason;

        //    var command_transfer = DataCommandFactory.ApplyPromoCodeCommand(cph, active_membership.TransaxId, promoCode.PrefixPoints, db);

        //    var result_transfer = await command_transfer.Execute();

        //    if (result_transfer != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EWalletController - ApplyPromoCode - Unable to apply point from promocodeId {0} to member {1} total points {2}", promoCode.Id, member.Id, promoCode.PrefixPoints);

        //        throw new Exception(string.Format("EWalletController - ApplyPromoCode - Unable to apply point from promocodeId {0} to member {1} total points {2}", promoCode.Id, member.Id, promoCode.PrefixPoints));
        //    }

        //    #endregion

        //    #region Promo Code Update Section

        //    try
        //    {
        //        promoCode.AlreadyUsed += 1;
        //        db.Entry(promoCode).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("Unable to update already used promo code. memberId {0} cardId {1} code {2}", member.Id, card.Id, promoCode.Code);
        //        logger.ErrorFormat("Exception", ex.ToString());
        //        logger.ErrorFormat("StackTrace", ex.StackTrace);
        //    }

        //    #endregion
        //}
    }
}