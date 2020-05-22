using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Common.Core.Utilities;
using System.Data.Entity;
using System.Configuration;
using IMS.Common.Core.Enumerations;

namespace IMS.Common.Core.Services
{
    public class AccountingManager
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IMS_Detail CreateContractHeaderAndDetail(Int64 enterpriseId, Int64 merchantId, Int64? locationId, Int64? outsideChannelId, Int64? salesRepId, Int64? contractCommissionId, Int64? terminalId, Int32 lineItemId, Decimal amount, Int32 currencyId, Int64 TransactionId, Int32 TransactionTypeId, String description, IMSEntities db)
        {
            IMS_Detail detail = db.IMS_Detail.Where(a => a.IMS_LineItem.Id == lineItemId && a.IMS_Header.MerchantId == merchantId && a.TransactionId == TransactionId && DbFunctions.TruncateTime(a.CreationDate) == DbFunctions.TruncateTime(DateTime.Now)).FirstOrDefault();

            if (detail == null)
            {
                DateTime TransactionDate = DateTime.Now;
                IMS_Header header = new IMS_Header();

                //Merchant information
                Int64 MerchantId = Convert.ToInt64(merchantId);
                //Enterprise information
                Int64 EnterpriseId = Convert.ToInt64(enterpriseId);
                //PortalId
                Int64 PortalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
                //Currency information
                Int32 CurrencyId = currencyId;

                header = new AccountingManager().CreateHeader(EnterpriseId, MerchantId, locationId, outsideChannelId, salesRepId, contractCommissionId, terminalId, PortalId, amount, CurrencyId, TransactionDate, TransactionDate, db);

                if (header != null)
                    detail = new AccountingManager().CreateDetail(header.Id, lineItemId, TransactionId, Convert.ToDecimal(amount), TransactionTypeId , description, TransactionDate, db);
            }
            return detail;
        }

