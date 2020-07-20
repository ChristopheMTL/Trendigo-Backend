using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using IMS.Common.Core.Utilities;
using AutoMapper;

namespace IMS.Common.Core.Services
{
    public class TransactionManager
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<TrxFinancialTransaction> GetTransactionListWithRole(IMSUser user)
        {
            List<TrxFinancialTransaction> transactions = context.TrxFinancialTransactions.ToList();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                return transactions;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                return transactions.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                List<String> locations = context.Locations.Where(a => a.IMSUsers.Any(b => b.Id == user.Id)).Select(s => s.TransaxId).ToList();
                return transactions.Where(a => locations.Contains(a.entityId)).ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SalesRep.ToString()))
            {
                var query =
                    from merchants in context.Merchants
                    from locations in merchants.Locations
                    from contractLocations in locations.ContractLocations
                    where contractLocations.Contract.SalesRepId == user.Id
                    select locations.TransaxId.Distinct();

                return transactions.Where(a => query.Contains(a.entityId)).ToList();
            }

            return new List<TrxFinancialTransaction>();
        }

        public List<TrxFinancialTransaction> GetTransactionListWithRole(IMSUser user, DateTime startingDate, DateTime endingDate)
        {
            List<TrxFinancialTransaction> transactions = context.TrxFinancialTransactions.Where(a => a.localDateTime >= startingDate && a.localDateTime <= endingDate).ToList();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                return transactions;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                return transactions.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                List<String> locations = context.Locations.Where(a => a.IMSUsers.Any(b => b.Id == user.Id)).Select(s => s.TransaxId).ToList();
                return transactions.Where(a => locations.Contains(a.entityId)).ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SalesRep.ToString()))
            {
                var query =
                    from merchants in context.Merchants
                    from locations in merchants.Locations
                    from contractLocations in locations.ContractLocations
                    where contractLocations.Contract.SalesRepId == user.Id
                    select locations.TransaxId.Distinct();

                return transactions.Where(a => query.Contains(a.entityId)).ToList();
            }

            return new List<TrxFinancialTransaction>();
        }

        public List<TrxFinancialTransaction> GetLocationTransactionHistory(Location loc)
        {
            List<TrxFinancialTransaction> transactions = new List<TrxFinancialTransaction>();

            return (from t in context.TrxFinancialTransactions
                    from l in context.Locations
                    where t.entityId == l.TransaxId
                    where l.TransaxId == loc.TransaxId
                    select t).OrderByDescending(a => a.systemDateTime).ToList();

        }

        public async Task<List<TrxNonFinancialTransaction>> GetNonFinancialTransactions(Enterprise enterprise, int lastNonFinancialTransaction)
        {
            List<TrxNonFinancialTransaction> trxs = new List<TrxNonFinancialTransaction>();
            List<IMS.Utilities.PaymentAPI.Model.TransactionNonFinancial> nfts = new List<IMS.Utilities.PaymentAPI.Model.TransactionNonFinancial>();

            nfts = await new IMS.Utilities.PaymentAPI.Api.TransactionsApi().FindNonFinancialTransactions(lastNonFinancialTransaction, null, Convert.ToInt32(enterprise.TransaxId));

            if (nfts.Count > 0)
            {

                #region Mapper Section

                Mapper.CreateMap<IMS.Utilities.PaymentAPI.Model.TransactionNonFinancial, TrxNonFinancialTransaction>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TransactionId.Value))
                    .ForMember(dest => dest.orderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                    .ForMember(dest => dest.baseAmount, opt => opt.MapFrom(src => src.BaseAmount.HasValue ? Convert.ToDecimal(src.BaseAmount.Value) : 0))
                    .ForMember(dest => dest.approvedAmount, opt => opt.MapFrom(src => src.ApprovedAmount.HasValue ? Convert.ToDecimal(src.ApprovedAmount.Value) : 0))
                    .ForMember(dest => dest.amount, opt => opt.MapFrom(src => src.Amount.HasValue ? Convert.ToDecimal(src.Amount.Value) : 0))
                    .ForMember(dest => dest.aditionalAmount, opt => opt.MapFrom(src => src.AdditionalAmount.HasValue ? Convert.ToDecimal(src.AdditionalAmount.Value) : 0))
                    .ForMember(dest => dest.tip, opt => opt.MapFrom(src => src.Tip.HasValue ? Convert.ToDecimal(src.Tip.Value) : 0))
                    .ForMember(dest => dest.memberId, opt => opt.MapFrom(src => src.MemberId.Value))
                    .ForMember(dest => dest.pointsExpended, opt => opt.MapFrom(src => src.PointExpended.HasValue ? src.PointExpended.Value : 0))
                    .ForMember(dest => dest.pointsGained, opt => opt.MapFrom(src => src.PointGained.HasValue ? src.PointGained.Value : 0))
                    .ForMember(dest => dest.systemDateTime, opt => opt.MapFrom(src => src.SystemDateTime.Value))
                    .ForMember(dest => dest.localDateTime, opt => opt.MapFrom(src => src.TerminalDateTime.Value))
                    .ForMember(dest => dest.terminalId, opt => opt.MapFrom(src => src.TerminalId.Value))
                    .ForMember(dest => dest.program, opt => opt.MapFrom(src => src.ProgramId.Value))
                    .ForMember(dest => dest.entityId, opt => opt.MapFrom(src => src.LocationId.Value))
                    .ForMember(dest => dest.promotion_appliedId, opt => opt.MapFrom(src => src.PromotionId.Value))
                    .ForMember(dest => dest.vendorId, opt => opt.MapFrom(src => src.VendorId))
                    .ForMember(dest => dest.inputModeId, opt => opt.MapFrom(src => src.InputModeId.HasValue ? src.InputModeId.Value : (int)TransaxInputMode.UNKNOWN))
                    .ForMember(dest => dest.transactionTypeId, opt => opt.MapFrom(src => src.TransactionTypeId.HasValue ? src.TransactionTypeId.Value : (int)TransaxTransactionType.Unknown))
                    .ForMember(dest => dest.voided, opt => opt.MapFrom(src => src.Voided.HasValue ? (src.Voided.Value == true ? 1 : 0) : 0))
                    .ForMember(dest => dest.refunded, opt => opt.MapFrom(src => src.Refunded.HasValue ? (src.Refunded.Value == true ? 1 : 0) : 0))
                    .ForMember(dest => dest.concluded, opt => opt.MapFrom(src => src.Concluded.HasValue ? (src.Concluded.Value == true ? 1 : 0) : 0))
                    .ForMember(dest => dest.creationDate, opt => opt.MapFrom(src => src.CreationDate.Value))
                    .ForMember(dest => dest.EnterpriseId, opt => opt.MapFrom(src => src.EnterpriseId.HasValue ? enterprise.Id : enterprise.Id))
                    .ForMember(dest => dest.responseMessage, opt => opt.MapFrom(src => src.ResponseMessage));
                    
                #endregion

                try
                {
                    Mapper.Map<List<IMS.Utilities.PaymentAPI.Model.TransactionNonFinancial>, List<TrxNonFinancialTransaction>>(nfts, trxs);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("TransactionManager - GetNonFinancialTransactions - Exception in mapping from PaymentAPI for enterpriseId {0} Exception {1}", enterprise.Id, ex.InnerException.ToString());
                }
            }

            return trxs;
        }

        public async Task<List<TrxFinancialTransaction>> GetFinancialTransactions(Enterprise enterprise, long lastFinancialTransaction, string transactionStatus)
        {
            List<TrxFinancialTransaction> trxs = new List<TrxFinancialTransaction>();
            List<IMS.Utilities.PaymentAPI.Model.TransactionFinancial> fts = new List<IMS.Utilities.PaymentAPI.Model.TransactionFinancial>();

            try
            {
                fts = await new IMS.Utilities.PaymentAPI.Api.TransactionsApi().FindFinancialTransactions(Convert.ToInt32(lastFinancialTransaction), null, Convert.ToInt32(enterprise.TransaxId), transactionStatus);
            }
            catch (IMS.Utilities.PaymentAPI.Client.ApiException apiEx)
            {
                logger.ErrorFormat("TransactionManager - GetFinancialTransactions - FindFinancialTransactions - ApiException ErrorCode {0} ErrorContent {1} Data {2}", apiEx.ErrorCode, apiEx.ErrorContent, string.Join(";", apiEx.Data));
            }

            if (fts.Count > 0)
            {
                #region Mapper Section

                Mapper.CreateMap<IMS.Utilities.PaymentAPI.Model.TransactionFinancial, TrxFinancialTransaction>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TransactionId.ToString()))
                    .ForMember(dest => dest.processorId, opt => opt.MapFrom(src => src.ProcessorId.Value))
                    .ForMember(dest => dest.acquirerId, opt => opt.MapFrom(src => (int)AcquirerEnum.GlobalPayment))
                    .ForMember(dest => dest.EnterpriseId, opt => opt.MapFrom(src => src.EnterpriseId.HasValue ? enterprise.Id : enterprise.Id))
                    .ForMember(dest => dest.baseAmount, opt => opt.MapFrom(src => src.BaseAmount.HasValue ? Convert.ToDecimal(src.BaseAmount.Value) : 0))
                    .ForMember(dest => dest.approvedAmount, opt => opt.MapFrom(src => src.ApprovedAmount.HasValue ? Convert.ToDecimal(src.ApprovedAmount.Value) : 0))
                    .ForMember(dest => dest.amount, opt => opt.MapFrom(src => src.Amount.HasValue ? Convert.ToDecimal(src.Amount.Value) : 0))
                    .ForMember(dest => dest.aditionalAmount, opt => opt.MapFrom(src => src.AdditionalAmount.HasValue ? Convert.ToDecimal(src.AdditionalAmount.Value) : 0))
                    .ForMember(dest => dest.tip, opt => opt.MapFrom(src => src.Tip.HasValue ? Convert.ToDecimal(src.Tip.Value) : 0))
                    .ForMember(dest => dest.entityId, opt => opt.MapFrom(src => src.LocationId))
                    .ForMember(dest => dest.vendorId, opt => opt.MapFrom(src => src.VendorId))
                    .ForMember(dest => dest.terminalReference, opt => opt.MapFrom(src => src.TerminalReference))
                    .ForMember(dest => dest.localDateTime, opt => opt.MapFrom(src => src.LocalDateTime.Value))
                    .ForMember(dest => dest.acquirerReference, opt => opt.MapFrom(src => src.AcquirerReference))  //.Length > 0 ? src.AcquirerResponseMessage.Substring(src.AcquirerResponseMessage.IndexOf(" "), src.AcquirerResponseMessage.Length - src.AcquirerResponseMessage.IndexOf(" ")) : ""
                    .ForMember(dest => dest.currencyId, opt => opt.MapFrom(src => src.CurrencyId))
                    .ForMember(dest => dest.inputModeId, opt => opt.MapFrom(src => src.InputModeId))
                    .ForMember(dest => dest.cardTypeId, opt => opt.MapFrom(src => src.CreditCardTypeId.Value == 0 ? (int)TransaxCardType.Unknown : src.CreditCardTypeId.Value))
                    .ForMember(dest => dest.transactionTypeId, opt => opt.MapFrom(src => src.TransactionTypeId))
                    .ForMember(dest => dest.legTransaction, opt => opt.MapFrom(src => src.TransactionNonFinancialId))
                    .ForMember(dest => dest.systemDateTime, opt => opt.MapFrom(src => src.SystemDateTime))
                    .ForMember(dest => dest.transaxTerminalId, opt => opt.MapFrom(src => src.TerminalId))
                    .ForMember(dest => dest.creditCardId, opt => opt.MapFrom(src => src.CreditCardId.Value))
                    .ForMember(dest => dest.merchantResponseMessage, opt => opt.MapFrom(src => src.AcquirerResponseMessage))
                    .ForMember(dest => dest.acquirerResponseCode, opt => opt.MapFrom(src => src.AcquirerResponseCode))
                    .ForMember(dest => dest.acquirerMerchantId, opt => opt.MapFrom(src => src.AcquirerMerchantId))
                    .ForMember(dest => dest.acquirerTerminalId, opt => opt.MapFrom(src => src.AcquirerTerminalId))
                    .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Clerk))
                    .ForMember(dest => dest.voided, opt => opt.MapFrom(src => src.Voided.HasValue ? (src.Voided.Value == true ? 1 : 0) : 0))
                    .ForMember(dest => dest.refunded, opt => opt.MapFrom(src => src.Refunded.HasValue ? (src.Refunded.Value == true ? 1 : 0) : 0))
                    .ForMember(dest => dest.concluded, opt => opt.MapFrom(src => src.Concluded.HasValue ? (src.Concluded.Value == true ? 1 : 0) : 0))
                    .ForMember(dest => dest.creationDate, opt => opt.MapFrom(src => src.CreationDate))
                    .ForMember(dest => dest.idRelatedTransaction, opt => opt.MapFrom(src => src.RelatedTransactionId.Value));

                #endregion

                try
                {
                    Mapper.Map<List<IMS.Utilities.PaymentAPI.Model.TransactionFinancial>, List<TrxFinancialTransaction>>(fts, trxs);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("TransactionManager - GetFinancialTransactions - Exception in mapping from PaymentAPI for enterpriseId {0} Exception {1}", enterprise.Id, ex.InnerException.ToString());
                }
            }

            return trxs;
        }

        public List<TrxNonFinancialTransaction> GetNonFinancialTransactionListWithRole(IMSUser user, Boolean MerchantTransactionOnly = true)
        {
            List<TrxNonFinancialTransaction> nonFinancial = context.TrxNonFinancialTransactions.ToList();

            if (MerchantTransactionOnly)
            {
                nonFinancial = nonFinancial.Where(a => a.entityId != "0").ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                return nonFinancial;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                return nonFinancial.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                List<String> locations = context.Locations.Where(a => a.IMSUsers.Any(b => b.Id == user.Id)).Select(s => s.TransaxId).ToList();
                return nonFinancial.Where(a => locations.Contains(a.entityId)).ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SalesRep.ToString()))
            {
                var query =
                    from merchants in context.Merchants
                    from locations in merchants.Locations
                    from contractLocations in locations.ContractLocations
                    where contractLocations.Contract.SalesRepId == user.Id
                    select locations.TransaxId.Distinct();

                return nonFinancial.Where(a => query.Contains(a.entityId)).ToList();
            }

            return new List<TrxNonFinancialTransaction>();
        }

        public List<TrxNonFinancialTransaction> GetNonFinancialTransactionListWithRole(IMSUser user, DateTime startingDate, DateTime endingDate, Boolean MerchantTransactionOnly = true)
        {
            List<TrxNonFinancialTransaction> nonFinancial = context.TrxNonFinancialTransactions.Where(a => a.systemDateTime >= startingDate && a.systemDateTime <= endingDate).ToList();

            if (MerchantTransactionOnly)
            {
                nonFinancial = nonFinancial.Where(a => a.entityId != "0").ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                return nonFinancial;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                return nonFinancial.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                List<String> locations = context.Locations.Where(a => a.IMSUsers.Any(b => b.Id == user.Id)).Select(s => s.TransaxId).ToList();
                return nonFinancial.Where(a => locations.Contains(a.entityId)).ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SalesRep.ToString()))
            {
                var query =
                    from merchants in context.Merchants
                    from locations in merchants.Locations
                    from contractLocations in locations.ContractLocations
                    where contractLocations.Contract.SalesRepId == user.Id
                    select locations.TransaxId.Distinct();

                return nonFinancial.Where(a => query.Contains(a.entityId)).ToList();
            }

            return new List<TrxNonFinancialTransaction>();
        }

        /// <summary>
        /// This method validate if a transaction was voided or has a transaction type void
        /// </summary>
        /// <param name="transaction">Transaction to evaluate</param>
        /// <param name="db">Entities</param>
        /// <returns>True of False</returns>
        public bool transactionIsVoided(TrxFinancialTransaction transaction, IMSEntities db)
        {
            if (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidRefund || transaction.transactionTypeId == (int)TransaxTransactionType.VoidPreauth)
                return true;

            TrxFinancialTransaction voidedTrx = db.TrxFinancialTransactions.FirstOrDefault(a => a.idRelatedTransaction == transaction.Id.ToString() && a.voided.Value == 1);

            if (voidedTrx != null)
                return true;

            return false;
        }


        public async Task<String> BuildReceipt(TrxFinancialTransaction financial, TrxNonFinancialTransaction nonFinancial, String filePath)
        {
            #region Declaration Section

            String receipt = "";

            #endregion

            #region Validation Section

            if (financial == null || nonFinancial == null)
            {
                throw new Exception("Missing parameter");
            }

            Location location = await context.Locations.FirstOrDefaultAsync(a => a.TransaxId == nonFinancial.entityId);

            if (location == null)
            {
                throw new Exception("Location not found");
            }

            Member member = await context.Members.FirstOrDefaultAsync(a => a.TransaxId == nonFinancial.memberId.ToString());

            if (member == null)
            {
                throw new Exception("No Member assigned to card");
            }

            CreditCard creditCard = await context.CreditCards.FirstOrDefaultAsync(a => a.TransaxId == financial.creditCardId.ToString());

            String currency = "$";
            Currency cur = await context.Currencies.FirstOrDefaultAsync(a => a.TransaxId == financial.currencyId.Value.ToString());

            if (cur != null)
            {
                currency = cur.Symbol;
            }

            #endregion

            try
            {
                receipt = await BuildReceipt(location, member, creditCard, financial, nonFinancial, currency, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SendEmailReceipt - Exception {0} InnerException {1}", ex.ToString(), ex.InnerException));
            }

            return receipt;
        }

        public async Task<String> BuildReceipt(Location location, Member member, CreditCard creditCard, TrxFinancialTransaction financial, TrxNonFinancialTransaction nonFinancial, String currency, String filePath)
        {
            if (nonFinancial.transactionTypeId == (int)TransaxTransactionType.Sale || nonFinancial.transactionTypeId == (int)TransaxTransactionType.VoidSale)
            {
                #region Validation Section

                if (nonFinancial == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter NonFinancial - FinancialId {0}", financial.Id.ToString()));
                }

                if (financial == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter Financial - NonFinancialId {0}", nonFinancial.Id.ToString()));
                }

                if (creditCard == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter CreditCard - FinancialId {0}", financial.Id.ToString()));
                }

                if (member == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter Member - FinancialId {0}", financial.Id.ToString()));
                }

                if (location == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter Location - FinancialId {0}", financial.Id.ToString()));
                }

                if (location.Merchant == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter Merchant - FinancialId {0}", financial.Id.ToString()));
                }

                if (location.Address == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter Address - FinancialId {0}", financial.Id.ToString()));
                }

                if (location.Address.State == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter State - FinancialId {0}", financial.Id.ToString()));
                }

                if (financial.localDateTime == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter LocalDateTime - FinancialId {0}", financial.Id.ToString()));
                }

                if (financial.baseAmount == null)
                {
                    throw new Exception(string.Format("Build Receipt - Missing parameter BaseAmount - FinancialId {0}", financial.Id.ToString()));
                }

                #endregion

                string language = "en";
                if (member.Language != null)
                    language = member.Language.ISO639_1;

                string emailBodyTemplateName = "";

                if (nonFinancial.transactionTypeId == (int)TransaxTransactionType.Sale)
                {
                    //emailBodyTemplateName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("Views/Email/Invoice.{0}.html", language));
                    emailBodyTemplateName = Path.Combine(filePath, String.Format("Invoice.{0}.html", language));
                }
                else if (nonFinancial.transactionTypeId == (int)TransaxTransactionType.VoidSale)
                {
                    //emailBodyTemplateName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("Views/Email/Void.{0}.html", language));
                    emailBodyTemplateName = Path.Combine(filePath, String.Format("Void.{0}.html", language));
                }

                string htmlBody = "";

                if (nonFinancial.receipt != null && nonFinancial.receipt.Length > 0)
                {
                    htmlBody = nonFinancial.receipt;
                }
                else
                {
                    #region Declaration Section

                    string merchant = " ";
                    string address = " ";
                    string city = " ";
                    string state = " ";
                    string zip = " ";
                    string _citystatezip = " ";
                    string citystatezip = " ";
                    string phone = " ";
                    string receipt = " ";
                    string date = " ";
                    string clerk = " ";
                    decimal calc_baseAmount = 0;
                    decimal calc_additionalAmount = 0;
                    decimal calc_tip = 0;
                    decimal _amount = 0;
                    string amount = " ";
                    string tip = " ";
                    decimal _total = 0;
                    string total = " ";
                    string number = " ";
                    int pointUsed = 0;
                    string ptsUsed = " ";
                    int pointReturned = 0;
                    string ptsReturned = " ";
                    decimal _total2 = 0;
                    string total2 = " ";
                    string message = " ";
                    string autorization = " ";
                    string batch = " ";
                    string sequence = " ";
                    string merchantId = " ";
                    string terminalId = " ";
                    int pointGained = 0;
                    string ptsGained = " ";
                    int pointRemoved = 0;
                    string ptsRemoved = " ";
                    string default_logo = " ";
                    string logo = " ";
                    string default_image = " ";
                    string merchantImage = " ";
                    string approvedAmount = " ";
                    string voided = " ";

                    #endregion

                    #region Value Section

                    //Parameter {0} : Merchant Name
                    merchant = location.Merchant.Name.Substring(0, Math.Min(location.Merchant.Name.Length, 24)).ToUpper();

                    //Parameter {1} : Full address
                    address = location.Address.StreetAddress.Substring(0, Math.Min(location.Address.StreetAddress.Length, 24)).ToUpper();
                    city = location.Address.City.ToUpper();
                    state = location.Address.State.Alpha2Code.ToUpper();
                    zip = location.Address.Zip.ToUpper();
                    _citystatezip = (city + " " + state + " " + zip);

                    //Parameter {2} : City, State and Zip
                    citystatezip = _citystatezip.Substring(0, Math.Min(_citystatezip.Length, 24));

                    //Parameter {3} : Phone number
                    phone = location.Telephone.Length == 10 ? String.Format("{0:(###) ###-####}", location.Telephone) : location.Telephone;

                    //Parameter {4} : Receipt number
                    if (nonFinancial.transactionTypeId == (int)TransaxTransactionType.Sale)
                    {
                        receipt = financial.Id.ToString().PadLeft(9, '0');
                    }
                    else
                    {
                        receipt = nonFinancial.Id.ToString().PadLeft(9, '0');
                    }

                    //Parameter {5} : Transaction DateTime
                    date = financial.localDateTime.Value.ToString("yyyy-MM-dd hh:mm:ss");

                    //Parameter {6} : Clerk Identification
                    clerk = string.IsNullOrEmpty(financial.description) ? " " : financial.description.Substring(9, 2);

                    //Parameter {7} : Invoice Amount (base amount + tax amount)
                    calc_baseAmount = financial.baseAmount.Value;
                    calc_additionalAmount = financial.aditionalAmount.HasValue ? financial.aditionalAmount.Value : 0;
                    calc_tip = financial.tip.HasValue ? financial.tip.Value : 0;
                    _amount = calc_baseAmount + calc_additionalAmount - calc_tip;
                    amount = string.Format("{0:N2}", _amount);

                    //Parameter {8} : Tip Amount 
                    tip = string.Format("{0:N2}", calc_tip);

                    //Parameter {9} : Total Amount
                    _total = calc_baseAmount + calc_additionalAmount;
                    total = string.Format("{0:N2}", _total);

                    //Parameter {10} : Card Provider
                    string card = "Trendigo";

                    //Parameter {11} : Masked Card Number
                    //number = "****" + assignedCard.CardNumber.Substring(assignedCard.CardNumber.Length - 4, 4);
                    number = member.Id.ToString().PadLeft(10, '0');

                    //Parameter {12} : Points Used/Returned for Transaction
                    pointUsed = nonFinancial.pointsExpended.HasValue ? nonFinancial.pointsExpended.Value : 0;
                    ptsUsed = pointUsed.ToString();
                    pointReturned = pointUsed * -1;
                    ptsReturned = pointReturned.ToString();

                    _total2 = _total - pointUsed;
                    total2 = string.Format("{0:N2}", _total2 < 0 ? 0 : _total2);

                    string[] response = financial.merchantResponseMessage.Split(' ');
                    //Parameter {13} : Acquirer Message
                    message = response.Count() > 0 ? response[0] : " ";
                    //Parameter {14} : Authorization Number
                    autorization = response.Count() > 1 ? response[1] : "";

                    //Parameter {15} : Batch Number
                    //batch = nonFinancial.batchDay.HasValue ? nonFinancial.batchDay.Value.ToString().PadLeft(4, '0') : "0000";
                    batch = "0000";

                    //Parameter {16} : Sequence Number
                    //sequence = nonFinancial.seqBatch.HasValue ? nonFinancial.seqBatch.Value.ToString().PadLeft(5, '0') : "00000";
                    sequence = "00000";

                    //Parameter {17} : Reference Number
                    string reference = "000000000001";

                    //Parameter {18} : Merchant Id
                    merchantId = financial.entityId.ToString();

                    //Parameter {19} : Terminal Id
                    terminalId = financial.transaxTerminalId.ToString();

                    //Parameter {20} : Point Gained/Removed Value
                    pointGained = nonFinancial.pointsGained.HasValue ? nonFinancial.pointsGained.Value : 0;
                    ptsGained = pointGained.ToString();
                    pointRemoved = pointGained * -1;
                    ptsRemoved = pointRemoved.ToString();

                    //Parameter {21} : Currency (this parameter is passed in the method)

                    //Parameter {22} : Merchant Logo
                    default_logo = ConfigurationManager.AppSettings["IMS.Default.Merchant.Logo." + location.Merchant.Enterprises.FirstOrDefault().Id.ToString()];
                    logo = new MerchantManager().GetMerchantLogoPath(location.Merchant.Id, location.Merchant.LogoPath);

                    //Parameter {23} : Merchant Image
                    default_image = ConfigurationManager.AppSettings["IMS.Default.Merchant.Image." + location.Merchant.Enterprises.FirstOrDefault().Id.ToString()];
                    merchantImage = new MerchantManager().GetMerchantDefaultImage(location.Merchant.Id, location.Merchant.MerchantImages.OrderBy(a => a.Weight).FirstOrDefault().ImagePath);
                    if (logo == default_logo)
                    {
                        merchantImage = default_image;
                    }

                    //Parameter {24} : Approval Amount
                    approvedAmount = string.Format("{0:N2}", financial.approvedAmount ?? 0);

                    //Parameter {25} : Voided Amount (if transaction type is VOIDED)
                    voided = financial.Id.ToString();

                    #endregion

                    using (var fs = File.OpenRead(emailBodyTemplateName))
                    using (var sr = new StreamReader(fs))
                    {
                        htmlBody = sr.ReadToEnd();

                        if (nonFinancial.pointsExpended >= 0 && nonFinancial.transactionTypeId == (int)TransaxTransactionType.Sale)
                        {
                            try
                            {
                                htmlBody = String.Format(htmlBody, merchant, address, citystatezip, phone, receipt, date, clerk, amount, tip, total, card, number, ptsUsed, message, autorization, batch, sequence, reference, merchantId, terminalId, ptsGained, currency, logo, merchantImage, approvedAmount);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Build Receipt - Cannot Build Receipt - Merchant {0}, Address {1}, CityStateZip {2}, Phone {3}, Receipt {4}, Date {5}, Clerk {6}, Amount {7}, Tip {8}, Total {9}, Card {10}, Number {11}, PtsUsed {12}, Message {13}, Authorization {14}, Batch {15}, Sequence {16}, Reference {17}, MerchantId {18}, TerminalId {19}, PtsGained {20}, Currency {21}, Logo {22}, MerchantImage {23}, ApprovedAmount {24}", merchant, address, citystatezip, phone, receipt, date, clerk, amount, tip, total, card, number, ptsUsed, message, autorization, batch, sequence, reference, merchantId, terminalId, ptsGained, currency, logo, merchantImage, approvedAmount));
                            }

                        }
                        else if (nonFinancial.pointsExpended < 0 && nonFinancial.transactionTypeId == (int)TransaxTransactionType.VoidSale)
                        {
                            htmlBody = String.Format(htmlBody, merchant, address, citystatezip, phone, receipt, date, clerk, voided, amount, tip, total, card, number, amount, ptsReturned, message, autorization, batch, sequence, reference, date, merchantId, terminalId, ptsRemoved, currency, logo, merchantImage);
                        }
                        else
                        {
                            throw new Exception(string.Format("Unknowned transaction type transaction Id {0} transaction type Id {1}", nonFinancial.Id, nonFinancial.transactionTypeId));
                        }
                    }

                    //Update non financial transaction with receipt
                    using (IMSEntities db = new IMSEntities())
                    {
                        TrxNonFinancialTransaction nft = await db.TrxNonFinancialTransactions.FirstOrDefaultAsync(a => a.Id == nonFinancial.Id);
                        nft.receipt = htmlBody;
                        db.Entry(nft).State = System.Data.Entity.EntityState.Modified;

                        try
                        {
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                return htmlBody;
            }

            return null;
        }

        #region Transaction Transfer Section

        public Int64 GetLastTransactionId()
        {
            Int64? lastTransaction = context.IMS_Detail.Take(1).OrderByDescending(a => a.TransactionId).Select(a => a.TransactionId).FirstOrDefault();

            lastTransaction = lastTransaction == null ? 0 : lastTransaction;

            return (long)lastTransaction;
        }

        public async Task<decimal> TransfertTransactions(TrxFinancialTransaction transaction, long portalId, IMSEntities context)
        {
            TimeSpan batchClosingTime = await context.IMSEnterpriseParameters.Where(a => a.EnterpriseId == portalId).Select(b => b.BatchCloseTime).FirstOrDefaultAsync();
            DateTime IMSDateTimeToProcess = DateTime.Now.Date + batchClosingTime;
            DateTime ProcessDate = DateTime.Now;
            decimal totalMerchantAmount_ProcessDate = 0;

            #region Validation Section

            //Validate if transactions as all parameters
            if (!transactionIsValid(transaction, context))
            {
                //TODO log transaction as ERROR
                throw new Exception("Missing parameters");
            }

            //Validate if transaction is voided
            if (new TransactionManager().transactionIsVoided(transaction, context))
            {
                return 0;
            }

            Location location = await context.Locations.Where(a => a.TransaxId == transaction.entityId).FirstOrDefaultAsync();

            if (location == null)
            {
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_LOCATION_INFO_FAILED, context);
                return 0;
            }

            IMS.Common.Core.Data.Merchant merchant = location.Merchant;

            if (merchant == null)
            {
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_LOCATION_INFO_FAILED, context);
                return 0;
            }

            Enterprise enterprise = context.Enterprises.FirstOrDefault(a => a.Id == transaction.EnterpriseId);

            if (enterprise == null)
            {
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_ENTERPRISE_INFO_FAILED, context);
                return 0;
            }

            Contract contract = location.Merchant.Contracts.Where(a => a.IsActive == true).FirstOrDefault();    //a.ContractStartDate <= transaction.localDateTime && a.ContractEndDate >= transaction.localDateTime && 

            if (contract == null)
            {
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_CONTRACT_INFO_FAILED, context);
                return 0;
            }

            SalesRep salesRep = contract.SalesRep;

            if (salesRep == null)
            {
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_SALESREP_INFO_FAILED, context);
                return 0;
            }

            Terminal terminal = location.Location_Terminals.Where(b => b.TransaxId == transaction.transaxTerminalId).Select(b => b.Terminal).FirstOrDefault();

            if (terminal == null)
            {
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_TERMINAL_INFO_FAILED, context);
                return 0;
            }

            IMSUser vendor = await context.IMSUsers.Where(a => a.TransaxId == transaction.vendorId).FirstOrDefaultAsync();

            if (vendor == null)
            {
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_VENDOR_INFO_FAILED, context);
                return 0;
            }

            #endregion

            #region Initialization Section

            Decimal ims_taxes = 0;
            Decimal merchant_taxes = 0;

            //Promotion
            Promotion promotion = location.Promotions.Where(a => a.Locations.FirstOrDefault().Merchant.Enterprises.Any(b => b.Id == enterprise.Id) && a.Package.StartingDate <= transaction.localDateTime && a.Package.EndingDate >= transaction.localDateTime && a.IsActive == true).FirstOrDefault();

            //This is the minimum commission that IMS will make on a transaction
            Decimal IMSBaseCommission = Convert.ToDecimal(ConfigurationManager.AppSettings["minPercentageCommission"]);

            //This is the commission percentage that IMS will make based on the promotion and contract
            Decimal commissionPercent = GetCommissionPercentage(promotion, contract, transaction);

            //This is the tip amount in the transaction
            Decimal tips = Convert.ToDecimal(transaction.tip.HasValue ? transaction.tip.Value : 0);
            //This is the tax amount combine with the tip amount
            Decimal additionalAmount = Convert.ToDecimal(transaction.aditionalAmount.HasValue ? transaction.aditionalAmount.Value : 0);
            //This is the total amount of the transaction without tips
            Decimal totalTransactionAmountWithTaxes = transaction.baseAmount.Value + additionalAmount - tips;

            //Applicable taxes
            Decimal applicabletaxes = 0;

            if (enterprise.Address.State.StateTaxes != null)
            {
                foreach (StateTax statetax in enterprise.Address.State.StateTaxes.Where(a => a.IsActive == true).ToList())
                {
                    applicabletaxes += Convert.ToDecimal(statetax.Value);
                }
            }

            //This is the base amount of the transaction
            Decimal baseAmount = 0;
            if (applicabletaxes > 0)
            {
                baseAmount = totalTransactionAmountWithTaxes / (1 + applicabletaxes);
            }
            else
            {
                baseAmount = totalTransactionAmountWithTaxes;
            }

            //This is the tax amount in the transaction. That amount is calculated from the additional amount minus the tips amount
            Decimal taxes = 0;
            if (applicabletaxes > 0)
            {
                taxes = totalTransactionAmountWithTaxes - baseAmount;
            }

            #endregion

            #region Process Date Section

            try
            {
                //Get the latest datetime to process for the same day
                DateTime LatestDateTimeToProcessSameDay = TimeZoneInfo.ConvertTime(IMSDateTimeToProcess, new UtilityManager().getTimeZoneInfoForEntity(location, context));
                //This is the process date for that transaction, if datetime is greater than our bacth closing time then we add one day
                ProcessDate = DateTime.Now.AddDays(TimeSpan.Compare(LatestDateTimeToProcessSameDay.TimeOfDay, Convert.ToDateTime(transaction.localDateTime).TimeOfDay) == -1 ? 1 : 0).Date;
            }
            catch (Exception ex)
            {

                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.GET_TIMEZONE_INFO_FOR_ENTRY_ERROR, context);
                throw new Exception(string.Format("Transaction Manager - Transfer Transaction - Error getting timezone for locationId {0} Exception {1}", location.Id, ex.ToString()));
            }

            ProcessDate = GetDateWithDelayForProcessDate(ProcessDate);

            #endregion

            #region IMS Section

            //This is the commission amount for IMS based on the promotion
            Decimal ims_amount = Math.Round(baseAmount * commissionPercent, 2);

            //This is the base commission amount for IMS based on the value in the configuration file
            Decimal ims_baseCommission_amount = Math.Round(baseAmount * IMSBaseCommission, 2);

            if (applicabletaxes > 0)
            {
                ims_taxes = Math.Round(ims_amount * applicabletaxes, 2);
            }

            #endregion

            #region Merchant Section

            Decimal merchant_amount = baseAmount - ims_amount - ims_taxes;
            merchant_amount = merchant_amount * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1);

            merchant_taxes = applicabletaxes > 0 ? taxes : 0;
            merchant_taxes = merchant_taxes * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1);

            Decimal merchant_tips = tips * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1);
            Decimal merchant_totalAmount = merchant_amount + merchant_taxes;
            totalMerchantAmount_ProcessDate += merchant_totalAmount + merchant_tips;

            #endregion

            #region Sales Rep Section

            Decimal salesRepPercent = 0;
            Decimal salesRep_amount = 0;
            if (contract.CommissionRate > 0)
            {
                salesRepPercent = (Decimal)contract.CommissionRate;
                salesRep_amount = ims_baseCommission_amount * salesRepPercent;
                salesRep_amount = salesRep_amount * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1);
            }

            #endregion

            #region Outside Channel Section

            Decimal outsideChannelPercent = 0;
            Decimal outsideChannelAmount = 0;
            OutsideChannel outsideChannel = new OutsideChannel();

            if (transaction.legTransaction != null)
            {
                long idRelatedFinancialTransaction = Convert.ToInt64(transaction.legTransaction);

                Member member = (from nf in context.TrxNonFinancialTransactions
                                 from m in context.Members
                                 where m.TransaxId == nf.memberId.ToString()
                                 select m).FirstOrDefault();

                //if (member != null && member.OutsideChannelId != null)
                //{
                //    outsideChannel = member.OutsideChannel;

                //    if (outsideChannel.CommissionRate > 0)
                //    {
                //        outsideChannelPercent = (Decimal)outsideChannel.CommissionRate;
                //        outsideChannelAmount = ims_baseCommission_amount * outsideChannelPercent;
                //        outsideChannelAmount = outsideChannelAmount * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1);
                //    }
                //}
            }

            #endregion

            #region Header and Detail

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //Insert header and details for IMS
                    CreateHeaderDetail(
                        transaction,
                        enterprise,
                        terminal,
                        (ims_amount * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1)) + (ims_taxes * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1)),
                        ims_amount * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1),
                        ims_taxes * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1),
                        ProcessDate,
                        context);

                    //Insert header and details for merchant
                    CreateHeaderDetail(transaction, enterprise, location, terminal, merchant_totalAmount, merchant_amount, merchant_taxes, merchant_tips, ProcessDate, context);

                    //Insert header and details for sales rep
                    if (salesRep_amount != 0)
                    {
                        CreateHeaderDetail(transaction, enterprise, salesRep, terminal, salesRep_amount, salesRep_amount, ProcessDate, context);
                    }

                    //Insert header and details for outside channel
                    if (outsideChannelAmount != 0)
                    {
                        CreateHeaderDetail(transaction, enterprise, outsideChannel, terminal, outsideChannelAmount, outsideChannelAmount, ProcessDate, context);
                    }

                    //Insert header and details for outside rep
                    if (contract.ContractCommissions.Count > 0)
                    {
                        foreach (ContractCommission cc in contract.ContractCommissions)
                        {
                            IMSUser rep = cc.IMSUser;
                            Decimal repPercent = (Decimal)cc.CommissionRate;
                            Decimal rep_amount = ims_baseCommission_amount * repPercent;
                            rep_amount = rep_amount * ((transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? -1 : 1);

                            CreateHeaderDetail(transaction, enterprise, cc, terminal, rep_amount, rep_amount, ProcessDate, context);
                        }
                    }

                    dbContextTransaction.Commit();

                    //transfer successful - update transfer date
                    transaction.IMSTransferStatusId = (int)IMS.Common.Core.Enumerations.IMSTransferStatus.SUCCESS;
                    transaction.IMSTransferMessageId = null;
                    transaction.IMSTransferDate = DateTime.Now;
                    context.Entry(transaction).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw new Exception(string.Format("Transaction Manager - Transfer Transaction Exception {0}", ex.ToString()));
                }
            }

            #endregion

            return totalMerchantAmount_ProcessDate;
        }

        /// <summary>
        /// This is the Create Header and Detail For IMS
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="enterprise"></param>
        /// <param name="terminal"></param>
        /// <param name="amount"></param>
        /// <param name="baseAmount"></param>
        /// <param name="taxes"></param>
        /// <param name="ProcessDate"></param>
        private bool CreateHeaderDetail(TrxFinancialTransaction transaction, Enterprise enterprise, Terminal terminal, Decimal amount, Decimal baseAmount, Decimal taxes, DateTime ProcessDate, IMSEntities db)
        {
            IMS_Detail detail;
            Int64 PortalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
            Int32 CurrencyId = enterprise.Address.Country.CurrencyId;

            IMS_Header header = db.IMS_Header.Where(
                a => a.EnterpriseId == enterprise.Id &&
                DbFunctions.TruncateTime(a.CreationDate) == DbFunctions.TruncateTime((DateTime)transaction.localDateTime)
                ).FirstOrDefault();

            if (header == null)
                try
                {
                    header = new AccountingManager().CreateHeader(enterprise.Id, null, null, null, null, null, terminal.Id, PortalId, amount, CurrencyId, (DateTime)transaction.localDateTime, ProcessDate, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error create header for IMS - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_HEADER_IMS_FAILED, db);
                    return false;
                }
            else
                try
                {
                    header = new AccountingManager().AdjustHeader(header, amount, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error adjust header for IMS - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.ADJUST_HEADER_IMS_FAILED, db);
                    return false;
                }

            //Package commission detail
            try
            {
                IMSLineItem TransactionTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.PACKAGE_COMMISSION_VOID : IMSLineItem.PACKAGE_COMMISSION;
                detail = new AccountingManager().CreateDetail(header.Id, (int)TransactionTransactionType, transaction.Id, baseAmount, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create amount detail for IMS - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_PACKAGE_COMMISSION_IMS_FAILED, db);
                return false;
            }

            //Tax detail
            try
            {
                IMSLineItem TaxTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.PACKAGE_COMMISSION_TAX_VOID : IMSLineItem.PACKAGE_COMMISSION_TAX;
                detail = new AccountingManager().CreateDetail(header.Id, (int)TaxTransactionType, transaction.Id, taxes, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create taxes detail for IMS - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_TAX_IMS_FAILED, db);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This is the Create Header and Detail for the merchant
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="enterprise"></param>
        /// <param name="location"></param>
        /// <param name="terminal"></param>
        /// <param name="amount"></param>
        /// <param name="baseAmount"></param>
        /// <param name="taxes"></param>
        /// <param name="tips"></param>
        /// <param name="ProcessDate"></param>
        private bool CreateHeaderDetail(TrxFinancialTransaction transaction, Enterprise enterprise, Location location, Terminal terminal, Decimal amount, Decimal baseAmount, Decimal taxes, Decimal tips, DateTime ProcessDate, IMSEntities db)
        {
            IMS_Header header;
            IMS_Detail detail;
            Int64 PortalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
            Int32 CurrencyId = enterprise.Address.Country.CurrencyId;
            header = db.IMS_Header.Where(
                    a => a.EnterpriseId == enterprise.Id &&
                    a.LocationId == location.Id &&
                    DbFunctions.TruncateTime(a.CreationDate) == DbFunctions.TruncateTime((DateTime)transaction.localDateTime)
                    ).FirstOrDefault();

            //Create header
            if (header == null)
            {
                try
                {
                    header = new AccountingManager().CreateHeader(enterprise.Id, location.MerchantId, location.Id, null, null, null, terminal.Id, PortalId, amount, CurrencyId, (DateTime)transaction.localDateTime, ProcessDate, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error create header for merchant - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_HEADER_MERCHANT_FAILED, db);
                    return false;
                }
            }
            else
            {
                //FV20171010 If Opening Amount is negative, we add the tips to the amount otherwise we leave it like that
                if (header.OpeningAmount < 0)
                    amount = amount + tips;

                try
                {
                    header = new AccountingManager().AdjustHeader(header, amount, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error adjust header for merchant - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.ADJUST_HEADER_MERCHANT_FAILED, db);
                    return false;
                }
            }

            //Create detail for amount
            try
            {
                IMSLineItem TransactionTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.MERCHANT_REIMBURSEMENT_VOID : IMSLineItem.MERCHANT_REIMBURSEMENT;
                detail = new AccountingManager().CreateDetail(header.Id, (int)TransactionTransactionType, transaction.Id, baseAmount, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create amount detail for merchant - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_MERCHANT_REIMBURSMENT_FAILED, db);
                return false;
            }

            //Create detail for taxes
            try
            {
                IMSLineItem TaxTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.MERCHANT_TAX_REIMBURSEMENT_VOID : IMSLineItem.MERCHANT_TAX_REIMBURSEMENT;
                detail = new AccountingManager().CreateDetail(header.Id, (int)TaxTransactionType, transaction.Id, taxes, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create taxes detail for merchant - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_TAX_MERCHANT_FAILED, db);
                return false;
            }

            //Create detail for tips
            try
            {
                if (tips != 0)
                {
                    IMSLineItem TipsTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.MERCHANT_TIPS_REIMBURSEMENT_VOID : IMSLineItem.MERCHANT_TIPS_REIMBURSEMENT;
                    detail = new AccountingManager().CreateDetail(header.Id, (int)TipsTransactionType, transaction.Id, tips, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create tips detail for merchant - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_TIP_MERCHANT_FAILED, db);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This is the Create Header and Detail for the SalesRep
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="enterprise"></param>
        /// <param name="salesRep"></param>
        /// <param name="terminal"></param>
        /// <param name="amount"></param>
        /// <param name="baseAmount"></param>
        /// <param name="ProcessDate"></param>
        private bool CreateHeaderDetail(TrxFinancialTransaction transaction, Enterprise enterprise, SalesRep salesRep, Terminal terminal, Decimal amount, Decimal baseAmount, DateTime ProcessDate, IMSEntities db)
        {
            IMS_Header header;
            IMS_Detail detail;
            Int64 PortalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
            Int32 CurrencyId = enterprise.Address.Country.CurrencyId;

            header = db.IMS_Header.Where(
                a => a.EnterpriseId == enterprise.Id &&
                a.SalesRepId == salesRep.Id &&
                DbFunctions.TruncateTime(a.CreationDate) == DbFunctions.TruncateTime((DateTime)transaction.localDateTime)
                ).FirstOrDefault();

            if (header == null)
                try
                {
                    header = new AccountingManager().CreateHeader(enterprise.Id, null, null, null, salesRep.Id, null, null, PortalId, amount, CurrencyId, (DateTime)transaction.localDateTime, ProcessDate, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error create header for sales rep - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_HEADER_SALESREP_FAILED, db);
                    return false;
                }

            else
                try
                {
                    header = new AccountingManager().AdjustHeader(header, amount, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error adjust header for sales rep - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.ADJUST_HEADER_SALESREP_FAILED, db);
                    return false;
                }

            try
            {
                IMSLineItem SalesRepTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.SALESREP_COMMISSION_VOID : IMSLineItem.SALESREP_COMMISSION;
                detail = new AccountingManager().CreateDetail(header.Id, (int)SalesRepTransactionType, transaction.Id, amount, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create detail for sales rep - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_SALESREP_COMMISSION_FAILED, db);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This is the Create Header and Detail for the outside channel
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="enterprise"></param>
        /// <param name="salesRep"></param>
        /// <param name="terminal"></param>
        /// <param name="amount"></param>
        /// <param name="baseAmount"></param>
        /// <param name="ProcessDate"></param>
        private bool CreateHeaderDetail(TrxFinancialTransaction transaction, Enterprise enterprise, OutsideChannel outsideChannel, Terminal terminal, Decimal amount, Decimal baseAmount, DateTime ProcessDate, IMSEntities db)
        {
            IMS_Header header;
            IMS_Detail detail;
            Int64 PortalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
            Int32 CurrencyId = enterprise.Address.Country.CurrencyId;

            header = db.IMS_Header.Where(
                a => a.EnterpriseId == enterprise.Id &&
                a.OutsideChannelId == outsideChannel.Id &&
                DbFunctions.TruncateTime(a.CreationDate) == DbFunctions.TruncateTime((DateTime)transaction.localDateTime)
                ).FirstOrDefault();

            if (header == null)
                try
                {
                    header = new AccountingManager().CreateHeader(enterprise.Id, null, null, outsideChannel.Id, null, null, null, PortalId, amount, CurrencyId, (DateTime)transaction.localDateTime, ProcessDate, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error create header for outside channel - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_HEADER_OUTSIDECHANNEL_FAILED, db);
                    return false;
                }

            else
                try
                {
                    header = new AccountingManager().AdjustHeader(header, amount, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error adjust header for outside channel - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.ADJUST_HEADER_OUTSIDECHANNEL_FAILED, db);
                    return false;
                }

            try
            {
                IMSLineItem OutsideChannelTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.OUTSIDECHANNEL_COMMISSION_VOID : IMSLineItem.OUTSIDECHANNEL_COMMISSION;
                detail = new AccountingManager().CreateDetail(header.Id, (int)OutsideChannelTransactionType, transaction.Id, amount, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create detail for outside channel - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_OUTSIDECHANNEL_COMMISSION_FAILED, db);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This is the Create Header and Detail for the Contract Commission (outside rep)
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="enterprise"></param>
        /// <param name="contractCommission"></param>
        /// <param name="terminal"></param>
        /// <param name="amount"></param>
        /// <param name="baseAmount"></param>
        /// <param name="ProcessDate"></param>
        private bool CreateHeaderDetail(TrxFinancialTransaction transaction, Enterprise enterprise, ContractCommission contractCommission, Terminal terminal, Decimal amount, Decimal baseAmount, DateTime ProcessDate, IMSEntities db)
        {
            IMS_Header header;
            IMS_Detail detail;
            Int64 PortalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
            Int32 CurrencyId = enterprise.Address.Country.CurrencyId;

            header = db.IMS_Header.Where(
                a => a.EnterpriseId == enterprise.Id &&
                a.ContractCommissionId == contractCommission.Id &&
                DbFunctions.TruncateTime(a.CreationDate) == DbFunctions.TruncateTime((DateTime)transaction.localDateTime)
                ).FirstOrDefault();

            if (header == null)
                try
                {
                    header = new AccountingManager().CreateHeader(enterprise.Id, null, null, null, null, contractCommission.Id, null, PortalId, amount, CurrencyId, (DateTime)transaction.localDateTime, ProcessDate, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error create header for outside rep - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_HEADER_OUTSIDEREP_FAILED, db);
                    return false;
                }
            else
                try
                {
                    header = new AccountingManager().AdjustHeader(header, amount, db);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error adjust header for outside rep - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.ADJUST_HEADER_OUTSIDEREP_FAILED, db);
                    return false;
                }

            try
            {
                IMSLineItem SalesRepTransactionType = (transaction.transactionTypeId == (int)TransaxTransactionType.VoidSale || transaction.transactionTypeId == (int)TransaxTransactionType.VoidConclusion) ? IMSLineItem.CONTRACT_COMMISSION_VOID : IMSLineItem.CONTRACT_COMMISSION;
                detail = new AccountingManager().CreateDetail(header.Id, (int)SalesRepTransactionType, transaction.Id, amount, (int)IMSTransactionType.TRANSAX, transaction.description, (DateTime)transaction.localDateTime, db);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error create detail for outside rep - TransactionId {0} Exception {1}", transaction.Id, ex.ToString());
                LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, (int)IMS.Common.Core.Enumerations.IMSTranferMessage.CREATE_DETAIL_OUTSIDEREP_COMMISSION_FAILED, db);
                return false;
            }

            return true;
        }

        private bool transactionIsValid(TrxFinancialTransaction transaction, IMSEntities db)
        {
            Boolean isValid = true;
            Int32? messageId = null;

            if (transaction.baseAmount == null)
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_BASE_AMOUNT;
            }

            if (transaction.aditionalAmount == null)
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_ADDITIONAL_AMOUNT;
            }

            if (transaction.approvedAmount == null)
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_APPROVED_AMOUNT;
            }

            if (string.IsNullOrEmpty(transaction.entityId))
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_ENTITY_ID;
            }

            if (transaction.localDateTime == null)
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_LOCAL_DATETIME;
            }

            if (transaction.currencyId == null)
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_CURRENCY_ID;
            }

            if (transaction.transactionTypeId == null)
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_TRANSACTION_TYPE_ID;
            }

            if (transaction.legTransaction == null)
            {
                isValid = false;
                messageId = (int)IMS.Common.Core.Enumerations.IMSTranferMessage.MISSING_LEG_TRANSACTION_ID;
            }

            if (!string.IsNullOrEmpty(messageId.ToString()))
            {
                try
                {
                    LogTransactionTransfer(transaction, (int)IMS.Common.Core.Enumerations.IMSTransferStatus.FAILED, Convert.ToInt32(messageId), db);
                }
                catch (Exception ex)
                {

                }
            }

            return isValid;
        }

        private DateTime GetDateWithDelayForProcessDate(DateTime dateToProcess)
        {
            Int64 daysToAdd = Convert.ToInt64(ConfigurationManager.AppSettings["DelayToProcessTransactionInOpeningDays"]);

            switch (dateToProcess.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dateToProcess = dateToProcess.AddDays(daysToAdd);
                    break;
                case DayOfWeek.Tuesday:
                    dateToProcess = dateToProcess.AddDays(daysToAdd);
                    break;
                case DayOfWeek.Wednesday:
                    dateToProcess = dateToProcess.AddDays(daysToAdd);
                    break;
                case DayOfWeek.Thursday:
                    dateToProcess = dateToProcess.AddDays(daysToAdd + 2);
                    break;
                case DayOfWeek.Friday:
                    dateToProcess = dateToProcess.AddDays(daysToAdd + 2);
                    break;
                case DayOfWeek.Saturday:
                    dateToProcess = dateToProcess.AddDays(daysToAdd + 2);
                    break;
                case DayOfWeek.Sunday:
                    dateToProcess = dateToProcess.AddDays(daysToAdd + 1);
                    break;
                default:
                    break;
            }

            return dateToProcess;
        }

        private decimal GetCommissionPercentage(Promotion promo, Contract contract, TrxFinancialTransaction transaction)
        {
            decimal commission = 0;

            if (promo != null)
            {
                //We check if promo was scheduled for that day
                if (promo.Promotion_Schedules.Any(a => a.StartDate <= transaction.localDateTime && a.EndDate >= transaction.localDateTime && a.IsActive))
                {
                    commission = Convert.ToDecimal(promo.Promotion_Schedules.Where(a => a.StartDate <= transaction.localDateTime && a.EndDate >= transaction.localDateTime && a.IsActive == true).FirstOrDefault().Value);

                    //We check if there was an override on the package commission
                    if (promo.Package.PackageCommission != null)
                    {
                        commission += Convert.ToDecimal(promo.Package.PackageCommission);
                    }
                    else //No override, we take the base commission for that package
                    {
                        commission += Convert.ToDecimal(promo.Package.PackageType.PackageCommission);
                    }
                }
                else
                {
                    commission = Convert.ToDecimal(promo.Package.PackageType.MinCommissionAllYear);
                }
            }
            else //No promo, we take the all year commission from the contract
            {
                if (contract != null)
                {
                    commission = Convert.ToDecimal(contract.Packages.Where(a => a.IsActive == true).FirstOrDefault().PackageType.MinCommissionAllYear);
                }
                else //No contract, we take the base commission
                {
                    decimal baseCommission = Convert.ToDecimal(ConfigurationManager.AppSettings["minPercentageCommission"]);
                    commission = Convert.ToDecimal(baseCommission);
                }
            }

            return commission >= 1 ? commission / 100 : commission;
        }

        private void LogTransactionTransfer(TrxFinancialTransaction transaction, Int32 statusId, Int32 messageId, IMSEntities db)
        {
            transaction.IMSTransferStatusId = statusId;
            transaction.IMSTransferMessageId = messageId;
            db.Entry(transaction).State = EntityState.Modified;
            db.SaveChanges();
        }

        #endregion
    }
}
