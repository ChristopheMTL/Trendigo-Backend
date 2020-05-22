using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.DataCommands;

namespace IMS.Common.Core.Services
{
    public class ReferralCampaignService
    {
        private IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This method return the referral campaign with the referral for a member 
        /// </summary>
        /// <param name="memberId">The member identification</param>
        /// <returns>Referral campaign object containing the referrals of the member</returns>
        public async Task<ReferralCampaign> GetReferralCampaign(long memberId) 
        {
            DateTime targetDate = DateTime.Now;

            //Check if member already referred
            List<Referral> referrals = await db.Referrals.Where(a => a.Member.Id == memberId).OrderBy(b => b.CreationDate).ToListAsync();

            if (referrals.Count > 0) 
            {
                targetDate = referrals.FirstOrDefault().CreationDate; 
            }

            //GetCampaign
            ReferralCampaign campaign = await db.ReferralCampaigns.FirstOrDefaultAsync(a => a.StartingDate <= targetDate && a.EndingDate >= targetDate);

            if (campaign != null) 
            {
                campaign.Referrals = null;
                campaign.Referrals = await db.Referrals.Where(a => a.Member.Id == memberId).ToListAsync();
            }

            return campaign;
        }

        /// <summary>
        /// This method return a referral campaign in range for a start date and end date 
        /// </summary>
        /// <param name="startDate">Start date to look for</param>
        /// <param name="endDate">End ddate to look for</param>
        /// <returns>Referral campaign object</returns>
        public async Task<ReferralCampaign> GetReferralCampaign(DateTime startDate, DateTime endDate)
        {
            ReferralCampaign campaign = await db.ReferralCampaigns.FirstOrDefaultAsync(a => (a.StartingDate <= startDate && a.EndingDate >= startDate) || (a.StartingDate <= endDate && a.EndingDate >= endDate));

            return campaign;
        }

        /// <summary>
        /// This method return a Referral Campaign for the actual date
        /// </summary>
        /// <returns>Referral campaign</returns>
        public async Task<ReferralCampaign> GetActualReferralCampaign()
        {
            ReferralCampaign rc = await db.ReferralCampaigns.Where(a => (DbFunctions.TruncateTime(a.StartingDate) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(a.EndingDate) >= DbFunctions.TruncateTime(DateTime.Now))).FirstOrDefaultAsync();

            return rc;
        }

        /// <summary>
        /// This method return the referral for a referred email address
        /// </summary>
        /// <param name="referred_email">The email address that was referred</param>
        /// <returns>Referral</returns>
        public async Task<Referral> GetReferredInfo(string referred_email) 
        {
            Referral referral = await db.Referrals.FirstOrDefaultAsync(a => a.ReferredToEmail == referred_email);

            return referral;
        }

        /// <summary>
        /// This method update a referral by adding the referred member id for a given email address
        /// </summary>
        /// <param name="referrer_memberId">The member id of the referrer</param>
        /// <param name="referred_memberId">The member id of the referred</param>
        /// <param name="referred_email">The email address of the referred</param>
        /// <returns>Referral</returns>
        public async Task<Referral> AddReferredMemberId(long referrer_memberId, long referred_memberId, string referred_email)
        {
            Referral referral = await db.Referrals.FirstOrDefaultAsync(a => a.Member.Id == referrer_memberId && a.ReferredToEmail == referred_email);

            if (referral != null){

                referral.ReferredTo = referred_memberId;
                referral.ModificationDate = DateTime.Now;
                db.Entry(referral).State = EntityState.Modified;

                try 
                {
                    await db.SaveChangesAsync();
                }
                catch(Exception ex) 
                {
                    logger.ErrorFormat("AddReferredMemberId - Unable to add referred memberId - ReferrerId {0} ReferredId {1} Referred Email {2} Exception {3}", referrer_memberId, referred_memberId, referred_email, ex.ToString());
                    return null;
                }
            }

            return referral;
        }

