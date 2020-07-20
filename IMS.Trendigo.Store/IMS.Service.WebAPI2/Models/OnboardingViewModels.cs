using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.Service.WebAPI2.Models
{
    public class OnboardingUserRQ
    {
        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string email { get; set; }

        public string language { get; set; }
    }

    public class OnboardingMerchantRQ
    {
        [Required]
        public long merchantAdminId { get; set; }

        [Required]
        public string name { get; set; }


        [Required]
        public string streetAddress { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public int stateId { get; set; }

        [Required]
        public int countryId { get; set; }

        [Required]
        public string zip { get; set; }

        [Required]
        public string phone { get; set; }
    }

    public class OnboardingMerchantInformationRQ
    {
        public List<OnboardingMerchantLocaleRQ> locales { get; set; }
        [Required]
        public int baseReward { get; set; }
        [Required]
        public bool acceptTips { get; set; }
        [Required]
        public int categoryId { get; set; }
        public List<OnboardingMerchantTagRQ> tags { get; set; }        
    }

    public class OnboardingMerchantLocaleRQ
    {
        [Required]
        public string language { get; set; }
        [Required]
        public string description { get; set; }
    }

    public class OnboardingMerchantTagRQ
    {
        [Required]
        public int tagId { get; set; }
    }

    public class OnboardingBusinessHoursAndHolidaysRQ
    {
        [Required]
        public List<OnboardingBusinessHourRQ> hours { get; set; }
        public List<OnboardingHolidayRQ> holidays { get; set; }
    }

    public class OnboardingBusinessHourRQ
    {
        [Required]
        public int dayOfWeek { get; set; }
        [Required]
        public string openingHour { get; set; }
        [Required]
        public string closingHour { get; set; }
        public bool isClosed { get; set; }
    }

    public class OnboardingHolidayRQ
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class OnboardingUpdateMerchantRQ
    {
        public long merchantId { get; set; }
        public string name { get; set; }
        public List<OnboardingUpdateMerchantLocationRQ> locations { get; set; }
    }

    public class OnboardingUpdateMerchantLocationRQ
    {
        public long locationId { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public int stateId { get; set; }
        public int countryId { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
    }
}