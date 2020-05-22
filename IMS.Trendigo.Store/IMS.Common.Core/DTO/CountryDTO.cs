using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class CountryDTO
    {
        public int countryId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public CurrencyDTO currency { get; set; }
        public List<CountryLocaleDTO> locale { get; set; }
    }
    
    public class CurrencyDTO
    {
        public int currencyId { get; set; }
        public int transaxId { get; set; }
        public string code { get; set; }
        public string symbol { get; set; }
    }

    public class CountryLocaleDTO
    {
        public int countryLocaleId { get; set; }
        public string language { get; set; }
        public string name { get; set; }
    }
}
