using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Service.WebAPI2.Models
{
    public class CreditCardTypeRS
    {
        public int creditCardTypeId { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
    }

    public class CreditCardIssuerRS
    {
        public string Issuer { get; set; }
        public int IIN_from { get; set; }
        public int IIN_to { get; set; }
        public int minLength { get; set; }
        public int maxLength { get; set; }
        public bool lunhValidation { get; set; }
        public int creditCardTypeId { get; set; }
    }
}