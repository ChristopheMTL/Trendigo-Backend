using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace IMS.Common.Core.Services
{
    public class MemberManager
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<Member> GetMembersWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<Member> members = new List<Member>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                members = context.Members.OrderBy(a => a.LastName).ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                members = context.Members.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().OrderBy(a => a.LastName).ToList();
            }

            if (ActiveOnly)
                members = members.Where(a => a.IsActive == true).ToList();

            return members;
        }

        public async Task<List<Member>> GetMembersWithRole(IMSUser user, int start, int length, bool ActiveOnly = false)
        {
            List<Member> members = new List<Member>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                members = await context.Members.OrderBy(a => a.LastName).Skip(start).Take(length).ToListAsync();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                members = await context.Members.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().OrderBy(a => a.LastName).Skip(start).Take(length).ToListAsync();
            }

            if (ActiveOnly)
                members = members.Where(a => a.IsActive == true).ToList();

            return members;
        }

        public Member GetMemberWithRole(IMSUser user, String memberId)
        {
            List<Member> members = GetMembersWithRole(user);
            var listOfMemberId = members.Select(m => m.Id);

            return context.Members.Where(a => a.Id.ToString() == memberId).Where(a => listOfMemberId.Contains(a.Id)).FirstOrDefault();
        }

        //public async Task<List<Member>> GetHalfMembers(Int64 enterpriseId) 
        //{
        //    return await context.Members.Where(a => a.IsActive == true && a.EnterpriseId == enterpriseId && (!a.AddressId.HasValue || (a.AddressId.HasValue && a.CreditCards.Count == 0))).ToListAsync();
        //}

        //public async Task<List<Member>> GetFullMembers(Int64 enterpriseId)
        //{
        //    return await context.Members.Where(a => a.IsActive == true && a.EnterpriseId == enterpriseId && a.AddressId.HasValue && a.CreditCards.Any()).ToListAsync();
        //}

        //public async Task<List<Member>> GetNoTransactionMembers(Int64 enterpriseId) 
        //{
        //    List<String> nonFinancialcards = await context.TrxNonFinancialTransactions.Distinct().Select(a => a.cardNonFinancialId).ToListAsync();
        //    List<Member> members = await context.IMSCards.Where(a => a.MemberId.HasValue && !nonFinancialcards.Contains(a.TransaxId)).Select(b => b.Member).ToListAsync();

        //    return members;
        //}

        //public async Task<List<Member>> GetMembersWithXTransaction(Int64 enterpriseId, Int32 numberOfTransaction)
        //{
        //    var cards = from a in context.TrxNonFinancialTransactions
        //           group a by a.cardNonFinancialId into g
        //           select new { NonFinancialCardId = g.Key, Count = g.Count() };

        //    List<String> cardIds = cards.Where(a => a.Count == numberOfTransaction).Select(b => b.NonFinancialCardId).ToList();
        //    List<Member> members = await context.IMSCards.Where(a => a.MemberId.HasValue && cardIds.Contains(a.TransaxId)).Select(b => b.Member).ToListAsync();

        //    return members;
        //}

        //public async Task<List<Member>> GetMembersWithTransaction(Int64 enterpriseId)
        //{
        //    List<String> nonFinancialcards = await context.TrxNonFinancialTransactions.Distinct().Select(a => a.cardNonFinancialId).ToListAsync();
        //    List<Member> members = await context.IMSCards.Where(a => a.MemberId.HasValue && nonFinancialcards.Contains(a.TransaxId)).Select(b => b.Member).ToListAsync();

        //    return members;
        //}

        //public async Task<List<Member>> GetMembersWithDayElapseTransaction(Int64 enterpriseId, Int32 NumberOfDays)
        //{
        //    List<String> nonFinancialcards = context.TrxNonFinancialTransactions.Where(a => a.systemDateTime < DateTime.Now.AddDays(NumberOfDays * -1)).GroupBy(x => x.cardNonFinancialId,
        //                 (key, xs) => xs.OrderByDescending(x => x.systemDateTime)
        //                                .First()
        //                                .cardNonFinancialId).ToList();
            
        //    List<Member> members = await context.IMSCards.Where(a => a.MemberId.HasValue && nonFinancialcards.Contains(a.TransaxId)).Select(b => b.Member).ToListAsync();

        //    return members;
        //}

        public async Task<Member> GetMemberWithUserId(String userId) 
        {
            AspNetUser user = await context.AspNetUsers.FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                throw new Exception(string.Format("AspNetUser not found for user {0}", userId));
            }

            Member member = user.Members.FirstOrDefault();

            if (member == null)
            {
                throw new Exception(string.Format("Member not found for userId {0}", userId));
            }

            return member;
        }

        public String GetMemberNameWithAPICardNonFinancialId(String cardNonFinancialId)
        {
            String memberName = "";

            IMSCard card = context.IMSCards.FirstOrDefault(a => a.TransaxId == cardNonFinancialId);

            //if (card != null)
            //    memberName = card.Member.FirstName + " " + card.Member.LastName;

            return memberName;
        }
        
    }
}
