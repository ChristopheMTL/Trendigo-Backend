using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IMS.Common.Core.Slack;

namespace IMS.Common.Core.Services
{
    public class EnrollmentService
    {
        IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Add a new Enroll Membership process to the enrollment service
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        /// <param name="oldMembershipId">Old membership identification (Optional)</param>
        //public void EnrollmentNewMemberProcess(long memberId, long nemMembershipId, long? oldMembershipId) 
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();
            
        //    //Add Enrollment
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.EnrollMembership, 1, BatchNumber, memberId, nemMembershipId, oldMembershipId, null);
        //}

        /// <summary>
        /// Add a new Enroll Membership process, Transfer Point process, Delete Old Membership process, Deactivate Old Non Financial Card process and Update Enroll Membership process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        /// <param name="oldMembershipId">Old membership identification (Optional)</param>
        //public void EnrollmentUpgradeProcess(long memberId, long newMembershipId, long? oldMembershipId) 
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

        //    //Add Enrollment
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.EnrollMembership, 1, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //    //Transfer Point
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.TransferPoint, 2, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //    //Delete Old Membership
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.DeleteEnrollMembership, 3, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //    //Deactivate Old Card
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.DeactivateOldNonFinancialCard, 4, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //    //Enroll default card with new membership 
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.UpdateEnrollMembership, 5, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //}

        /// <summary>
        /// Add a new Enroll Membership process, Transfer Point process, Delete Old Membership process and Update Enroll Membership process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        /// <param name="oldMembershipId">Old membership identification (Optional)</param>
        //public void EnrollmentRenewProcess(long memberId, long newMembershipId, long? oldMembershipId) 
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

        //    //Add Enrollment
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.EnrollMembership, 1, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //    //Transfer Point
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.TransferPoint, 2, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //    //Delete Old Membership
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.DeleteEnrollMembership, 3, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //    //Enroll default card 
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.UpdateEnrollMembership, 4, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //}

