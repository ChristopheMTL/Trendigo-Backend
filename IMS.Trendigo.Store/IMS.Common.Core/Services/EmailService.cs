using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using IMS.Common.Core.Data;
using Microsoft.AspNet.Identity;
using IMS.Common.Core.Enumerations;
using System.Web.Mvc;
using IMS.Common.Core.Utilities;
using System.Data.Entity;

namespace IMS.Common.Core.Services
{
    public class EmailService : IIdentityMessageService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _login;
        private readonly string _pwd;
        private readonly string _sentFrom;

        public EmailService()
        {
            _host =     ConfigurationManager.AppSettings["IMS.Common.Core.Smtp.Host"];
            _port =     Convert.ToInt32(ConfigurationManager.AppSettings["IMS.Common.Core.Smtp.Port"]);
            _login =    ConfigurationManager.AppSettings["IMS.Common.Core.Smtp.Login"];
            _pwd =      ConfigurationManager.AppSettings["IMS.Common.Core.Smtp.Password"];
            _sentFrom = ConfigurationManager.AppSettings["IMS.Common.Core.Smtp.From"];
        }

        IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Send Identity Message (Email validation, etc)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            // Create the message:
            var mail = new System.Net.Mail.MailMessage(_sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:
            return SendAsync(mail);
        }

        public Task SendAsync(MailMessage message)
        {
            if (message.From == null)
                message.From = new MailAddress(_sentFrom);

            // Configure the client:
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(_host, _port);

            client.Port = _port;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_login, _pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            return client.SendMailAsync(message);
        }

