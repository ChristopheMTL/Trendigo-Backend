using IMS.Common.Core.Entities;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Exceptions;
using IMS.Store.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using System.Configuration;
using System.Data.Entity;
using IMS.Utilities.PaymentAPI.Model;
using IMS.Utilities.PaymentAPI.Client;

namespace IMS.Common.Core.Services
{
    public class CardManager
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Non Financial Card

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        public async Task<int> GetNonFinancialCardPointBalance(string membershipId) 
        {
            int points = -1;

            if (!string.IsNullOrEmpty(membershipId)) 
            {
                try
                {
                    Point _points = await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().FindMemberPoints(Convert.ToInt32(membershipId));

                    if (_points != null)
                    {
                        points = _points.Value.HasValue ? _points.Value.Value : 0;
                    }
                }
                catch(ApiException apiEx)
                {

                }
                catch (Exception ex)
                {
                    logger.DebugFormat("CardManager - GetNonFinancialCardPointBalance - FindMemberPoints Exception {0} InnerException {1}", ex.ToString(), ex.InnerException.ToString());
                }
            }

            return points;
        }

        public Boolean IsValidTransaxCardNumber(String CardNumber)
        {
            //Validate length of card number
            if (CardNumber.Length != 17)
                return false;

            //Validate IIN
            if (CardNumber.Substring(0, 6) != "639569")
                return false;

            //Validate security (CDV)
            long card_number = Convert.ToInt64(CardNumber);
            long modulo_tocheck = card_number % 10;
            long modulo_value = Convert.ToInt32(CardNumber.Substring(16, 1));
            if (modulo_tocheck != modulo_value)
                return false;

            return true;
        }

        //public IMSCard GetAssignableCard(long programId, bool isVirtualCard) 
        //{
        //    IMSCard assignableCard = context.IMSCards.Where(x => x.ProgramID == programId && x.IsVirtual == isVirtualCard && x.AssignDate == null && x.CardStatusId == (int) IMSCardStatus.INVENTORY).FirstOrDefault();
        //    if (assignableCard == null)
        //        throw new ApplicationException("No assignable card found in inventory!");

        //    return assignableCard;
        //}

        //public async Task<IMSCard> AssignVirtualCard(long memberId, long programId, int? expirationInMonth = 0)
        //{
        //    #region Declaration Section

        //    DateTime now = DateTime.Now;
        //    IMSCard assignedCard = null;

        //    #endregion

        //    #region Validation Section

        //    IMSCard alreadyAssigned = await context.IMSCards.Where(a => a.MemberId == memberId && a.ProgramID == programId && a.TransaxId != null && a.IsVirtual == true && (a.CardStatusId == (int)IMSCardStatus.ASSIGNED || a.CardStatusId == (int)IMSCardStatus.ACTIVATED)).FirstOrDefaultAsync();

        //    Data.Member member = await context.Members.FirstOrDefaultAsync(a => a.Id == memberId);

        //    if (member == null)
        //    {
        //        logger.Error(string.Concat("Member not found for Id ", memberId));
        //        throw new Exception(string.Format("Member not found for Id ", memberId));
        //    }

        //    #endregion

        //    #region Assign Card Section

        //    if (alreadyAssigned == null)
        //    {
        //        IMSCard virtualCard = await context.IMSCards.Where(x => x.ProgramID == programId && x.IsVirtual == true && x.MemberId == null && x.CardStatusId == (int)IMSCardStatus.INVENTORY).FirstOrDefaultAsync();

        //        if (virtualCard == null)
        //        {
        //            logger.Error(string.Concat("No assignable virtual card found in inventory for programId ", programId));
        //            throw new Exception(string.Format("No assignable virtual card found in inventory for programId ", programId));
        //        }

        //        //Assignation of card to member
        //        try
        //        {
        //            virtualCard.Member = member;
        //            virtualCard.MemberId = memberId;
        //            virtualCard.CardStatusId = (int)IMSCardStatus.ASSIGNED;
        //            context.Entry(virtualCard).State = System.Data.Entity.EntityState.Modified;
        //            context.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.ErrorFormat("Unable to assign card {0} to memberId {1}. Exception : {2}", virtualCard.CardNumber, memberId, ex.ToString());
        //            throw new Exception(string.Format("Unable to assign card {0} to memberId {1}. Exception : {2}", virtualCard.CardNumber, memberId, ex.ToString()));
        //        }

        //        var command = DataCommandFactory.AddCardNonFinancialCommand(virtualCard, member.TransaxId, (int)IMSCardStatus.ASSIGNED, context);

        //        var addCardresult = await command.Execute();