        public IMS_Header CreateHeader(Int64 EnterpriseId, Int64? MerchantId, Int64? LocationId, Int64? OutsideChannelId, Int64? SalesRepId, Int64? contractCommissionId, Int64? TerminalId, Int64 PortalId, Decimal Amount, Int32 CurrencyId, DateTime CurrentDate, DateTime ProcessDate, IMSEntities db) 
        {
            IMS_Header lastHeader;

            //if (MerchantId != null)
                lastHeader = db.IMS_Header.Where(a => a.EnterpriseId == EnterpriseId && a.MerchantId == MerchantId).OrderByDescending(b => b.CreationDate).Take(1).FirstOrDefault();
            //else
            //    lastHeader = context.IMS_Header.Where(a => a.EnterpriseId == EnterpriseId && a.MerchantId == null).OrderByDescending(b => b.CreationDate).Take(1).FirstOrDefault();

            IMS_Header header = new IMS_Header();

            DateTime FiscalYearToFormat = context.IMSEnterpriseParameters.Where(a => a.EnterpriseId == PortalId).Select(b => b.FiscalYearEnd).FirstOrDefault();
            DateTime endFiscalYear = new DateTime(DateTime.Now.Year, FiscalYearToFormat.Month, FiscalYearToFormat.Day);

            if (lastHeader != null) 
            {
                if (lastHeader.CreationDate.Year == CurrentDate.Year && lastHeader.CreationDate.Month == CurrentDate.Month && lastHeader.CreationDate.Day == CurrentDate.Day) 
                {
                    header = lastHeader;
                    header.OpeningAmount = lastHeader.ClosingAmount + Amount;
                    header.TotalAmount += Amount;
                    header.DueAmount = ((header.OpeningAmount + header.TotalAmount) > 0 ? header.TotalAmount : 0);
                    header.ClosingAmount = (header.OpeningAmount + header.TotalAmount > 0 ? 0 : header.OpeningAmount + header.TotalAmount);

                    db.Entry(header).State = EntityState.Modified;
                }
                else 
                {
                    //Enterprise information
                    header.EnterpriseId = EnterpriseId;
                    //Merchant information
                    if (!String.IsNullOrEmpty(MerchantId.ToString()))
                        header.MerchantId = MerchantId;
                    //Location information
                    if (!String.IsNullOrEmpty(LocationId.ToString()))
                        header.LocationId = LocationId;
                    //Outside Channel information
                    if (!String.IsNullOrEmpty(OutsideChannelId.ToString()))
                        header.OutsideChannelId = OutsideChannelId;
                    //Sales Rep information
                    if (!String.IsNullOrEmpty(SalesRepId.ToString()))
                        header.SalesRepId = SalesRepId;
                    //Sales Rep information
                    if (!String.IsNullOrEmpty(contractCommissionId.ToString()))
                        header.ContractCommissionId = contractCommissionId;
                    //TerminalId
                    if (!String.IsNullOrEmpty(TerminalId.ToString()))
                        header.TerminalId = TerminalId;
                    //PortalId
                    header.PortalId = PortalId;
                    //Currency information
                    header.CurrencyId = CurrencyId;
                    //Current date
                    header.CreationDate = CurrentDate;
                    //Current fiscal year
                    header.CurrentFiscalYear = (DateTime.Now.Month >= endFiscalYear.Month ? DateTime.Now.Year + 1 : DateTime.Now.Year);
                    //Current fiscal month
                    header.CurrentFiscalMonth = new UtilityManager().MonthDifference(endFiscalYear.AddYears(-1), DateTime.Now);
                    //Total amount information
                    header.OpeningAmount = lastHeader.ClosingAmount;
                    header.TotalAmount += Amount;
                    header.DueAmount = ((header.OpeningAmount + header.TotalAmount) > 0 ? header.TotalAmount : 0);
                    header.ClosingAmount = (header.OpeningAmount + header.TotalAmount > 0 ? 0 : header.OpeningAmount + header.TotalAmount);
                    header.ProcessDate = ProcessDate;

                    if (MerchantId != null)
                        header.PaymentStatusId = db.Merchants.Where(a => a.Id == MerchantId).Select(b => b.IsActive).FirstOrDefault() == true ? (int)IMSPaymentStatus.READY : (int)IMSPaymentStatus.ONHOLD;
                    else
                        header.PaymentStatusId = (int)IMSPaymentStatus.READY;

                    db.IMS_Header.Add(header);
                }
            }
            else //First header
            {
                //Enterprise information
                header.EnterpriseId = EnterpriseId;
                //Merchant information
                header.MerchantId = MerchantId;
                //Location information
                header.LocationId = LocationId;
                //TerminalId
                header.TerminalId = TerminalId;
                //PortalId
                header.PortalId = PortalId;
                //Currency information
                header.CurrencyId = CurrencyId;
                //Current date
                header.CreationDate = CurrentDate;
                //Current fiscal year
                header.CurrentFiscalYear = (DateTime.Now.Month >= endFiscalYear.Month ? DateTime.Now.Year + 1 : DateTime.Now.Year);
                //Current fiscal month
                header.CurrentFiscalMonth = new UtilityManager().MonthDifference(endFiscalYear.AddYears(-1), DateTime.Now);
                //Total amount information
                header.OpeningAmount = 0;
                header.TotalAmount = Amount;
                header.DueAmount = ((header.OpeningAmount + header.TotalAmount) > 0 ? header.TotalAmount : 0);
                header.ClosingAmount = (Amount < 0 ? Amount : 0);
                header.ProcessDate = ProcessDate;

                if (MerchantId != null)
                    header.PaymentStatusId = db.Merchants.Where(a => a.Id == MerchantId).Select(b => b.IsActive).FirstOrDefault() == true ? (int)IMSPaymentStatus.READY : (int)IMSPaymentStatus.ONHOLD;
                else
                    header.PaymentStatusId = (int)IMSPaymentStatus.READY;

                db.IMS_Header.Add(header);
            }

            db.SaveChanges();
            
            return header;
        }

