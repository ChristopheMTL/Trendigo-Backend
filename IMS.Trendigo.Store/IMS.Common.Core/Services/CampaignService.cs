using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IMS.Common.Core.Enumerations;

namespace IMS.Common.Core.Services
{
    public class CampaignService
    {
        IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<String> GetCampaignNewsletterTemplate(Int32 campaignTypeId, Int32 languageId) 
        {
            //String newsletterTemplate = await db.NewsletterTranslations.Where(a => a.Newsletter.IsActive == true && a.LanguageId == languageId && a.Newsletter.Campaigns.Any(b => b.CampaignTypeId == campaignTypeId)).Select(c => c.Value).FirstOrDefaultAsync();

            //if(String.IsNullOrEmpty(newsletterTemplate))
            //    newsletterTemplate = await db.NewsletterTranslations.Where(a => a.Newsletter.IsActive == true && a.Language.ISO639_1 == "en" && a.Newsletter.Campaigns.Any(b => b.CampaignTypeId == campaignTypeId)).Select(c => c.Value).FirstOrDefaultAsync();

            //if (String.IsNullOrEmpty(newsletterTemplate))
            //    throw new Exception("Newsletter not found");

            //return newsletterTemplate;

            return null;
        }

        public async Task<List<CampaignType>> GetCampaignTypeList()
        {
            List<CampaignType> campaigntypes = new List<CampaignType>();

            campaigntypes = await db.CampaignTypes.Where(a => a.IsActive == true).OrderBy(a => a.Name).ToListAsync();

            return campaigntypes;
        }
    }
}
