using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class CreditCardDTO
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string TransaxId { get; set; }
        public string CardHolder { get; set; }
        public int CreditCardTypeId { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Token { get; set; }

        public virtual CreditCardTypeDTO CreditCardType { get; set; }
    }

    public class CreditCardTypeDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
