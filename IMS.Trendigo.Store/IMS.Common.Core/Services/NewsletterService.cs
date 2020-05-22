using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class NewsletterService
    {
        IMSEntities db = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Newsletter AddNewsletterSections(Newsletter newsletter, Enterprise enterprise) 
        {
            if (newsletter.Campaigns.Count() > 0) 
            {
                foreach (Language language in enterprise.Languages)
                {
                    NewsletterContent content = new NewsletterContent();
                    content.CreationDate = DateTime.Now;
                    content.NewsletterId = newsletter.Id;
                    content.LanguageId = language.Id;

                    StringBuilder sb = new StringBuilder();
                    
                    foreach (TemplateSection ts in newsletter.Campaigns.FirstOrDefault().CampaignType.Template.TemplateSections)
                    {
                        NewsletterSection ns = new NewsletterSection();
                        ns.OrderId = ts.OrderId;
                        ns.SectionId = ts.SectionId;

                        switch (ts.Section.SectionTypeId)
                        {
                            case (int)SectionTypeEnum.Header:
                                NewsletterSectionTranslation header = CreateNewsletterHeader(ts, language.Id);
                                ns.NewsletterSectionTranslations.Add(header);
                                sb.Append(header.Value);
                                break;
                            case (int)SectionTypeEnum.Greeting:
                                NewsletterSectionTranslation greeting = CreateNewsletterGreetings(ts, language.Id);
                                ns.NewsletterSectionTranslations.Add(greeting);
                                sb.Append(greeting.Value);
                                break;
                            case (int)SectionTypeEnum.Text:
                                switch (ts.Template.CampaignTypes.FirstOrDefault().Id)
                                {
                                    case (int)CampaignTypeEnum.FirstTransactionNotice:
                                    case (int)CampaignTypeEnum.SecondTransactionNotice:
                                    case (int)CampaignTypeEnum.ThirdTransactionNotice:
                                        NewsletterSectionTranslation text = CreateNewsletterIntroTextWithPoints(ts, language.Id);
                                        ns.NewsletterSectionTranslations.Add(text);
                                        sb.Append(text.Value);
                                        break;
                                }
                                break;
                            case (int)SectionTypeEnum.Image:
                                switch (ts.Template.CampaignTypes.FirstOrDefault().Id)
                                {
                                    case (int)CampaignTypeEnum.FirstTransactionNotice:
                                    case (int)CampaignTypeEnum.SecondTransactionNotice:
                                    case (int)CampaignTypeEnum.ThirdTransactionNotice:
                                        NewsletterSectionTranslation image = CreateNewsletterHero(ts, language.Id);
                                        ns.NewsletterSectionTranslations.Add(image);
                                        sb.Append(image.Value);
                                        break;
                                }
                                break;
                            case (int)SectionTypeEnum.MerchantList:
                                NewsletterSectionTranslation merchantlist = CreateNewsletterMerchantList(ts, language.Id);
                                ns.NewsletterSectionTranslations.Add(merchantlist);
                                sb.Append(merchantlist.Value);
                                break;
                            case (int)SectionTypeEnum.Footer:
                                NewsletterSectionTranslation footer = CreateNewsletterFooter(ts, language.Id);
                                ns.NewsletterSectionTranslations.Add(footer);
                                sb.Append(footer.Value);
                                break;
                            default:
                                break;
                        }
                        newsletter.NewsletterSections.Add(ns);
                    }
                    content.Value = sb.ToString();
                    newsletter.NewsletterContents.Add(content);
                }
            }

            return newsletter;
        }

        public Newsletter BuildNewsletterContent(Newsletter newsletter, Enterprise enterprise) 
        {
            foreach (Language language in enterprise.Languages) 
            {
                NewsletterContent nc = new NewsletterContent();
                StringBuilder sb = new StringBuilder();

                foreach (NewsletterSection ns in newsletter.NewsletterSections.Where(a => a.NewsletterSectionTranslations.Any(b => b.LanguageId == language.Id)).OrderBy(c => c.OrderId))
                {
                    switch (ns.Section.SectionTypeId)
                    {
                        case (int)SectionTypeEnum.Header:
                        case (int)SectionTypeEnum.Greeting:
                        case (int)SectionTypeEnum.Link:
                        case (int)SectionTypeEnum.Footer:
                            sb.Append(ns.NewsletterSectionTranslations.FirstOrDefault().Value);
                            break;
                        case (int)SectionTypeEnum.Text:
                            switch (newsletter.Campaigns.FirstOrDefault().CampaignTypeId)
                            {
                                case (int)CampaignTypeEnum.FirstTransactionNotice:
                                case (int)CampaignTypeEnum.SecondTransactionNotice:
                                case (int)CampaignTypeEnum.ThirdTransactionNotice:
                                    sb.Append(ns.NewsletterSectionTranslations.FirstOrDefault().Value);
                                    sb.Append(GetNewsletterSpacer());
                                    break;
                            }
                            break;
                        case (int)SectionTypeEnum.Image:
                            sb.Append(ns.NewsletterSectionTranslations.FirstOrDefault().Value);
                            sb.Append(GetNewsletterSpacer());
                            break;
                        case (int)SectionTypeEnum.MerchantList:
                            sb.Append(GetMerchantListHeader(language));
                            sb.Append(ns.NewsletterSectionTranslations.FirstOrDefault().Value);
                            sb.Append(GetNewsletterSpacer());
                            sb.Append(GetMorePromotionLink(language));
                            sb.Append(GetNewsletterSpacer());
                            break;
                        default:
                            break;
                    }
                }
            }

            

            return newsletter;
        }

        private String GetNewsletterSpacer()
        {
            return "<table class=&quot;spacer&quot;><tbody><tr><td height=&quot;16px&quot; style=&quot;font-size:16px;line-height:16px;&quot;>&#xA0;</td></tr></tbody></table>";
        }

        private String GetMerchantListHeader(Language language) 
        {
            String title = IMS.Common.Core.IMSMessages.ResourceManager.GetString("NewsletterExploreMerchantTitle_" + language.ISO639_1);

            return "<table class=&quot;row&quot;><tbody><tr><th class=&quot;small-12 large-12 columns first last&quot;><table><tr><th><h3 class=&quot;&quot;>" + title + "</h3></th><th class=&quot;expander&quot;></th></tr></table></th></tr></tbody></table>";
        }

        private String GetMorePromotionLink(Language language) 
        {
            String buttonText = IMS.Common.Core.IMSMessages.ResourceManager.GetString("NewsletterGetMorePromotionText_" + language.ISO639_1);

            return "<table class=&quot;row&quot;><tbody><tr><th class=&quot;small-12 large-12 columns first last&quot;><table><tr><th><table class=&quot;button secondary radius float-right&quot;><tr><td><table><tr><td><a href=&quot;https://trendigo.com/" + language.ISO639_1 + "/promotions&quot; target=&quot;_blank&quot;>" + buttonText + "</a></td></tr></table></td></tr></table></th><th class=&quot;expander&quot;></th></tr></table></th></tr></tbody></table>";
        }

        private NewsletterSectionTranslation CreateNewsletterHeader(TemplateSection ts, Int32 languageId) 
        {
            return CreateNewsletterSectionTranslation(ts, languageId);
        }

        private NewsletterSectionTranslation CreateNewsletterGreetings(TemplateSection ts, Int32 languageId)
        {
            return CreateNewsletterSectionTranslation(ts, languageId);
        }

        private NewsletterSectionTranslation CreateNewsletterIntroTextWithPoints(TemplateSection ts, Int32 languageId)
        {
            return CreateNewsletterSectionTranslation(ts, languageId);
        }

        private NewsletterSectionTranslation CreateNewsletterHero(TemplateSection ts, Int32 languageId)
        {
            return CreateNewsletterSectionTranslation(ts, languageId);
        }

        private NewsletterSectionTranslation CreateNewsletterMerchantList(TemplateSection ts, Int32 languageId)
        {
            return CreateNewsletterSectionTranslation(ts, languageId);
        }

        private NewsletterSectionTranslation CreateNewsletterFooter(TemplateSection ts, Int32 languageId)
        {
            return CreateNewsletterSectionTranslation(ts, languageId);
        }

        private NewsletterSectionTranslation CreateNewsletterSectionTranslation(TemplateSection ts, Int32 languageId) 
        {
            NewsletterSectionTranslation nst = new NewsletterSectionTranslation();
            nst.CreationDate = DateTime.Now;
            nst.ModificationDate = DateTime.Now;
            nst.Value = ts.Section.SectionTranslations.FirstOrDefault(a => a.LanguageId == languageId).Value;
            nst.LanguageId = languageId;

            return nst;
        }
    }
}
