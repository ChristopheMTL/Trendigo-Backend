using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class MembershipManager
    {
        IMSEntities context = new IMSEntities();

        //public async Task<IMSMembership> AddMembership(long programId, long memberId, DateTime? expiryDate)
        //{
        //    IMSMembership membership = new IMSMembership();

        //    try
        //    {
        //        membership.MemberID = memberId;
        //        membership.ProgramID = programId;
        //        membership.IsActive = true;
        //        membership.CreationDate = DateTime.Now;

        //        if (expiryDate != null)
        //        {
        //            membership.ExpiryDate = expiryDate;
        //            membership.RenewalNotificationDate = Convert.ToDateTime(expiryDate).AddMonths(-1);
        //        }

        //        context.IMSMemberships.Add(membership);
        //        await context.SaveChangesAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return membership;
        //}

        //public void DeleteMembership(long membershipId) 
        //{
        //    IMSMembership membership_exist = context.IMSMemberships.FirstOrDefault(a => a.Id == membershipId);

        //    if (membership_exist != null) 
        //    {
        //        membership_exist.IsActive = false;
        //        context.Entry(membership_exist).State = EntityState.Modified;
        //        context.SaveChanges();
        //    }
        //}

        //public IMSMembership RenewMembership(long programId, long memberId, DateTime? expiryDate, bool noShipping)
        //{
        //    IMSMembership actualMembership = context.IMSMemberships.FirstOrDefault(a => a.MemberID == memberId && a.IsActive == true);

        //    //Deactivate actual
        //    try 
        //    {
        //        if (actualMembership != null)
        //        {
        //            actualMembership.IsActive = false;
        //            context.Entry(actualMembership).State = EntityState.Modified;
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex) 
        //    {
        //        return null;
        //    }

        //    IMSMembership newMembership = new IMSMembership();

        //    try
        //    {
        //        newMembership.MemberID = actualMembership.MemberID;
        //        newMembership.ProgramID = actualMembership.ProgramID;
        //        newMembership.IsActive = true;
        //        newMembership.NoShipping = noShipping;
        //        newMembership.CreationDate = DateTime.Now;

        //        if (expiryDate != null)
        //        {
        //            newMembership.ExpiryDate = expiryDate;
        //            newMembership.RenewalNotificationDate = Convert.ToDateTime(expiryDate).AddMonths(-1);
        //        }

        //        context.IMSMemberships.Add(newMembership);
        //        context.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        //Revert membership
        //        actualMembership.IsActive = true;
        //        context.Entry(actualMembership).State = EntityState.Modified;
        //        context.SaveChanges();

        //        return null;
        //    }

        //    return newMembership;
        //}

        public IMSMembership GetMembership(long? membershipId = null, long? programId = null, long? memberId = null)
        {
            IMSMembership membership = null;

            if (membershipId.HasValue)
            {
                membership = context.IMSMemberships.Where(a => a.Id == membershipId.Value).FirstOrDefault();
                return membership;
            }

            if (programId.HasValue)
            {
                membership = context.IMSMemberships.Where(a => a.ProgramID == programId.Value && a.IsActive == true).FirstOrDefault();
                return membership;
            }

            if (memberId.HasValue)
            {
                membership = context.IMSMemberships.Where(a => a.MemberID == memberId.Value && a.IsActive == true).FirstOrDefault();
                return membership;
            }


            return membership;
        }

        //public MembershipTransactionHeader CreateHeader(Int64 EnterpriseId, Decimal Amount, Int32 CurrencyId, DateTime CurrentDate)
        //{
        //    MembershipTransactionHeader last_header = context.MembershipTransactionHeaders.Where(a => a.EnterpriseId == EnterpriseId).OrderByDescending(b => b.CurrentDate).Take(1).FirstOrDefault();

        //    MembershipTransactionHeader header = new MembershipTransactionHeader();

        //    if (last_header != null)
        //    {
        //        if (last_header.CurrentDate.Year == CurrentDate.Year && last_header.CurrentDate.Month == CurrentDate.Month && last_header.CurrentDate.Day == CurrentDate.Day)
        //        {
        //            header = last_header;
        //            header.TotalAmount = header.TotalAmount + Amount;

        //            context.Entry(header).State = EntityState.Modified;
        //        }
        //        else
        //        {
        //            //Enterprise information
        //            header.EnterpriseId = EnterpriseId;
        //            //Currency information
        //            header.CurrencyId = CurrencyId;
        //            //Current date
        //            header.CurrentDate = CurrentDate;
        //            //Current fiscal year
        //            DateTime endFiscalYear = context.IMSEnterpriseParameters.Where(a => a.EnterpriseId == EnterpriseId).Select(b => b.FiscalYearEnd).FirstOrDefault();
        //            header.TotalAmount = Amount;

        //            context.MembershipTransactionHeaders.Add(header);
        //        }
        //    }
        //    else //First header
        //    {
        //        //Enterprise information
        //        header.EnterpriseId = EnterpriseId;
        //        //Currency information
        //        header.CurrencyId = CurrencyId;
        //        //Current date
        //        header.CurrentDate = CurrentDate;
        //        header.TotalAmount = Amount;

        //        context.MembershipTransactionHeaders.Add(header);
        //    }

        //    context.SaveChanges();

        //    return header;
        //}

        //public IMS_Header AdjustHeader(IMS_Header header, Decimal Amount, String DebitOrCredit)
        //{
        //    header.TotalAmount = DebitOrCredit.ToUpper() == "DEBIT" ? header.TotalAmount + Convert.ToDecimal(Amount) : header.TotalAmount - Convert.ToDecimal(Amount);
        //    context.Entry(header).State = EntityState.Modified;
        //    context.SaveChanges();

        //    return header;
        //}

        //public IMS_Detail CreateDetail(Int64 HeaderId, String LineItem, String ReferenceId, Decimal Amount, String Description, DateTime TransactionDate)
        //{
        //    IMS_Detail detail = new IMS_Detail();
        //    detail.HeaderId = HeaderId;
        //    detail.LineItemId = context.IMS_LineItem.Where(a => a.Description == LineItem).Select(b => b.Id).FirstOrDefault();
        //    detail.TransactionId = ReferenceId;
        //    detail.Amount = Amount;
        //    detail.Description = Description;
        //    detail.CreationDate = TransactionDate;

        //    context.IMS_Detail.Add(detail);
        //    context.SaveChanges();

        //    return detail;
        //}

        //public IMS_Detail GetDetailLine(Int64 HeaderId, String LineItem, String ReferenceId)
        //{
        //    IMS_Detail detail = context.IMS_Detail.Where(a => a.HeaderId == HeaderId && a.ReferenceId == ReferenceId).FirstOrDefault();

        //    if (detail == null)
        //    {
        //        detail = new IMS_Detail();
        //    }

        //    return detail;
        //}
    }
}