        public IMS_Header AdjustHeader(IMS_Header header, Decimal Amount, IMSEntities db)
        {
            header.TotalAmount = header.TotalAmount + Convert.ToDecimal(Amount);
            header.DueAmount = ((header.OpeningAmount + header.TotalAmount) > 0 ? header.TotalAmount : 0);
            header.ClosingAmount = (header.OpeningAmount + header.TotalAmount > 0 ? 0 : header.OpeningAmount + header.TotalAmount);
            db.Entry(header).State = EntityState.Modified;
            db.SaveChanges();

            return header;
        }

        public IMS_Detail CreateDetail(Int64 HeaderId, Int32 LineItemId, Int64 ReferenceId, Decimal Amount, Int32 TransactionTypeId, String Description, DateTime TransactionDate, IMSEntities db) 
        {
            IMS_Detail detail = new IMS_Detail();
            detail.HeaderId = HeaderId;
            detail.LineItemId = LineItemId;
            detail.TransactionId = ReferenceId;
            detail.TransactionTypeId = TransactionTypeId;
            detail.Amount = Amount;
            detail.Description = Description;
            detail.CreationDate = TransactionDate;

            db.IMS_Detail.Add(detail);
            db.SaveChanges();

            return detail;
        }

        public IMS_Detail GetDetailLine(Int64 HeaderId, String LineItem, Int64 ReferenceId) 
        {
            IMS_Detail detail = context.IMS_Detail.FirstOrDefault(a => a.HeaderId == HeaderId && a.TransactionId == ReferenceId);

            if (detail == null) 
            {
                detail = new IMS_Detail();
            }

            return detail;
        }

        public MembershipTransactionHeader AddMembershipTransaction(long membershipId, string amountPaid, string approvalNumber, string traceNumber)
        {
            IMSMembership membership = context.IMSMemberships.FirstOrDefault(a => a.Id == membershipId);

            if (membership == null) 
            {
                throw new Exception("Membership not found for id : " + membershipId.ToString());
            }

            var transactionDate = DateTime.Now;
            var currencyId = membership.Program.ProgramFees.Where(a => a.IsActive).Select(x => x.CurrencyId).FirstOrDefault();
            var enterpriseId = membership.Program.EnterpriseId;
            decimal amount = Convert.ToDecimal(amountPaid)/ (decimal)100.0;

            MembershipTransactionHeader header = new MembershipTransactionHeader();
            MembershipTransactionDetail detail = new MembershipTransactionDetail();

            try 
            {
                header = CreateMembershipTransactionHeader(enterpriseId, amount, currencyId, transactionDate);
            }
            catch (Exception ex) 
            {
                throw new Exception("header not inserted. Exception : " + ex.ToString());
            }

            try 
            {
                detail = CreateMembershipTransactionDetail(header.Id, membership.Id, amount, approvalNumber, traceNumber, transactionDate);
            }
            catch (Exception ex) 
            {
                throw new Exception("detail not inserted. Exception : " + ex.ToString());
            }

            header.MembershipTransactionDetails.Add(detail);

            return header;
        }

        private MembershipTransactionHeader CreateMembershipTransactionHeader(Int64 enterpriseId, Decimal amount, Int32 currencyId, DateTime currentDate)
        {
            MembershipTransactionHeader lastHeader = context.MembershipTransactionHeaders.FirstOrDefault(a => a.EnterpriseId == enterpriseId &&
                                                                                                               DbFunctions.TruncateTime(a.CurrentDate) == DbFunctions.TruncateTime(currentDate));
            MembershipTransactionHeader header = new MembershipTransactionHeader();

            if (lastHeader != null)
            {
                header = lastHeader;
                header.TotalAmount = header.TotalAmount + amount;

                context.Entry(header).State = EntityState.Modified;
            }
            else
            {
                header.EnterpriseId = enterpriseId;
                header.CurrencyId = currencyId;
                header.TotalAmount = amount;
                header.CurrentDate = currentDate;

                context.MembershipTransactionHeaders.Add(header);
            }

            try 
            {
                context.SaveChanges();
            }
            catch (Exception ex) 
            {
                logger.ErrorFormat("AccountingManager - CreateMembershipTransactionHeader Exception {0}", ex.ToString());
                throw ex;
            }

            return header;
        }

