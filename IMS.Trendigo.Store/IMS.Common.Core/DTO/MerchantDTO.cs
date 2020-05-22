using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class MerchantDTO
    {
        public long merchantId { get; set; }
        public long transaxId { get; set; }
        public long merchantAdminId { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public int promotion { get; set; }
        public string status { get; set; }
        public List<merchantImageDTO> images { get; set; }
        public MerchantBankingInfoDTO bankAccount { get; set; }
        public List<MerchantLocaleDTO> locales { get; set; }
        public List<MerchantLocationDTO> locations { get; set; }
        public List<MerchantTagDTO> tags { get; set; }
        public List<addUserDTO> clerks { get; set; }
    }

    public class merchantImageDTO
    {
        public string imageId { get; set; }
        public string path { get; set; }
    }

    public class MerchantBankingInfoDTO
    {
        public long bankAccountId { get; set; }
        public string accountName { get; set; }
        public string transit { get; set; }
        public string branch { get; set; }
        public string account { get; set; }
    }

    public class MerchantLocaleDTO
    {
        public long localeId { get; set; }
        public string merchantName { get; set; }
        public string merchantDesc { get; set; }
        public string locale { get; set; }
    }

    public class MerchantLocationDTO
    {
        public long locationId { get; set; }
        public long transaxId { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string phone { get; set; }
        //public List<locationTaxe> taxes { get; set; }
        public List<locationBusinessHourDTO> businessHours { get; set; }
    }

    public class locationBusinessHourDTO
    {
        public int businessHourId { get; set; }
        public int dayOfWeek { get; set; }
        public string openingHour { get; set; }
        public string closingHour { get; set; }
    }

    public class MerchantTagDTO
    {
        public int merchantTagId { get; set; }
        public string name { get; set; }
        public List<tagLocaleDTO> locale { get; set; }
    }

    public class tagLocaleDTO
    {
        public long tagLocaleId { get; set; }
        public string tagName { get; set; }
        public string locale { get; set; }
    }

    public class addUserDTO
    {
        public long userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string language { get; set; }
    }
}