        //        if (addCardresult != DataCommandResult.Success)
        //        {
        //            logger.ErrorFormat("Unable to assign card {0} to memberId {1}. Exception : {2}", virtualCard.CardNumber, memberId, addCardresult.ToString());
        //            throw new Exception(string.Format("Unable to assign card {0} to memberId {1}. Exception : {2}", virtualCard.CardNumber, memberId, addCardresult.ToString()));
        //        }

        //        assignedCard = virtualCard;
        //    }
        //    else
        //    {
        //        assignedCard = alreadyAssigned;
        //    }

        //    #endregion

        //    #region Membership Section

        //    IMSMembership membership = context.IMSMemberships.Where(a => a.MemberID == memberId && a.IsActive == true).FirstOrDefault();

        //    if (membership == null)
        //    {
        //        membership = new IMSMembership();
        //        membership.Member = member;
        //        membership.MemberID = member.Id;
        //        membership.Program = assignedCard.Program;
        //        membership.ProgramID = assignedCard.ProgramID;
        //        membership.ExpiryDate = new ProgramManager().GetExpiryDateWithProgram(assignedCard.ProgramID, now);
        //        membership.RenewalNotificationDate = membership.ExpiryDate.Value.AddMonths(-1);
        //        membership.IsActive = true;
        //        membership.NoShipping = true;

        //        try
        //        {
        //            int pointBalance = assignedCard.PrefixPoints.HasValue ? assignedCard.PrefixPoints.Value : 0;

        //            var command2 = DataCommandFactory.AddMembershipCommand(membership, member.TransaxId, pointBalance, context);

        //            var addMembership = await command2.Execute();

        //            if (addMembership != DataCommandResult.Success)
        //            {
        //                logger.Error(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, assignedCard.CardNumber, addMembership.ToString()));
        //                throw new Exception(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, assignedCard.CardNumber, addMembership.ToString()));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, assignedCard.CardNumber, ex.ToString()));
        //            throw new Exception(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, assignedCard.CardNumber, ex.ToString()));
        //        }
        //    }

        //    #endregion

        //    #region Card Activation Section

        //    try
        //    {
        //        if (alreadyAssigned == null)
        //        {
        //            assignedCard.AssignDate = now;
        //            assignedCard.ShippingDate = now;
        //            assignedCard.ModificationDate = now;
        //            assignedCard.ActivationDate = now;
        //            assignedCard.ModificationDate = now;
        //            assignedCard.NoShipping = true;
        //            assignedCard.CardStatusId = (int)IMSCardStatus.ACTIVATED;

        //            if (expirationInMonth > 0)
        //            {
        //                DateTime expirationDate = Convert.ToDateTime(now).AddMonths(assignedCard.Program.ExpirationInMonth);
        //                assignedCard.ExpiryDate = expirationDate;
        //                assignedCard.RenewalNotificationDate = expirationDate.AddMonths(-1);
        //            }

        //            var command3 = DataCommandFactory.UpdateCardNonFinancialCommand(assignedCard, member.TransaxId, (int)IMSCardStatus.ACTIVATED, context);

        //            var activateCardresult = await command3.Execute();

        //            if (activateCardresult != DataCommandResult.Success)
        //            {
        //                logger.ErrorFormat("AssignVirtualCard - UpdateCardNonFinancialCommand. Card number {0} - Exception {1}", assignedCard.CardNumber, activateCardresult.ToString());
        //                throw new Exception(string.Format("AssignVirtualCard - UpdateCardNonFinancialCommand. Card number {0} - Exception {1}", assignedCard.CardNumber, activateCardresult.ToString()));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("AssignVirtualCard - Update entered card values. Card number {0} - Exception {1}", assignedCard.CardNumber, ex.ToString());
        //        return null;
        //    }

        //    #endregion

        //    return assignedCard;
        //}

        //public async Task<IMSCard> AssignVirtualCard(long memberId, long programId, int? expirationInMonth = 0) 
        //{
        //    #region Declaration Section

        //    DateTime now = DateTime.Now;

        //    #endregion

        //    #region Validation Section

        //    IMSCard alreadyAssigned = context.IMSCards.Where(a => a.MemberId == memberId && a.ProgramID == programId && a.TransaxId != null && a.IsVirtual == true && (a.CardStatusId == (int)IMSCardStatus.ASSIGNED || a.CardStatusId == (int)IMSCardStatus.ACTIVATED)).FirstOrDefault();

        //    if (alreadyAssigned != null)
        //    {
        //        return alreadyAssigned;
        //    }