        /// <summary>
        /// This method send a email with a link to reset the password
        /// </summary>
        /// <param name="member">Member information</param>
        /// <param name="email">Email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="textLink">Text that will appear in the button</param>
        /// <param name="link">Link to access the forgot password page</param>
        /// <returns>Nothing</returns>
        public Task SendForgotPasswordEmail(Member member, String email, String subject, String textLink, String link)
        {
            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/ForgotPasswordEmail.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = member.FirstName + " " + member.LastName;
                htmlBody = String.Format(htmlBody, fullname, link);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);

        }

        /// <summary>
        /// This method send a email with a link to reset the password
        /// </summary>
        /// <param name="member">Member information</param>
        /// <param name="email">Email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="textLink">Text that will appear in the button</param>
        /// <param name="link">Link to access the forgot password page</param>
        /// <returns>Nothing</returns>
        public Task SendForgotPasswordEmail(IMSUser user, String email, String subject, String textLink, String link)
        {
            string language = "en";
            //if (user.Language != null)
            //    language = user.Language;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/ForgotPasswordEmail.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = user.FirstName + " " + user.LastName;
                htmlBody = String.Format(htmlBody, fullname, link);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);

        }

        /// <summary>
        /// This method send an email with a link to confirm the email address
        /// </summary>
        /// <param name="member"></param>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="textLink"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public Task SendConfirmEmailAddressEmail(Member member, String email, String subject, String textLink, String link)
        {
            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/EmailValidation.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string firstName = member.FirstName;
                htmlBody = String.Format(htmlBody, firstName, link);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);

        }

        /// <summary>
        /// This method send an email with a link to confirm the email address
        /// </summary>
        /// <param name="member"></param>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="textLink"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public Task SendConfirmEmailAddressEmail(IMSUser user, String email, String subject, String textLink, String link)
        {
            string language = "en";
            if (user.Language != null)
                language = user.Language.ISO639_1;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/EmailValidation.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string firstName = user.FirstName;
                htmlBody = String.Format(htmlBody, firstName, link);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);

        }

        /// <summary>
        /// This method will send an email receipt for a subscription
        /// </summary>
        /// <param name="member">Member information</param>
        /// <param name="membership">Membership information</param>
        /// <param name="NewMembership">Validate if the receipt is for a new subscription or a renewal of an existing subscription</param>
        /// <returns>Nothing</returns>
        public Task SendSubscriptionReceiptEmail(Member member, IMSMembership membership, Boolean NewMembership)
        {
            //#region Validation Section

            //if (member == null) 
            //{
            //    //logger.ErrorFormat("SendSubscriptionReceiptEmail - member not present for membership id {0}", membership.Id);
            //    throw new Exception("Member not present");
            //}

            //Address address = member.Address;

            //if (address == null) 
            //{
            //    logger.ErrorFormat("SendSubscriptionReceiptEmail - address not present for member id {0}", member.Id);
            //    return null;
            //}

            //if (membership == null) 
            //{
            //    logger.ErrorFormat("SendSubscriptionReceiptEmail - membership not present for member id {0}", member.Id);
            //    return null;
            //}

            //Program program = membership.Program;

            //if (program == null) 
            //{
            //    logger.ErrorFormat("SendSubscriptionReceiptEmail - program not present for member id {0}", member.Id);
            //    return null;
            //}

            //MembershipTransactionDetail membershipTransactionDetail = membership.MembershipTransactionDetails.OrderByDescending(a => a.CreationDate).FirstOrDefault();

            //if (membershipTransactionDetail == null) 
            //{
            //    throw new Exception("membershipTransactionDetail not present");
            //    //logger.ErrorFormat("SendSubscriptionReceiptEmail - membershipTransactionDetail not present for membership id {0}", membership.Id);
            //}

            //DateTime _transactionDate = membershipTransactionDetail.CreationDate;

            //ProgramFee programFee = membership.Program.ProgramFees.OrderByDescending(a => a.CreationDate).FirstOrDefault(a => a.CreationDate <= _transactionDate);

            //if (programFee == null) 
            //{
            //    logger.ErrorFormat("SendSubscriptionReceiptEmail - programmFee not present for program id {0}", program.Id);
            //    return null;
            //}

            //CreditCard creditCard = member.CreditCards.FirstOrDefault(a => a.defaultCard == true);

            //if (creditCard == null) 
            //{
            //    logger.ErrorFormat("SendSubscriptionReceiptEmail - creditCard not present for member id {0}", member.Id);
            //    return null;
            //}

            //#endregion

            //try 
            //{
            //    string language = "en";
            //    if (member.Language != null)
            //        language = member.Language.ISO639_1;

            //    string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format(NewMembership == true ? "Views/Email/SubscriptionReceipt.{0}.html" : "Views/Email/RenewalReceipt.{0}.html", language));

            //    string htmlBody;

            //    using (var fs = File.OpenRead(emailBodyTemplateName))
            //    using (var sr = new StreamReader(fs))
            //    {
            //        htmlBody = sr.ReadToEnd();
            //        //0
            //        string fullname = member.FirstName + " " + member.LastName;
            //        //1
            //        string receipt = membershipTransactionDetail.Id.ToString();
            //        //2
            //        string approval = membershipTransactionDetail.ApprovalNbr;
            //        //3
            //        string transactionDate = _transactionDate.ToString("yyyy/MM/dd hh:mm:ss");
            //        //4 fullname
            //        //5
            //        string invoiceAddress = address.StreetAddress + " " + address.App;
            //        //6
            //        string city_state_zip = address.City + ", " + address.State.Name + ", " + address.Zip;
            //        //7
            //        string invoiceCountry = address.Country.Name;
            //        //8
            //        string invoiceMobile = string.IsNullOrEmpty(member.Phone) ? "n/d" : member.Phone;
            //        //9
            //        string cardName = creditCard.CardHolder;
            //        //10
            //        string cardType = creditCard.CreditCardType.Description;
            //        //11
            //        string cardNumber = creditCard.CardNumber;
            //        //4
            //        string deliveryName = fullname;
            //        //12
            //        string membershipType = program.ShortDescription;
            //        //13
            //        decimal _amount = programFee.AssociatedFees;
            //        string amount = string.Format("{0:F2}", _amount);
            //        //14
            //        decimal _promoCodeValue = Math.Round(membership.ProgramID > 0 ? membership.PromoCode.Value : 0, 2);
            //        decimal _subtotal = _amount - _promoCodeValue;
            //        string promoCodeValue = string.Format("{0:F2}", _promoCodeValue);
            //        //23
            //        string symbol = address.State.Country.Currency.Symbol;
            //        //16
            //        string tax1Name = "TPS/GST";
            //        StateTax stateTax1 = address.State.StateTaxes.FirstOrDefault(a => a.Code == tax1Name && a.IsActive == true);
            //        //17
            //        string tax1Number = stateTax1.EnterpriseStateTaxNumbers.FirstOrDefault(a => a.IsActive).TaxNumber;
            //        //18
            //        decimal membership1_gst = Convert.ToDecimal(stateTax1.Value);
            //        decimal _tax1Amount = Math.Round(new UtilityManager().getTaxForAmount(_subtotal, membership1_gst), 2);
            //        //21
            //        string tax2Name = "TVQ/QST";
            //        //20
            //        StateTax stateTax2 = address.State.StateTaxes.FirstOrDefault(a => a.Code == tax2Name && a.IsActive == true);
            //        string tax2Number = stateTax2.EnterpriseStateTaxNumbers.FirstOrDefault(a => a.IsActive).TaxNumber;
            //        //19
            //        decimal membership2_qst = Convert.ToDecimal(address.State.StateTaxes.FirstOrDefault(a => a.Code == tax2Name).Value);
            //        decimal _tax2Amount = Math.Round(new UtilityManager().getTaxForAmount(_subtotal, membership2_qst), 2);
            //        //15
            //        string subtotal = String.Format("{0:F2}", _subtotal);
            //        //22
            //        string total = string.Format("{0:F2}", membership.MembershipTransactionDetails.FirstOrDefault().Amount);

            //        htmlBody = String.Format(htmlBody, fullname, receipt, approval, transactionDate, fullname, invoiceAddress, city_state_zip, invoiceCountry, invoiceMobile, cardName, cardType, cardNumber, membershipType, amount, promoCodeValue, subtotal, tax1Name, tax1Number, String.Format("{0:F2}", _tax1Amount), tax2Name, tax2Number, String.Format("{0:F2}", _tax2Amount), total, symbol);
            //    }

            //    MailMessage mail = new MailMessage();
            //    mail.To.Add(member.AspNetUser.Email);
            //    mail.Body = htmlBody;
            //    mail.IsBodyHtml = true;

            //    return SendAsync(mail);
            //}
            //catch (Exception ex) 
            //{
            //    logger.ErrorFormat("SendSubscriptionReceiptEmail - unable to send subscription email for member id {0} membership id {1} exception {2}", member.Id, membership.Id, ex.ToString());
            //    return null;
            //}

            throw new NotImplementedException();
        }

        /// <summary>
        /// This method send an email with a virtual card number when an account is created
        /// </summary>
        /// <param name="member">Member information</param>
        /// <param name="email">Email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <returns>Nothing</returns>
        public Task SendWelcomeEmail(Member member, String email, String subject)
        {
            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/Welcome.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = member.FirstName + " " + member.LastName;
                htmlBody = String.Format(htmlBody, fullname);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantAdmin"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task SendWelcomeMerchantEmail(IMSUser merchantAdmin, String password)
        {
            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Views/Email/WelcomeB2B.html");

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = merchantAdmin.FirstName + " " + merchantAdmin.LastName;
                string username = merchantAdmin.AspNetUser.Email;
                htmlBody = String.Format(htmlBody, fullname, username, password);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = "Trendigo";
            mail.To.Add(merchantAdmin.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="financial"></param>
        /// <param name="nonFinancial"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task SendReceiptEmail(TrxFinancialTransaction financial, TrxNonFinancialTransaction nonFinancial, String filePath, Boolean sendReceipt = false, Boolean saveReceipt = false)
        {
            if (financial == null) 
            {
                return;
            }

            if (nonFinancial == null)
            {
                return;
            }

            if (financial.legTransaction == null)
            {
                //logger.ErrorFormat("SendReceiptEmail - Financial transaction {0} - No Non Financial transaction associated to this financial transaction", financial.Id);
                throw new Exception(string.Format("SendReceiptEmail - No Non Financial Transaction found for Financial Transaction {0}", financial.Id));
            }

            Location location = await db.Locations.FirstOrDefaultAsync(a => a.TransaxId == nonFinancial.entityId);

            if (location == null)
            {
                logger.ErrorFormat("NonFinancial transaction {0} - Location not found for entity {1}", nonFinancial.Id, nonFinancial.entityId);
                throw new Exception("Location not found");
            }

            CreditCard creditCard = await db.CreditCards.FirstOrDefaultAsync(a => a.TransaxId == financial.creditCardId.ToString());

            if (creditCard == null)
            {
                logger.ErrorFormat("Financial transaction {0} - Credit Card not found for id {1}", financial.Id, financial.creditCardId);
                throw new Exception("Credit Card not found");
            }

            Member member = await db.Members.FirstOrDefaultAsync(a => a.TransaxId == nonFinancial.memberId.ToString());

            if (member == null)
            {
                logger.ErrorFormat("NonFinancial transaction {0} - Member not found for Id {1}", nonFinancial.Id, nonFinancial.memberId);
                throw new Exception("Member not found");
            }

            ////Member does not want to receive a receipt by email
            //if (member.Receipt == null || (member.Receipt.HasValue && member.Receipt.Value == false))
            //{
            //    return;
            //}

            String currency = "$";
            Currency cur = db.Currencies.FirstOrDefault(a => a.TransaxId == financial.currencyId.Value.ToString());

            if (cur != null)
            {
                currency = cur.Symbol;
            }

            try
            {
                await SendReceiptEmail(location, member, creditCard, financial, nonFinancial, currency, filePath, sendReceipt, saveReceipt);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("SendEmailReceipt - Exception {0} InnerException {1}", ex.ToString(), ex.InnerException);
                throw new Exception(string.Format("SendEmailReceipt - Exception {0} InnerException {1}", ex.ToString(), ex.InnerException));
            }

            return;
        }

        /// <summary>
        /// This method send a receipt to the member of the transaction he just processed at a merchant
        /// </summary>
        /// <param name="location">The location from where the transaction was process</param>
        /// <param name="member">The member that process that transaction</param>
        /// <param name="creditCard">The card used to process the transaction</param>
        /// <param name="financial">The detail of the financial transaction</param>
        /// <param name="nonFinancial">The detail of the non financial transaction</param>
        /// <param name="currency">The currency used for that transaction</param>
        /// <param name="filePath">The file location of the receipt</param>
        /// <returns>Nothing</returns>
        public async Task SendReceiptEmail(Location location, Member member, CreditCard creditCard, TrxFinancialTransaction financial, TrxNonFinancialTransaction nonFinancial, String currency, String filePath, Boolean sendReceipt = false, Boolean saveReceipt = false)
        {
            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            string htmlBody = "";

            try 
            {
                htmlBody = await new TransactionManager().BuildReceipt(location, member, creditCard, financial, nonFinancial, currency, filePath);
            }
            catch (Exception ex) 
            {
                logger.ErrorFormat("EmailService - SendReceiptEmail - BuildReceipt Error - Exception {0}", ex.ToString());
            }

            if (!string.IsNullOrEmpty(htmlBody)) 
            {
                if (sendReceipt == true)
                {
                    MailMessage mail = new MailMessage();

                    string invoiceReceipt = language != "en" ? "Trendigo transaction #" : "Trendigo Receipt #";
                    string receipt2 = "";
                    if (nonFinancial.transactionTypeId == (int)TransaxTransactionType.Sale)
                    {
                        receipt2 = financial.Id.ToString().PadLeft(9, '0');
                    }
                    else
                    {
                        receipt2 = nonFinancial.Id.ToString().PadLeft(9, '0');
                    }
                    mail.Subject = invoiceReceipt + receipt2;
                    mail.To.Add(member.AspNetUser.Email);
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true;

                    await SendAsync(mail);
                }

                if (saveReceipt == true)
                {
                    try
                    {
                        IMSEntities context = new IMSEntities();
                        nonFinancial.receipt = htmlBody;
                        context.Entry(nonFinancial).State = System.Data.Entity.EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("SendReceiptEmail - AddReceipt for nonFinancial Id {0} - error:{1}", nonFinancial.Id.ToString(), ex.ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task SendDepositNotificationEmail(List<IMS_Detail> details, decimal total)
        {
            try 
            {
                string recipient = ConfigurationManager.AppSettings["IMS.Service.TransferTransaction.Notification.Recipient"];
                string subject = "Trendigo Dépot $" + total.ToString();

                string body = "<html><head></head><body><table><tr><td>Marchand</td><td>Montant</td><td>Date</td></tr>";

                foreach (IMS_Detail detail in details)
                {
                    body += "<tr><td>" + detail.Description + "</td><td>" + detail.Amount.ToString() + "</td><td>" + detail.CreationDate.ToString("yyyy/MM/dd") + "</td></tr>";
                }

                body += "</body></html>";

                MailMessage mail = new MailMessage();
                mail.Subject = subject;
                mail.To.Add(recipient);
                mail.Body = body;
                mail.IsBodyHtml = true;

                return SendAsync(mail);
            }
            catch (Exception ex) 
            {
                logger.ErrorFormat("SendDepositNotificationEmail - Exception {0}", ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// This method will send an email to notify that a deposit failed
        /// </summary>
        /// <param name="document">The deposit failure document</param>
        /// <param name="reference">The document reference for the deposit failure</param>
        /// <returns>Nothing</returns>
        public Task SendDepositFailedNotificationEmail(String document, String reference)
        {
            try
            {
                string _recipients = ConfigurationManager.AppSettings["IMS.Service.TransferTransaction.Notification.Recipient"];
                string[] recipients = _recipients.Split(',');
                string subject = "Deposit Failed Ref: " + reference;

                MailMessage mail = new MailMessage();
                mail.Subject = subject;
                foreach (string recipient in recipients) 
                {
                    mail.To.Add(recipient);
                }
                mail.Body = document;
                mail.IsBodyHtml = true;

                return SendAsync(mail);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("SendDepositFailedNotificationEmail - Exception {0}", ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public Task SendProfileCompletionEmail(Member member, String subject) 
        {
            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/ProfileCompletion.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = member.FirstName + " " + member.LastName;
                htmlBody = String.Format(htmlBody, fullname);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #region Share Promo Section

        public Task SendSharePromoEmail(Member member, string promotionlink, string fullName, string email)
        {
            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/SharePromotion.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string _fullname = fullName;
                string _member = member.FirstName + " " + member.LastName;
                string _promotionId = promotionlink;
                htmlBody = String.Format(htmlBody, _fullname, _member, _promotionId);
            }

            string subject = member.FirstName + " " + IMS.Common.Core.IMSMessages.ResourceManager.GetString("SharePromoEmailSubject_" + language);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region Referral section

        public Task SendReferralInvitationEmail(Member member, ReferralCampaign campaign, String email)
        {
            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/ReferralInvitation.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string invitee_fullname = member.FirstName + " " + member.LastName;
                string pointsToEarn = campaign.PointsToReferred.ToString();
                htmlBody = String.Format(htmlBody, invitee_fullname, email, pointsToEarn);
            }

            string subject = IMS.Common.Core.IMSMessages.ResourceManager.GetString("ReferralInvitationEmailSubject_" + language);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);

        }

        public Task SendReferralPointsEmail(Member member, int points, Boolean isReferrer) 
        {
            string emailBodyTemplateName;

            string language = "en";
            if (member.Language != null)
                language = member.Language.ISO639_1;

            if (isReferrer) 
            {
                emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/ReferrerPointsEmail.{0}.html", language));
            }
            else 
            {
                emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Views/Email/ReferredPointsEmail.{0}.html", language));
            }

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string _fullname = member.FirstName + " " + member.LastName;
                string _points = points.ToString();
                htmlBody = String.Format(htmlBody, _fullname, _points);
            }

            string subject = IMS.Common.Core.IMSMessages.ResourceManager.GetString("ReferralPointsEmailSubject_" + language);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region CardNotification Section

        public async Task SendCardNotificationEmail(Notification notification, Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Email/CardNotification.{0}.html", language));
            //string emailBodyTemplateName = String.Format("../../Email/CardNotification.{0}.html", language);

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = member.FirstName + " " + member.LastName;
                htmlBody = String.Format(htmlBody, fullname);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            await SendAsync(mail);

            try
            {
                IMSEntities context = new IMSEntities();
                notification.EmailContent = htmlBody;
                context.Entry(notification).State = System.Data.Entity.EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SendCardNotificationEmail - AddEmailContent for notificationId {0} Exception {1}", notification.Id.ToString(), ex.InnerException.ToString()));
            }
        }

        #endregion

        #region MonthlyBonusPoints Section

        public Task SendMonthlyBonusPointsNotificationEmail(String fullname, String email, String language, String subject, Int32 points)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Email/MonthlyBonusPointsNotification.{0}.html", language));
            //string emailBodyTemplateName = String.Format("../../Email/MonthlyBonusPointsNotification.{0}.html", language);

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                htmlBody = String.Format(htmlBody, points, fullname);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region CreditCard Section

        public Task SendCreditCardExpirationEmail(Member member, CreditCard card, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Email/CreditCardExpiration.{0}.html", language));

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = member.FirstName + " " + member.LastName;
                string cardNumber = card.CardNumber;
                htmlBody = String.Format(htmlBody, fullname, cardNumber);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region SurveyNoTransaction Section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="language"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public Task SendSurveyNoTransactionEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //string emailBodyTemplateName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, String.Format("Email/SurveyNoTransaction.{0}.html", language));
            string emailBodyTemplateName = String.Format("../../Email/SurveyNoTransaction.{0}.html", language);

            string htmlBody;

            using (var fs = File.OpenRead(emailBodyTemplateName))
            using (var sr = new StreamReader(fs))
            {
                htmlBody = sr.ReadToEnd();
                string fullname = member.FirstName + " " + member.LastName;
                htmlBody = String.Format(htmlBody, fullname);
            }

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region CalendarRenewal Section

        public async Task SendCalendarRenewalEmail(IMSUser user, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            string htmlBody = "";

            //try
            //{
            //    htmlBody = await new PromotionManager().BuildPromotionSchedulesCalendar( TransactionManager().BuildReceipt(location, member, assignedCard, financial, nonFinancial, currency, filePath);
            //}
            //catch (Exception ex)
            //{
            //    logger.ErrorFormat("EmailService - SendCalendarRenewal - BuildCalendar Error - Exception {0}", ex.ToString());
            //}

            //if (!string.IsNullOrEmpty(htmlBody))
            //{
            //    MailMessage mail = new MailMessage();

                
            //    mail.Subject = subject;
            //    mail.To.Add(user.AspNetUser.Email);
            //    mail.Body = htmlBody;
            //    mail.IsBodyHtml = true;

            //    await SendAsync(mail);

            //    try
            //    {
            //        IMSEntities context = new IMSEntities();
                    
            //        await context.SaveChangesAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(string.Format("SendReceiptEmail - AddReceipt for nonFinancial Id {0} - error:{1}", nonFinancial.Id.ToString(), ex.ToString()));
            //    }
            //}
        }

        #endregion

        #region HalfMemberNotification Section

        public Task SendHalfMemberFirstNotificationEmail(Member member, String language, String subject) 
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Get email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendHalfMemberSecondNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendHalfMemberSurveyNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region FullMemberNotification Section

        public Task SendFullMemberCardWaitingNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendFullMemberCardReceivedNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendFullMemberCardNotActivatedNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendFullMemberCardActivatedNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region NoTransactionNotification Section

        public Task SendNoTransactionFirstNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendNoTransactionSecondNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendNoTransactionThirdNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendNoTransactionSurveyNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion

        #region TransactionNotification Section

        public Task SendFirstTransactionNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendSecondTransactionNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendThirdTransactionNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        public Task SendNoFollowingTransactionNotificationEmail(Member member, String language, String subject)
        {
            language = string.IsNullOrEmpty(language) ? "en" : language;

            //*********************************************************
            //
            //  Build email template
            string emailBodyTemplate = "";
            //
            //*********************************************************

            string htmlBody;

            string fullname = member.FirstName + " " + member.LastName;
            htmlBody = String.Format(emailBodyTemplate, fullname);

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.To.Add(member.AspNetUser.Email);
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return SendAsync(mail);
        }

        #endregion
    }
}