        /// <summary>
        /// This method update the referred information by changing the IsProfileCompleted
        /// </summary>
        /// <param name="referrer_memberId">The referrer member Id</param>
        /// <param name="referred_memberId">The referred member Id</param>
        /// <param name="campaignId">The referral campaign Id</param>
        /// <returns>Referral</returns>
        public async Task<Referral> CompletedReferred(long referrer_memberId, long referred_memberId, int campaignId)
        {
            #region Validation Section

            Referral referral = await db.Referrals.FirstOrDefaultAsync(a => a.ReferredBy == referrer_memberId && a.ReferredTo == referred_memberId);

            if (referral == null)
            {
                logger.ErrorFormat("CompletedReferred - Referral not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("referral not found");
            }

            ReferralCampaign campaign = await db.ReferralCampaigns.FirstOrDefaultAsync(a => a.Id == campaignId);

            if (campaign == null)
            {
                logger.ErrorFormat("CompletedReferred - Campaign not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("campaign not found");
            }

            IMSCard referrer_card = await new CardManager().GetActiveIMSCard(referrer_memberId);

            if (referrer_card == null)
            {
                logger.ErrorFormat("CompletedReferred - Referrer Card not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("referrer card not found");
            }

            IMSCard referred_card = await new CardManager().GetActiveIMSCard(referred_memberId);

            if (referred_card == null)
            {
                logger.ErrorFormat("CompletedReferred - Referred Card not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("referred card not found");
            }

            Member referrer_member = await db.Members.FirstOrDefaultAsync(a => a.Id == referrer_memberId);

            if (referrer_member == null) 
            {
                logger.ErrorFormat("CompletedReferred - Referrer Member not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("referrer member not found");
            }

            Member referred_member = await db.Members.FirstOrDefaultAsync(a => a.Id == referred_memberId);

            if (referred_member == null)
            {
                logger.ErrorFormat("CompletedReferred - Referred Member not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("referred member not found");
            }

            IMSMembership referrer_membership = referrer_member.IMSMemberships.FirstOrDefault(a => a.IsActive);

            if (referrer_membership == null)
            {
                logger.ErrorFormat("CompletedReferred - Referrer Membership not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("referred member not found");
            }

            IMSMembership referred_membership = referred_member.IMSMemberships.FirstOrDefault(a => a.IsActive);

            if (referred_membership == null)
            {
                logger.ErrorFormat("CompletedReferred - Referred Membership not found - ReferrerId {0} ReferredId {1} CampaignId {2}", referrer_memberId, referred_memberId, campaignId);
                throw new Exception("referred member not found");
            }

            #endregion

            if (referral != null)
            {
                referral.IsProfileCompleted = true;
                referral.ModificationDate = DateTime.Now;
                db.Entry(referral).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("CompletedReferred - Unable to complete referred - ReferrerId {0} ReferredId {1} Exception {2}", referrer_memberId, referred_memberId, ex.ToString());
                    return null;
                }

                //Apply point to both (referrer and referred)
                #region Add Points Section

                IMSUser ims_user = new RegistrationManager().GetUserWithEmail("francois.verdon@trendigo.com");

                //Referrer
                CardPointHistory referrer_cardHistory = new CardPointHistoryService().AddCardPointHistory(referrer_card.Id, campaign.PointsToReferrer, null, CardPointHistoryReason.ReferralProgramReferrer.ToString(), "0", ims_user.Id, this.db);

                var referrer_command = DataCommandFactory.AddMembershipPointsCommand(referrer_cardHistory, referrer_membership.TransaxId, campaign.PointsToReferrer, this.db);

                var referrer_result = await referrer_command.Execute();

                if (referrer_result != DataCommandResult.Success)
                {
                    throw new Exception(string.Format("CompletedReferred - Unable to add points to referrer ReferrerId {0} ReferredId {1} Exception {2}", referrer_memberId, referred_memberId, referrer_result.ToString()));
                }

                //Referred
                CardPointHistory referred_cardHistory = new CardPointHistoryService().AddCardPointHistory(referred_card.Id, campaign.PointsToReferred, null, CardPointHistoryReason.ReferralProgramReferred.ToString(), "0", ims_user.Id, this.db);

                var referred_command = DataCommandFactory.AddMembershipPointsCommand(referred_cardHistory, referred_membership.TransaxId, campaign.PointsToReferred, this.db);

                var referred_result = await referred_command.Execute();

                if (referred_result != DataCommandResult.Success)
                {
                    throw new Exception(string.Format("CompletedReferred - Unable to add points to referred ReferrerId {0} ReferredId {1} Exception {2}", referrer_memberId, referred_memberId, referred_result.ToString()));
                }

                #endregion

                //Send email to both (referrer and referred)
                #region Send Mail section

                await new EmailService().SendReferralPointsEmail(referrer_member, campaign.PointsToReferrer, true);

                await new EmailService().SendReferralPointsEmail(referred_member, campaign.PointsToReferred, false);

                #endregion
            }

            return referral;
        }
    }
}