        //    Data.Member member = context.Members.FirstOrDefault(a => a.Id == memberId);

        //    if (member == null)
        //    {
        //        logger.Error(string.Concat("Member not found for Id ", memberId));
        //        throw new Exception(string.Format("Member not found for Id ", memberId));
        //    }

        //    #endregion

        //    IMSCard virtualCard = context.IMSCards.Where(x => x.ProgramID == programId && x.IsVirtual == true && x.MemberId == null && x.CardStatusId == (int)IMSCardStatus.INVENTORY).FirstOrDefault();

        //    if (virtualCard == null)
        //    {
        //        logger.Error(string.Concat("No assignable virtual card found in inventory for programId ", programId));
        //        throw new Exception(string.Format("No assignable virtual card found in inventory for programId ", programId));
        //    }

        //    //Assignation of card to member
        //    try 
        //    {
        //        virtualCard.Member = member;
        //        virtualCard.MemberId = memberId;
        //        virtualCard.CardStatusId = (int)IMSCardStatus.ASSIGNED;
        //        context.Entry(virtualCard).State = EntityState.Modified;
        //        context.SaveChanges();
        //    }
        //    catch (Exception ex) 
        //    {
        //        logger.ErrorFormat("Unable to assign card {0} to memberId {1}. Exception : {2}", virtualCard.CardNumber, memberId, ex.ToString());
        //        throw new Exception(string.Format("Unable to assign card {0} to memberId {1}. Exception : {2}", virtualCard.CardNumber, memberId, ex.ToString()));
        //    }

        //    var command = DataCommandFactory.AddCardNonFinancialCommand(virtualCard, member.TransaxId, (int)IMSCardStatus.ASSIGNED, context);

        //    var addCardresult = await command.Execute();

        //    if (addCardresult == DataCommandResult.Success)
        //    {
        //        #region Membership Section

        //        IMSMembership membership = context.IMSMemberships.Where(a => a.MemberID == memberId && a.IsActive == true).FirstOrDefault();

        //        //Membership do not exist, we create new membership
        //        if (membership == null)
        //        {
        //            membership = new IMSMembership();
        //            membership.MemberID = member.Id;
        //            membership.ProgramID = virtualCard.ProgramID;
        //            membership.ExpiryDate = new ProgramManager().GetExpiryDateWithProgram(virtualCard.ProgramID, now);
        //            membership.RenewalNotificationDate = membership.ExpiryDate.Value.AddMonths(-1);
        //            membership.IsActive = true;
        //            membership.NoShipping = true;

        //            try
        //            {
        //                var command2 = DataCommandFactory.AddMembershipCommand(membership, member.TransaxId, 0, context);

        //                var addMembership = await command2.Execute();

