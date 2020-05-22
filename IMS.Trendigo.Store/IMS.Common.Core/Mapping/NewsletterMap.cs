using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    public class NewsletterMap : EntityTypeConfiguration<Newsletter>
    {
        public NewsletterMap()
        {
            this.HasMany(x => x.Campaigns).WithMany(x => x.Newsletters).Map(m =>
            {
                m.ToTable("CampaignNewsletters");
                m.MapLeftKey("NewsletterId");
                m.MapRightKey("CampaignId");
            });
        }
    }
}