        /// <summary>
        /// Add a new Disable Enroll Membership process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        //public void EnrollmentExpiredProcess(long memberId, long newMembershipId) 
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();
        //    //Disable Membership
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.DisableEnrollMembership, 1, BatchNumber, memberId, newMembershipId, null, null);
        //}

        /// <summary>
        /// Add a new Update Enroll Membership process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        //public void EnrollmentNewDefaultCreditCardProcess(long memberId, long newMembershipId) 
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

        //    //Enroll Credit Card
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.UpdateEnrollMembership, 1, BatchNumber, memberId, newMembershipId, null, null);
        //}

        /// <summary>
        /// Add a new Apply Promo Code process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        /// <param name="promoCodeId">PromoCode identification</param>
        //public void EnrollmentApplyPromoCodeProcess(long memberId, long newMembershipId, long promoCodeId)
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.ApplyPromoCode, 1, BatchNumber, memberId, newMembershipId, null, promoCodeId);
        //}

        /// <summary>
        /// Add a new Apply Promo Code process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        /// <param name="promoCodeId">PromoCode identification</param>
        public void EnrollmentMonthlyBonusPointProcess(long memberId, long newMembershipId, long monthlyBonusPointId)
        {
            String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

            AddEnrollmentProcess((int)EnrollmentProcessEnum.MonthlyBonusPoint, 1, BatchNumber, memberId, newMembershipId, null, monthlyBonusPointId);
        }

        /// <summary>
        /// Add a new Apply Promo Code process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        /// <param name="promoCodeId">PromoCode identification</param>
        //public void EnrollmentApplyPrefixPointProcess(long memberId, long newMembershipId, long cardId)
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.ApplyPrefixPoints, 1, BatchNumber, memberId, newMembershipId, null, cardId);
        //}

        /// <summary>
        /// Add a new Transfer Point process
        /// </summary>
        /// <param name="memberId">Member identification</param>
        /// <param name="nemMembershipId">New membership identification</param>
        /// <param name="oldMembershipId">Old membership identification (Optional)</param>
        //public void EnrollmentTransferPointsProcess(long memberId, long newMembershipId, long? oldMembershipId)
        //{
        //    String BatchNumber = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

        //    //Transfer Point
        //    AddEnrollmentProcess((int)EnrollmentProcessEnum.TransferPoint, 2, BatchNumber, memberId, newMembershipId, oldMembershipId, null);
        //}

        #region Private Section

        /// <summary>
        /// Add the enrollment process to the database
        /// </summary>
        /// <param name="enrollmentProcessId">Enrollment Process identification</param>
        /// <param name="orderId">Order Process identification</param>
        /// <param name="batchNumber">Batch Number for that enrollment</param>
        /// <param name="memberId">Member identification</param>
        /// <param name="newMembershipId">New membership identification</param>
        /// <param name="oldMembershipId">Old membership identification (Optional)</param>
        private void AddEnrollmentProcess(int enrollmentProcessId, int orderId, string batchNumber, long memberId, long newMembershipId, long? oldMembershipId, long? otherId)
        {
            Enrollment enrollmentExist = db.Enrollments.FirstOrDefault(a => a.MemberId == memberId && a.EnrollmentProcessId == enrollmentProcessId && a.NewMembershipId == newMembershipId && a.OldMembershipId == oldMembershipId && a.EnrollmentStatusId == (int)EnrollmentStatus.READY && enrollmentProcessId != (int)EnrollmentProcessEnum.UpdateEnrollMembership);

            if (enrollmentExist == null)
            {
                Enrollment newEnrollment = new Enrollment();

                newEnrollment.EnrollmentProcessId = enrollmentProcessId;
                newEnrollment.OrderId = orderId;
                newEnrollment.BatchNumber = batchNumber;
                newEnrollment.MemberId = memberId;
                newEnrollment.NewMembershipId = newMembershipId;
                newEnrollment.OldMembershipId = oldMembershipId;
                newEnrollment.OtherId = otherId;
                newEnrollment.EnrollmentStatusId = (int)EnrollmentStatus.READY;
                newEnrollment.CreationDate = DateTime.Now;
                newEnrollment.ModificationDate = DateTime.Now;

                try
                {
                    db.Enrollments.Add(newEnrollment);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Unable to save enrollment process id {0} memberId {1} Exception {2}", enrollmentProcessId, memberId, ex.ToString()));
                }
            }
        }

        /// <summary>
        /// Increment the enrollment process since it fails. Maximum possible try is 5.
        /// </summary>
        /// <param name="enrollmentId">Identifier of the enrollment process</param>
        /// <returns>No returned value</returns>
        private async Task IncrementEnrollment(long enrollmentId)
        {
            Enrollment enrollment = db.Enrollments.FirstOrDefault(a => a.Id == enrollmentId);

            if (enrollment.EnrollmentSequenceNumber == 5)
            {
                await ErrorEnrollment(enrollmentId);
            }

            if (enrollment.EnrollmentSequenceNumber < 5)
            {
                enrollment.EnrollmentSequenceNumber += 1;
                enrollment.ModificationDate = DateTime.Now;
                db.Entry(enrollment).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
            }

        }

        /// <summary>
        /// Change the actual status of enrollment to ERROR. This enrollment process will not execute neither the process that are following for the same batch number.
        /// </summary>
        /// <param name="enrollmentId">Identifier of the enrollment process</param>
        /// <returns>No returned value</returns>
        private async Task ErrorEnrollment(long enrollmentId)
        {
            Enrollment enrollment = db.Enrollments.FirstOrDefault(a => a.Id == enrollmentId);

            enrollment.EnrollmentStatusId = (int)EnrollmentStatus.ERROR;
            enrollment.ModificationDate = DateTime.Now;
            db.Entry(enrollment).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Change the actual status of enrollment to PROCESSED. Will also include the Enrollment process response if any was provided. 
        /// </summary>
        /// <param name="enrollmentId">Identifier of the enrollment process</param>
        /// <param name="response">Response returned from the enrollment process</param>
        /// <returns>No returned value</returns>
        private async Task SuccessEnrollment(long enrollmentId, string response)
        {
            Enrollment enrollment = db.Enrollments.FirstOrDefault(a => a.Id == enrollmentId);

            enrollment.EnrollmentStatusId = (int)EnrollmentStatus.PROCESSED;
            enrollment.ModificationDate = DateTime.Now;
            enrollment.EnrollmentProcessResponse = response;
            db.Entry(enrollment).State = System.Data.Entity.EntityState.Modified;
            try 
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
            
        }

        #endregion

        #region Service Section

        //public async Task EnrollMembership(Enrollment enrollment, IMSEntities db)
        //{
        //    #region Validation Section

        //    if (enrollment.EnrollmentProcessId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - EnrollNewMember - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = await db.Members.FirstOrDefaultAsync(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - EnrollNewMember - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership newMembership = await db.IMSMemberships.FirstOrDefaultAsync(a => a.Id == enrollment.NewMembershipId);

        //    if (newMembership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - EnrollNewMember - New Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.NewMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSCard card = member.IMSCards.Where(a => (a.CardStatusId == (int)IMSCardStatus.ACTIVATED && a.IsVirtual == true) || (a.CardStatusId == (int)IMSCardStatus.SHIPPED && a.IsVirtual == false)).FirstOrDefault();

        //    if (card == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - EnrollNewMember - Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    #endregion

        //    var enrollUserCommand = DataCommandFactory.EnrollMemberCommand(newMembership, card, member, db);

        //    var enrollResult = await enrollUserCommand.Execute();

        //    if (enrollResult != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EnrollmentService - EnrollNewMember error for card {0}", card.CardNumber);
        //        await IncrementEnrollment(enrollment.Id);
        //        return;
        //    }

        //    //logger.DebugFormat("EnrollmentService - EnrollNewMember done membershipId {0} cardId {1} memberId {2}", newMembership.Id, card.Id, member.Id);

        //    CreditCard defaultCreditCard = newMembership.Member.CreditCards.FirstOrDefault(a => a.defaultCard == true);

        //    //Set card linked
        //    if (defaultCreditCard != null) 
        //    {
        //        defaultCreditCard.IsLinked = true;
        //        db.Entry(defaultCreditCard).State = System.Data.Entity.EntityState.Modified;

        //        try 
        //        {
        //            await db.SaveChangesAsync();
        //        }
        //        catch (Exception ex) 
        //        {
        //            logger.ErrorFormat("EnrollmentService - Unable to set isLinked to cardId {0} - Exception {1}", defaultCreditCard.Id, ex.ToString());

        //            List<MessageParam> parameters = new List<MessageParam>();
        //            MessageParam param = new MessageParam("cardId", defaultCreditCard.Id.ToString());
        //            parameters.Add(param);

        //            new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "EnrollMembership - Unable to set isLinked to default credit card", parameters);
        //        }
        //    }

        //    //Request physical card

        //    if (card.IsVirtual == true && card.Member.Address.Longitude != 0 && card.Member.Address.Latitude != 0) 
        //    {
        //        try
        //        {
        //            DeviceRequest request = new DeviceRequestService().AddDeviceRequest(newMembership.MemberID, newMembership.Id, (int)DeviceTypeEnum.Card);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.ErrorFormat("EnrollmentService - Unable to request physical card for memberId {0} - Exception {1}", newMembership.MemberID, ex.ToString());

        //            List<MessageParam> parameters = new List<MessageParam>();
        //            MessageParam param = new MessageParam("memberId", newMembership.MemberID.ToString());
        //            parameters.Add(param);

        //            new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "EnrollMembership - Unable to request physical card for member", parameters);
        //        }
        //    }

        //    await SuccessEnrollment(enrollment.Id, newMembership.TransaxId);

        //}

        //public async Task TransferPoints(Enrollment enrollment, IMSEntities db)
        //{

        //    #region Validation Section

        //    if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - EnrollNewMember - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = db.Members.FirstOrDefault(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSCard newCard = member.IMSCards.OrderByDescending(a => a.ActivationDate).Where(a => a.CardStatusId == (int)IMSCardStatus.ACTIVATED).FirstOrDefault();

        //    if (newCard == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - New Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSCard actualCard = member.IMSCards.OrderByDescending(a => a.ActivationDate).Where(a => a.Id != newCard.Id).FirstOrDefault();

        //    if (actualCard == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - Old Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    if (enrollment.OldMembershipId == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - Old MembershipId not present : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership oldMembership = db.IMSMemberships.FirstOrDefault(a => a.Id == enrollment.OldMembershipId);


        //    if (oldMembership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - Old Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.OldMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership newMembership = db.IMSMemberships.FirstOrDefault(a => a.Id == enrollment.NewMembershipId);

        //    if (newMembership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - New Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.NewMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    #endregion

        //    Int32 pointBalance_actualCard = await new CardManager().GetNonFinancialCardPointBalance(member.TransaxId, actualCard.TransaxId, oldMembership.Program.TransaxId);

        //    CardPointHistory oldCardHistory = new CardPointHistory();
        //    CardPointHistory newCardHistory = new CardPointHistory();
        //    CardPointHistory transferCardHistory = new CardPointHistory();
        //    IMS.Common.Core.Data.IMSUser ims_user = new RegistrationManager().GetUserWithTransaxId(ConfigurationManager.AppSettings["IMSUserID"]);

        //    //If same program, transfer from old membership to new membership
        //    if (actualCard.ProgramID == newCard.ProgramID) 
        //    {
        //        transferCardHistory.IMSCard = actualCard;
        //        transferCardHistory.IMSCardId = actualCard.Id;
        //        transferCardHistory.Points = pointBalance_actualCard;
        //        transferCardHistory.Reason = CardPointHistoryReason.CardTransferPointBalance.ToString() + " transfer " + pointBalance_actualCard + " pts from " + oldMembership.TransaxId + " to " + newMembership.TransaxId;
        //        transferCardHistory.CreatedBy = ims_user.Id;

        //        var command_transfer = DataCommandFactory.TransferPointsCardNonFinancialCommand(transferCardHistory, oldMembership.TransaxId, newMembership.TransaxId, pointBalance_actualCard, member, db);

        //        var result_transfer = await command_transfer.Execute();

        //        if (result_transfer != DataCommandResult.Success) 
        //        {
        //            logger.ErrorFormat("EnrollmentService - TransferPoint - Unable to transfer point from membership {0} to membership {1} total points {2}", oldMembership.Id, newMembership.Id, pointBalance_actualCard);
        //            await IncrementEnrollment(enrollment.Id);
        //        }

        //        //logger.DebugFormat("EnrollmentService - TransferPoint done oldMembershipId {0} newMembershipId {1} points {2} memberId {3}", oldMembership.Id, newMembership.Id, pointBalance_actualCard, member.Id);

        //        await SuccessEnrollment(enrollment.Id, transferCardHistory.TransaxId);
        //    }
        //    else //If different program, remove from old program and add to new program
        //    {
        //        //if (pointBalance_actualCard > 0)
        //        //{
        //        //    //Remove point from oldMembership
        //        //    oldCardHistory.IMSCard = actualCard;
        //        //    oldCardHistory.IMSCardId = actualCard.Id;
        //        //    oldCardHistory.Points = (pointBalance_actualCard * -1);
        //        //    oldCardHistory.Reason = CardPointHistoryReason.CardTransferPointBalance.ToString() + "removing " + pointBalance_actualCard + " pts from " + oldMembership.TransaxId;
        //        //    oldCardHistory.CreatedBy = ims_user.Id;

        //        //    var command_remove = DataCommandFactory.RemovePointsCardNonFinancialCommand(oldCardHistory, pointBalance_actualCard, member, db);

        //        //    var result_remove = await command_remove.Execute();

        //        //    if (result_remove == DataCommandResult.Success)
        //        //    {
        //        //        //Add point to newMembership
        //        //        CardPointHistory cardHistory = new CardPointHistoryService().AddCardPointHistory(newCard.Id, pointBalance_actualCard, CardPointHistoryReason.CardTransferPointBalance.ToString() + " from " + oldMembership.TransaxId + " To " + newMembership.TransaxId, "0", ims_user.Id, this.db);

        //        //        newCardHistory.IMSCardId = newCard.Id;
        //        //        newCardHistory.Points = pointBalance_actualCard;
        //        //        newCardHistory.Reason = CardPointHistoryReason.CardTransferPointBalance.ToString() + "adding " + pointBalance_actualCard + " pts To " + newMembership.TransaxId;
        //        //        newCardHistory.CreatedBy = ims_user.Id;

        //        //        var command_add = DataCommandFactory.AddPointsCardNonFinancialCommand(newCardHistory, pointBalance_actualCard, member, db);

        //        //        var result_add = await command_add.Execute();

        //        //        if (result_add != DataCommandResult.Success)
        //        //        {
        //        //            //Rollback points to oldMembership
        //        //            //oldCardHistory = new CardPointHistoryService().AddCardPointHistory(actualCard.Id, pointBalance_actualCard, CardPointHistoryReason.CardTransferPointBalance.ToString() + " ROLLBACK from " + oldMembership.TransaxId + " To " + newMembership.TransaxId, "0", ims_user.Id, this.db);

        //        //            //var command_rollback = DataCommandFactory.AddPointsCardNonFinancialCommand(oldCardHistory, pointBalance_actualCard, member, db);

        //        //            //var result_rollback = await command_rollback.Execute();

        //        //            //await IncrementEnrollment(enrollment.Id);

        //        //            //if (result_rollback != DataCommandResult.Success) 
        //        //            //{
        //        //            logger.ErrorFormat("EnrollmentService - TransferPoint - Unable to add point to contract {0} points {1}", newMembership.TransaxId, pointBalance_actualCard);
        //        //            await IncrementEnrollment(enrollment.Id);
        //        //            //}
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //                logger.ErrorFormat("EnrollmentService - TransferPoint - Unable to transfer point from contract {0} to contract {1} points to rollback {2}", oldMembership.TransaxId, newMembership.TransaxId, pointBalance_actualCard);
        //                await IncrementEnrollment(enrollment.Id);
        //        //    }
        //        //}

        //        //await SuccessEnrollment(enrollment.Id, oldCardHistory.TransaxId + "/" + newCardHistory.TransaxId);
        //    }
        //}

        //public async Task DisableEnrollMembership(Enrollment enrollment, IMSEntities db)
        //{
        //    #region Validation Section

        //    if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DeleteEnrollMembership - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = db.Members.FirstOrDefault(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DeleteEnrollMembership - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership membership = db.IMSMemberships.FirstOrDefault(a => a.Id == enrollment.OldMembershipId);

        //    if (membership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DeleteEnrollMembership - Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.NewMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    #endregion

        //    var command = DataCommandFactory.DisableEnrollMemberCommand(membership, member, db);
        //    var result = await command.Execute();

        //    if (result != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DisableEnrollMembership - Unable to update membership : EnrollmentId {0} MembershipId {1}", enrollment.Id, membership.Id);
        //        await IncrementEnrollment(enrollment.Id);
        //        return;
        //    }

        //    logger.DebugFormat("EnrollmentService - DisableEnrollMember done membershipId {0} memberId {1}", membership.Id, member.Id);

        //    await SuccessEnrollment(enrollment.Id, "");

        //}

        //public async Task DeleteEnrollMembership(Enrollment enrollment)
        //{
        //    #region Validation Section

        //    if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DeleteEnrollMembership - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = db.Members.FirstOrDefault(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DeleteEnrollMembership - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership oldMembership = db.IMSMemberships.FirstOrDefault(a => a.Id == enrollment.OldMembershipId);

        //    if (oldMembership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DeleteEnrollMembership - Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.NewMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    #endregion

        //    var command = DataCommandFactory.DeleteEnrollMemberCommand(oldMembership, member, db);
        //    var result = await command.Execute();

        //    if (result != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EnrollmentService - DeleteEnrollMembership - Unable to delete membership : EnrollmentId {0} MembershipId {1}", enrollment.Id, oldMembership.Id);
        //        await IncrementEnrollment(enrollment.Id);
        //        return;
        //    }

        //    logger.DebugFormat("EnrollmentService - DeleteEnrollMembership done membershipId {0} memberId {1}", oldMembership.Id, member.Id);

        //    await SuccessEnrollment(enrollment.Id, "");

        //}

        //public async Task DeactivateOldNonFinancialCard(Enrollment enrollment)
        //{
        //    using (IMSEntities db = new IMSEntities())
        //    {
        //        if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //        {
        //            logger.ErrorFormat("EnrollmentService - DeactivateOldNonFinancialCard - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //            await ErrorEnrollment(enrollment.Id);
        //            return;
        //        }

        //        Member member = db.Members.FirstOrDefault(a => a.Id == enrollment.MemberId);

        //        if (member == null)
        //        {
        //            logger.ErrorFormat("EnrollmentService - DeactivateOldNonFinancialCard - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //            await ErrorEnrollment(enrollment.Id);
        //            return;
        //        }

        //        if (member.IMSCards.Where(a => a.CardStatusId == (int)IMSCardStatus.ACTIVATED).Count() < 2)
        //        {
        //            //logger.ErrorFormat("EnrollmentService - DeactivateOldNonFinancialCard - Only One Card Active : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //            //No card to deactivate, old card was probably reported stolen or lost
        //            await SuccessEnrollment(enrollment.Id, "");
        //            return;
        //        }

        //        IMSCard oldCard = member.IMSCards.Where(a => a.CardStatusId == (int)IMSCardStatus.ACTIVATED).OrderBy(b => b.AssignDate).FirstOrDefault();

        //        if (oldCard == null)
        //        {
        //            logger.ErrorFormat("EnrollmentService - DeactivateOldNonFinancialCard - Old Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //            await ErrorEnrollment(enrollment.Id);
        //            return;
        //        }

        //        var command = DataCommandFactory.DeactivateCardNonFinancialCommand(oldCard, member, (int)IMSCardStatus.UNUSABLE, db);
        //        var result = await command.Execute();

        //        if (result != DataCommandResult.Success)
        //        {
        //            logger.ErrorFormat("EnrollmentService - DeactivateOldNonFinancialCard - Unable to deactivate card : EnrollmentId {0} CardId {1}", enrollment.Id, oldCard.Id);
        //            await IncrementEnrollment(enrollment.Id);
        //            return;
        //        }

        //        logger.DebugFormat("EnrollmentService - DeactivateOldNonFinancialCard done cardId {0} memberId {1}", oldCard.Id, member.Id);

        //        await SuccessEnrollment(enrollment.Id, "");

        //    }
        //}

        //public async Task UpdateEnrollMembership(Enrollment enrollment, IMSEntities db)
        //{
            
        //    if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - UpdateEnrollMembership - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = db.Members.FirstOrDefault(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - UpdateEnrollMembership - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership newMembership = db.IMSMemberships.FirstOrDefault(a => a.Id == enrollment.NewMembershipId);

        //    if (newMembership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - UpdateEnrollMembership - New Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.NewMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    CreditCard defaultCreditCard = db.CreditCards.FirstOrDefault(a => a.MemberId == enrollment.MemberId && a.IsActive == true && a.defaultCard == true);

        //    if (defaultCreditCard == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - UpdateEnrollMembership - Default Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    var command = DataCommandFactory.UpdateEnrollMemberCommand(newMembership, member, defaultCreditCard, db);

        //    var result = await command.Execute();

        //    if (result != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EnrollmentService - UpdateEnrollMembership : EnrollmentId {0} memberId {1}", enrollment.Id, enrollment.MemberId);
        //        await IncrementEnrollment(enrollment.Id);
        //        return;
        //    }

        //    logger.DebugFormat("EnrollmentService - UpdateEnrollMembership done enrollmentId {0} memberId {1}", enrollment.Id, member.Id);

        //    if (defaultCreditCard.IsLinked == false)
        //    {
        //        defaultCreditCard.IsLinked = true;
        //        db.Entry(defaultCreditCard).State = System.Data.Entity.EntityState.Modified;
        //        await db.SaveChangesAsync();
        //    }

        //    await SuccessEnrollment(enrollment.Id, newMembership.TransaxId);

        //}

        //public async Task ApplyPromoCode(Enrollment enrollment, IMSEntities db)
        //{
        //    #region Declaration Section

        //    IMS.Common.Core.Data.IMSUser ims_user = new RegistrationManager().GetUserWithTransaxId(ConfigurationManager.AppSettings["IMSUserID"]);

        //    #endregion

        //    #region Validation Section

        //    if (ims_user == null) 
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPromoCode - ims_user not found : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPromoCode - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = await db.Members.FirstOrDefaultAsync(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPromoCode - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSCard newCard = member.IMSCards.OrderByDescending(a => a.ActivationDate).Where(a => a.CardStatusId == (int)IMSCardStatus.ACTIVATED).FirstOrDefault();

        //    if (newCard == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPromoCode - New Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    PromoCode promoCode = await db.PromoCodes.FirstOrDefaultAsync(a => a.Id == enrollment.OtherId);

        //    if (promoCode == null) 
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPromoCode - PromoCode not found : EnrollmentId {0} MembershipId {1} PromoCodeId {2}", enrollment.Id, enrollment.NewMembershipId, enrollment.OtherId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    string reason = (promoCode.FromGiftCard == null || promoCode.FromGiftCard == false ? "Website Promocode " : "Giftcard Promocode ") + promoCode.Code;

        //    #endregion

        //    CardPointHistory cph = new CardPointHistory();
        //    cph.CreatedBy = ims_user.Id;
        //    cph.CreatedDate = DateTime.Now;
        //    cph.IMSCardId = newCard.Id;
        //    cph.Points = promoCode.PrefixPoints;
        //    cph.PromoCodeId = promoCode.Id;
        //    cph.Reason = reason;

        //    var command_transfer = DataCommandFactory.ApplyPromoCodeCommand(cph, promoCode.PrefixPoints, member.Id, db);

        //    var result_transfer = await command_transfer.Execute();

        //    if (result_transfer != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPromoCode - Unable to apply point from promocodeId {0} to member {1} total points {2}", promoCode.Id, member.Id, promoCode.PrefixPoints);
        //        await IncrementEnrollment(enrollment.Id);

        //        List<MessageParam> parameters = new List<MessageParam>();
        //        MessageParam param = new MessageParam("promocodeId", promoCode.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("memberId", member.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("points", promoCode.PrefixPoints.ToString());
        //        parameters.Add(param);

        //        new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "ApplyPromoCode - Unable to apply point from promocode", parameters);
        //    }

        //    CardPointHistory _cph = await db.CardPointHistories.FirstOrDefaultAsync(a => a.IMSCardId == newCard.Id && a.PromoCodeId == promoCode.Id);

        //    //If cph is null this means that the promocode was not applied and we need to add an error in the enrollment process
        //    if (_cph == null) 
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPromoCode - PromoCode not applied : EnrollmentId {0} MembershipId {1} PromoCodeId {2}", enrollment.Id, enrollment.NewMembershipId, enrollment.OtherId);
        //        await IncrementEnrollment(enrollment.Id);

        //        List<MessageParam> parameters = new List<MessageParam>();
        //        MessageParam param = new MessageParam("enrollmentId", enrollment.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("membershipId", enrollment.NewMembershipId.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("promocodeId", enrollment.OtherId.ToString());
        //        parameters.Add(param);

        //        new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "ApplyPromoCode - PromoCode not applied", parameters);
        //        return;
        //    }

        //    logger.DebugFormat("EnrollmentService - ApplyPromoCode done promoCodeId {0} memberId {1} points {2}", promoCode.Id, member.Id, promoCode.PrefixPoints);
        //    await SuccessEnrollment(enrollment.Id, _cph.TransaxId);

        //    #region Add MemberPromoCodeHistory section

        //    try
        //    {
        //        MemberPromoCodeHistory newMemberPromoCodeHistory = new MemberPromoCodeHistory();
        //        newMemberPromoCodeHistory.MemberId = member.Id;
        //        newMemberPromoCodeHistory.PromocodeId = promoCode.Id;
        //        newMemberPromoCodeHistory.CardId = newCard.Id;
        //        newMemberPromoCodeHistory.CardPointHistoryId = _cph.Id;
        //        newMemberPromoCodeHistory.CreationDate = DateTime.Now;

        //        db.MemberPromoCodeHistories.Add(newMemberPromoCodeHistory);
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("Unable to add MemberPromoCodeHistory. memberId {0} cardId {1} code {2}", member.Id, newCard.Id, promoCode.Code);
        //        logger.ErrorFormat("Exception", ex.ToString());
        //        logger.ErrorFormat("InnerException", ex.InnerException.ToString());

        //        List<MessageParam> parameters = new List<MessageParam>();
        //        MessageParam param = new MessageParam("memberId", member.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("cardId", newCard.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("promocode", promoCode.Code);
        //        parameters.Add(param);

        //        new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "ApplyPromoCode - Unable to add MemberPromoCodeHistory", parameters);
        //    }

        //    #endregion

        //    #region PromoCode Incrementation Section

        //    try
        //    {
        //        promoCode.AlreadyUsed += 1;
        //        db.Entry(promoCode).State = System.Data.Entity.EntityState.Modified;
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("Unable to update already used promo code. memberId {0} cardId {1} code {2}", member.Id, newCard.Id, promoCode.Code);
        //        logger.ErrorFormat("Exception", ex.ToString());
        //        logger.ErrorFormat("InnerException", ex.InnerException.ToString());

        //        List<MessageParam> parameters = new List<MessageParam>();
        //        MessageParam param = new MessageParam("memberId", member.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("cardId", newCard.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("promocode", promoCode.Code);
        //        parameters.Add(param);

        //        new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "ApplyPromoCode - Unable to update already used promo code", parameters);
        //    }

        //    #endregion
            
        //}

        //public async Task ApplyMonthlyBonusPoints(Enrollment enrollment, IMSEntities db)
        //{

        //    #region Validation Section

        //    if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - EnrollNewMember - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = db.Members.FirstOrDefault(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSCard card = member.IMSCards.OrderByDescending(a => a.ActivationDate).Where(a => a.CardStatusId == (int)IMSCardStatus.ACTIVATED).FirstOrDefault();

        //    if (card == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - New Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership newMembership = db.IMSMemberships.FirstOrDefault(a => a.Id == enrollment.NewMembershipId);

        //    if (newMembership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - TransferPoints - New Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.NewMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    if (enrollment.OtherId == null) 
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyMonthlyBonusPoints - MonthlyBonusPoint not found : EnrollmentId {0} MonthlyBonusPointId {1}", enrollment.Id, enrollment.OtherId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    long id = Convert.ToInt64(enrollment.OtherId);

        //    MonthlyBonusPoint mbp = db.MonthlyBonusPoints.FirstOrDefault(a => a.Id == id);

        //    if (mbp == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyMonthlyBonusPoints - MonthlyBonusPoint not found : EnrollmentId {0} MonthlyBonusPointId {1}", enrollment.Id, enrollment.OtherId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    #endregion
            
        //    IMS.Common.Core.Data.IMSUser ims_user = new RegistrationManager().GetUserWithTransaxId(ConfigurationManager.AppSettings["IMSUserID"]);

        //    CardPointHistory cph = new CardPointHistory();
        //    cph.CreatedDate = DateTime.Now;
        //    cph.IMSCardId = card.Id;
        //    cph.Points = mbp.Points;
        //    cph.Reason = CardPointHistoryReason.MonthlyBonusPoints.ToString() + " " + mbp.Points + " pts applied";
        //    cph.CreatedBy = ims_user.Id;

        //    var command = DataCommandFactory.TransferPointsCardNonFinancialCommand(cph, "0", newMembership.TransaxId, mbp.Points, member, db);

        //    var result = await command.Execute();

        //    if (result != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyMonthlyBonusPoints - Unable to add point to membership {0} total points {1}", newMembership.Id, mbp.Points);
        //        await ErrorEnrollment(enrollment.Id);
                
        //        List<MessageParam> parameters = new List<MessageParam>();
        //        MessageParam param = new MessageParam("membershipId", newMembership.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("points", mbp.Points.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("EnrollmentId", enrollment.Id.ToString());
        //        parameters.Add(param);

        //        new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "ApplyMonthlyBonusPoints - Unable to add point to membership", parameters);
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(cph.TransaxId)) 
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyMonthlyBonusPoints - Unable to add point to membership {0} total points {1} Eko-Pay Id not returned", newMembership.Id, mbp.Points);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    await SuccessEnrollment(enrollment.Id, cph.TransaxId);
            

        //    mbp.TransaxId = cph.TransaxId;
        //    mbp.CommandStatusId = (int)CommandStatusEnum.Success;
        //    mbp.ModificationDate = DateTime.Now;

        //    try
        //    {
        //        db.Entry(mbp).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyMonthlyBonusPoints InternalServerError : mbpId {0} Exception {1} InnerException {2}", mbp.Id, ex.ToString(), ex.InnerException.ToString());

        //        List<MessageParam> parameters = new List<MessageParam>();
        //        MessageParam param = new MessageParam("membershipId", newMembership.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("Id", mbp.Id.ToString());
        //        parameters.Add(param);

        //        new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "ApplyMonthlyBonusPoints - InternalServerError", parameters);
        //        return;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        //public async Task ApplyPrefixPoints(Enrollment enrollment, IMSEntities db)
        //{

        //    #region Validation Section

        //    if (enrollment.EnrollmentStatusId != (int)EnrollmentStatus.READY)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - Enrollment not ready : EnrollmentId {0}", enrollment.Id);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    Member member = db.Members.FirstOrDefault(a => a.Id == enrollment.MemberId);

        //    if (member == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - Member not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    if (enrollment.OtherId == null) 
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSCard card = member.IMSCards.FirstOrDefault(a => a.Id == enrollment.OtherId);

        //    if (card == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - Card not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    if (card.PrefixPoints == null) 
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - Prefix point not found : EnrollmentId {0} MemberId {1}", enrollment.Id, enrollment.MemberId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    IMSMembership newMembership = db.IMSMemberships.FirstOrDefault(a => a.Id == enrollment.NewMembershipId);

        //    if (newMembership == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - New Membership not found : EnrollmentId {0} MembershipId {1}", enrollment.Id, enrollment.NewMembershipId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    if (enrollment.OtherId == null)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - MonthlyBonusPoint not found : EnrollmentId {0} MonthlyBonusPointId {1}", enrollment.Id, enrollment.OtherId);
        //        await ErrorEnrollment(enrollment.Id);
        //        return;
        //    }

        //    #endregion

        //    IMS.Common.Core.Data.IMSUser ims_user = new RegistrationManager().GetUserWithTransaxId(ConfigurationManager.AppSettings["IMSUserID"]);

        //    CardPointHistory cph = new CardPointHistory();
        //    cph.CreatedDate = DateTime.Now;
        //    cph.IMSCardId = card.Id;
        //    cph.Points = card.PrefixPoints.Value;
        //    cph.Reason = CardPointHistoryReason.PrefixPoints.ToString();
        //    cph.CreatedBy = ims_user.Id;

        //    var command = DataCommandFactory.TransferPointsCardNonFinancialCommand(cph, "0", newMembership.TransaxId, card.PrefixPoints.Value, member, db);

        //    var result = await command.Execute();

        //    if (result != DataCommandResult.Success)
        //    {
        //        logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - Unable to add point to membership {0} total points {1}", newMembership.Id, card.PrefixPoints.Value);
        //        await IncrementEnrollment(enrollment.Id);

        //        List<MessageParam> parameters = new List<MessageParam>();
        //        MessageParam param = new MessageParam("membershipId", newMembership.Id.ToString());
        //        parameters.Add(param);
        //        param = new MessageParam("points", card.PrefixPoints.Value.ToString());
        //        parameters.Add(param);

        //        new SlackClient().SlackAlert((int)SlackChannelEnum.Enrollment, SlackMessageTypeEnum.danger, "ApplyPrefixPoints - Unable to add point to membership", parameters);
        //        return;
        //    }

        //    //if (string.IsNullOrEmpty(cph.TransaxId))
        //    //{
        //    //    logger.ErrorFormat("EnrollmentService - ApplyPrefixPoints - Unable to add point to membership {0} total points {1} Eko-Pay Id not returned", newMembership.Id, card.PrefixPoints.Value);
        //    //    await ErrorEnrollment(enrollment.Id);
        //    //    return;
        //    //}

        //    //logger.DebugFormat("EnrollmentService - ApplyPrefixPoints done newMembershipId {0} points {1}", newMembership.Id, card.PrefixPoints.Value);

        //    await SuccessEnrollment(enrollment.Id, cph.TransaxId);
            
        //}

        #endregion

    }
}