        //                if (addMembership != DataCommandResult.Success)
        //                {
        //                    logger.Error(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, virtualCard.CardNumber, addMembership.ToString()));
        //                    throw new Exception(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, virtualCard.CardNumber, addMembership.ToString()));
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                logger.Error(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, virtualCard.CardNumber, ex.ToString()));
        //                throw new Exception(string.Format("AssignVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, virtualCard.CardNumber, ex.ToString()));
        //            }
        //        }

        //        #endregion

        //        //Activation of the virtual card
        //        try
        //        {
        //            virtualCard.AssignDate = now;
        //            virtualCard.ShippingDate = now;
        //            virtualCard.ModificationDate = now;
        //            virtualCard.ActivationDate = now;
        //            virtualCard.ModificationDate = now;
        //            virtualCard.NoShipping = true;
        //            virtualCard.CardStatusId = (int)IMSCardStatus.ACTIVATED;

        //            if (expirationInMonth > 0)
        //            {
        //                DateTime expirationDate = Convert.ToDateTime(now).AddMonths(virtualCard.Program.ExpirationInMonth);
        //                virtualCard.ExpiryDate = expirationDate;
        //                virtualCard.RenewalNotificationDate = expirationDate.AddMonths(-1);
        //            }

        //            var command2 = DataCommandFactory.UpdateCardNonFinancialCommand(virtualCard, member.TransaxId, (int)IMSCardStatus.ACTIVATED, context);

        //            var activateCardresult = await command2.Execute();

        //            if (activateCardresult != DataCommandResult.Success)
        //            {
        //                logger.ErrorFormat("AssignVirtualCard - UpdateCardNonFinancialCommand. Card number {0} - Exception {1}", virtualCard.CardNumber, activateCardresult.ToString());
        //                throw new Exception(string.Format("AssignVirtualCard - UpdateCardNonFinancialCommand. Card number {0} - Exception {1}", virtualCard.CardNumber, activateCardresult.ToString()));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.ErrorFormat("AssignVirtualCard - Update entered card values. Card number {0} - Exception {1}", virtualCard.CardNumber, ex.ToString());
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        //reverse card assignation
        //        virtualCard.MemberId = null;
        //        virtualCard.CardStatusId = (int)IMSCardStatus.UNASSIGNABLE;
        //        virtualCard.ModificationDate = now;
        //        context.Entry(virtualCard).State = EntityState.Modified;
        //        context.SaveChanges();

        //        logger.ErrorFormat("Error ({0})", "AddCardNonFinancialCommand " + virtualCard.CardNumber);
        //        return null;
        //    }

        //    return virtualCard;
        //}

        //public async Task<IMSMembership> EnrollVirtualCard(long memberId, long virtualCardId) 
        //{
        //    //Get member
        //    Data.Member member = context.Members.FirstOrDefault(a => a.Id == memberId);
        //    if (member == null)
        //    {
        //        logger.Error(string.Format("EnrollVirtualCard - Member not found for Id {0}", memberId));
        //        return null;
        //    }

        //    //Get VirtualCard
        //    IMSCard virtualCard = context.IMSCards.FirstOrDefault(a => a.Id == virtualCardId);
        //    if (virtualCard == null)
        //    {
        //        logger.Error(string.Concat("EnrollVirtualCard - Card not found for Id {0}", virtualCardId));
        //        return null;
        //    }

        //    //Get Membership
        //    IMSMembership membership = context.IMSMemberships.Where(a => a.MemberID == memberId && a.IsActive == true).FirstOrDefault();

        //    //new membership
        //    if (membership == null) 
        //    { 
        //        membership = new IMSMembership();

        //        try
        //        {
        //            membership = await new MembershipManager().AddMembership(virtualCard.ProgramID, Convert.ToInt64(virtualCard.MemberId), null, true);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error(string.Concat("EnrollVirtualCard - AddMembership error for member {0} card {1} exception {2}", memberId, virtualCard.CardNumber, ex.ToString()));
        //            return null;
        //        }
        //    }

        //    if(membership != null)
        //    {
        //        if (membership.TransaxId == null) //not enrolled
        //        {
        //            //IMSMembership membershipToEnroll = context.IMSMemberships.FirstOrDefault(a => a.Id == membership.Id);

        //            //// EnrollUser and update IMSMembership transaxID
        //            //var enrollUserCommand = DataCommandFactory.EnrollMemberCommand(membershipToEnroll, virtualCard, member, context);

        //            //var enrollResult = await enrollUserCommand.Execute();

        //            //if (enrollResult != DataCommandResult.Success)
        //            //{
        //            //    logger.ErrorFormat("EnrollVirtualCard - EnrollMemberCommand error for card {0}", virtualCard.CardNumber);
        //            //    return membership;
        //            //}

        //            try 
        //            {
        //                new EnrollmentService().EnrollmentNewMemberProcess(memberId, membership.Id, null);
        //            }
        //            catch (Exception ex) 
        //            {
        //                logger.ErrorFormat("EnrollVirtualCard - EnrollNewMemberProcess {0}", ex.ToString());
        //            }

        //            return membership;
        //        }
        //        else //already enrolled
        //        {
        //            return membership;
        //        }
        //    }

        //    return membership;
        //}

        //public async Task<IMSMembership> DeleteEnrollMember(IMSMembership membership, Member member) 
        //{
        //    var deleteMembership_command = DataCommandFactory.DeleteEnrollMemberCommand(membership, member, context);
        //    var deleteMembershipResult = await deleteMembership_command.Execute();

        //    if (deleteMembershipResult != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("Error ({0})", "DeleteEnrollMember " + membership.Id);
        //        throw new Exception(string.Concat("Error ({0})", "DeleteEnrollMember " + membership.Id));
        //    }

        //    return membership;
        //}

        //public async Task<IMSCard> DeactivateNonFinancialCard(IMSCard card, string memberId, string cardId, IMSEntities context)
        //{
        //    var deleteNonFinancialCard_command = DataCommandFactory.DeactivateCardNonFinancialCommand(card, (int)IMSCardStatus.EXPIRED, context);
        //    var deleteNonFinancial_Result = await deleteNonFinancialCard_command.Execute();

        //    if (deleteNonFinancial_Result != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("Error ({0})", "DeactivateNonFinancialCard " + card.Id);
        //        throw new Exception(string.Concat("Error ({0})", "DeleteNonFinancialCard " + card.Id));
        //    }

        //    return card;
        //}


        #endregion

    }
}
