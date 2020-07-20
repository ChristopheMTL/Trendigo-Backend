using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace IMS.Service.WebAPI2.Models
{
    #region Member Section

    #region Creation Section

    public class createMemberRQ
    {
        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string email { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        public bool passwordNotSet { get; set; }

        [Required]
        public string language { get; set; }

        public string uid { get; set; }

        public string provider { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("RegisterNewMemberModel {\n");
            sb.Append("  Email: ").Append(email).Append("\n");
            sb.Append("  FirstName: ").Append(firstName).Append("\n");
            sb.Append("  LastName: ").Append(lastName).Append("\n");
            sb.Append("  Language: ").Append(language).Append("\n");
            sb.Append("  Password: ").Append(password).Append("\n");
            sb.Append("  UID: ").Append(uid).Append("\n");
            sb.Append("  Provider: ").Append(provider).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class createMemberRS
    {
        public long memberId { get; set; }

        public int transaxId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string language { get; set; }

        public string uid { get; set; }

        public string provider { get; set; }
    }

    #endregion

    #region Modification Section

    public class UpdateMemberRQ
    {
        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string language { get; set; }

        public string avatar { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("UpdateMemberRQ {\n");

            sb.Append("  FirstName: ").Append(firstName).Append("\n");
            sb.Append("  LastName: ").Append(lastName).Append("\n");
            sb.Append("  Email: ").Append(email).Append("\n");
            sb.Append("  Language: ").Append(language).Append("\n");
            sb.Append("  Avatar: ").Append(avatar).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }


    #endregion

    #region Authentication Section

    public class LoginMemberRQ
    {
        [Required]
        public string email { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        public string deviceId { get; set; }

        [Required]
        public string notificationToken { get; set; }

        public string uid { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("LoginMemberRQ {\n");
            sb.Append("  Email: ").Append(email).Append("\n");
            sb.Append("  Password: ").Append(password).Append("\n");
            sb.Append("  DeviceId: ").Append(deviceId).Append("\n");
            sb.Append("  NotificationToken: ").Append(notificationToken).Append("\n");
            sb.Append("  UID: ").Append(uid).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class SocialLoginRQ
    {
        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string language { get; set; }

        [Required]
        public string deviceId { get; set; }

        [Required]
        public string uid { get; set; }

        [Required]
        public string provider { get; set; }

        [Required]
        public string notificationToken { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("RegisterNewMemberModel {\n");
            sb.Append("  Email: ").Append(email).Append("\n");
            sb.Append("  DeviceId: ").Append(deviceId).Append("\n");
            sb.Append("  UID: ").Append(uid).Append("\n");
            sb.Append("  Provider: ").Append(provider).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class LoginMemberRS
    {
        public long memberId { get; set; }
        public string sessionToken { get; set; }
        public string language { get; set; }
        public bool isCreditCardExist { get; set; }
    }

    public class EmailValidationRQ
    {
        [Required]
        public string code { get; set; }
    }

    #endregion

    #region Notification Section

    public class NotificationRQ
    {
        [Required]
        public string deviceId { get; set; }

        [Required]
        public string notificationToken { get; set; }
    }

    public class UpdateMemberNotificationRQ
    {
        public string deviceId { get; set; }

        public string notificationToken { get; set; }
    }

    #endregion

    #region Password Management Section

    public class ForgotPasswordRQ
    {
        [Required]
        public string email { get; set; }
    }

    public class ResetPasswordRQ
    {
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        public string code { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ResetPasswordRQ {\n");
            sb.Append("  Password: ").Append(password).Append("\n");
            sb.Append("  Code: ").Append(code).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class ChangePasswordRQ
    {
        [Required]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ChangePasswordRQ {\n");
            sb.Append("  oldPassword: ").Append(oldPassword).Append("\n");
            sb.Append("  newPassword: ").Append(newPassword).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    #endregion

    #endregion

    #region Credit Card Section

    public class CreditCardRQ
    {
        [Required]
        public int creditCardTypeId { get; set; }

        [Required]
        public string cardHolderName { get; set; }

        [Required]
        public string cardNumber { get; set; }

        [Required]
        public string expiryDate { get; set; }

        [Required]
        public string token { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CreditCardRQ {\n");
            sb.AppendLine("cardHolderName=" + cardHolderName);
            sb.AppendLine("cardNumber=" + cardNumber);
            sb.AppendLine("expiryDate=" + expiryDate);
            sb.AppendLine("creditCardTypeId=" + creditCardTypeId);
            sb.AppendLine("Token=" + token);
            sb.Append("}\n");

            return sb.ToString();
        }
    }

    public class CreditCardRS
    {
        public long creditCardId { get; set; }
        public long transaxId { get; set; }
        public long memberId { get; set; }
        public int creditCardTypeId { get; set; }
        public string cardholderName { get; set; }
        public string cardNumber { get; set; }
        public string expiryDate { get; set; }
    }

    public class UpdateCreditCardRQ
    {
        public long creditCardId { get; set; }
        public long transaxId { get; set; }
        public int creditCardTypeId { get; set; }
        public string cardholderName { get; set; }
        public string expiryDate { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UpdateCreditCardRQ {\n");
            sb.AppendLine("creditCardId=" + creditCardId);
            sb.AppendLine("transaxId=" + transaxId);
            sb.AppendLine("creditCardTypeId=" + creditCardTypeId);
            sb.AppendLine("cardHolderName=" + cardholderName);
            sb.AppendLine("expiryDate=" + expiryDate);
            sb.Append("}\n");

            return sb.ToString();
        }
    }

    #endregion

    #region Transaction Section

    public class MemberTransactionHistoryRS
    {
        public long invoiceId { get; set; }
        public DateTime date { get; set; }
        public long merchantId { get; set; }
        public string merchant { get; set; }
        public decimal amount { get; set; }
        public decimal reward { get; set; }
        public int pointUsed { get; set; }
    }
    public class MemberInvoicesRS
    {
        public int totalCount { get; set; }
        public List<MemberInvoiceTransactionRS>  invoiceTransaction { get; set; }
    }

    public class MemberInvoiceTransactionRS
    {
        public string merchantName { get; set; }
        public string merchantAddress { get; set; }
        public string merchantCity { get; set; }
        public string merchantState { get; set; }
        public string merchantZip { get; set; }
        public string merchantPhone { get; set; }
        public long invoiceId { get; set; }
        public string orderNumber { get; set; }
        public int transactionTypeId { get; set; }
        public DateTime date { get; set; }
        public string collaborator { get; set; }
        public decimal amount { get; set; }
        public decimal tip { get; set; }
        public decimal total { get; set; }
        public int reward { get; set; }
        public int pointUsed { get; set; }
        public string cardType { get; set; }
        public string cardNumber { get; set; }
        public decimal cardTransactionAmount { get; set; }
        public string authorization { get; set; }
    }

    public class MemberPaymentRS
    {
        public int totalCount { get; set; }
        public List<MemberPaymentHistoryRS> paymentHistory { get; set; }

    }

    public class MemberPaymentHistoryRS
    {
        public long invoiceId { get; set; }
        public DateTime date { get; set; }
        public long merchantId { get; set; }
        public string merchant { get; set; }
        public string merchantLogo{ get; set; }
        public string locationAddress { get; set; }
        public string locationCity { get; set; }
        public string locationState { get; set; }
        public string locationCountry { get; set; }
        public string locationZip { get; set; }
        public decimal amount { get; set; }
        public decimal point { get; set; }
        public bool isEarned { get; set; }
    }
    #endregion

    #region Message Section

    public class getMessageRS
    {
        public long messageId { get; set; }
        public string title { get; set; }
        public string message { get; set; }
    }

    #endregion
}