        public MembershipTransactionDetail CreateMembershipTransactionDetail(Int64 headerId, Int64 membershipId, Decimal amount, String approvalNbr, String traceNbr, DateTime transactionDate)
        {
            MembershipTransactionDetail detail = new MembershipTransactionDetail();
            detail.HeaderId = headerId;
            detail.LineItemId = 13; // Membership Fee
            detail.IMSMembershipId = membershipId;
            detail.Amount = amount;
            detail.ApprovalNbr = approvalNbr;
            detail.TraceNbr = traceNbr;
            detail.CreationDate = transactionDate;

            context.MembershipTransactionDetails.Add(detail);
            context.SaveChanges();

            return detail;
        }

        public GiftCardTransactionHeader AddGiftCardTransaction(GiftCardPurchas giftCardPurchase, string amountPaid, int currencyId, string approvalNumber, string traceNumber)
        {
            Member member = context.Members.FirstOrDefault(a => a.Id == giftCardPurchase.MemberId);

            var transactionDate = DateTime.Now;
            var enterpriseId = member.EnterpriseId;
            decimal amount = Convert.ToDecimal(amountPaid) / (decimal)100.0;


            GiftCardTransactionHeader header = CreateGiftCardTransactionHeader(enterpriseId, amount, currencyId, transactionDate);

            if (header == null)
            {
                throw new ApplicationException("header");
            }

            GiftCardTransactionDetail detail = CreateGiftCardTransactionDetail(header.Id, giftCardPurchase.Id, amount, approvalNumber, traceNumber, transactionDate);

            if (detail == null)
            {
                throw new ApplicationException("detail");
            }

            header.GiftCardTransactionDetails.Add(detail);

            return header;
        }

        private GiftCardTransactionHeader CreateGiftCardTransactionHeader(Int64 enterpriseId, Decimal amount, Int32 currencyId, DateTime currentDate)
        {
            GiftCardTransactionHeader lastHeader = context.GiftCardTransactionHeaders.FirstOrDefault(a => a.EnterpriseId == enterpriseId &&
                                                                                                               DbFunctions.TruncateTime(a.CurrentDate) == DbFunctions.TruncateTime(currentDate));

            GiftCardTransactionHeader header = new GiftCardTransactionHeader();

            if (lastHeader != null)
            {
                header = lastHeader;
                header.TotalAmount = header.TotalAmount + amount;

                context.Entry(header).State = EntityState.Modified;
            }
            else
            {
                header.EnterpriseId = enterpriseId;
                header.CurrencyId = currencyId;
                header.TotalAmount = amount;
                header.CurrentDate = currentDate;

                context.GiftCardTransactionHeaders.Add(header);
            }

            context.SaveChanges();

            return header;
        }

        public GiftCardTransactionDetail CreateGiftCardTransactionDetail(Int64 headerId, Int64 giftCardPurchaseId, Decimal amount, String approvalNbr, String traceNbr, DateTime transactionDate)
        {
            GiftCardTransactionDetail detail = new GiftCardTransactionDetail();
            detail.HeaderId = headerId;
            detail.LineItemId = 13; // Membership Fee
            detail.GiftCardPurchaseId = giftCardPurchaseId;
            detail.Amount = amount;
            detail.ApprovalNbr = approvalNbr;
            detail.TraceNbr = traceNbr;
            detail.CreationDate = transactionDate;

            context.GiftCardTransactionDetails.Add(detail);
            context.SaveChanges();

            return detail;
        }


    }
}
