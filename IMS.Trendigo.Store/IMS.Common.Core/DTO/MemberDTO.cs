using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class MemberDTO
    {
        public long memberId { get; set; }
        public int transaxId { get; set; }
        public string userId { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string email { get; set; }
        
        public string password { get; set; }
        public bool passwordNotSet { get; set; }
        public string uid { get; set; }
        public string provider { get; set; }

        [Required]
        public string language { get; set; }
        public string avatar { get; set; }
        public List<MemberCreditCardDTO> creditCards { get; set; }
        public List<NotificationDTO> notifications { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("MemberDTO {\n");
            
            sb.Append("  FirstName: ").Append(firstName).Append("\n");
            sb.Append("  LastName: ").Append(lastName).Append("\n");
            sb.Append("  Email: ").Append(email).Append("\n");
            sb.Append("  Language: ").Append(language).Append("\n");
            sb.Append("  UID: ").Append(uid).Append("\n");
            sb.Append("  Provider: ").Append(provider).Append("\n");
            sb.Append("  Language: ").Append(language).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class MemberCreditCardDTO
    {
        public long creditCardId { get; set; }
        public int transaxId { get; set; }
        public int creditCardTypeId { get; set; }
        public string cardHolderName { get; set; }
        public string cardNumber { get; set; }
        public string expiryDate { get; set; }
    }

    public class NotificationDTO
    {
        public long notificationId { get; set; }
        public string message { get; set; }
        public List<NotificationTranslationDTO> locale { get; set; }
    }

    public class NotificationTranslationDTO
    {
        public string language { get; set; }
        public string message { get; set; }
    }
}
