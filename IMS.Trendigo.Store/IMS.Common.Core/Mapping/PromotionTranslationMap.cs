using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Mapping
{
    class PromotionTranslationMap : EntityTypeConfiguration<promotion_translations>
    {

        public PromotionTranslationMap()
        {
            HasRequired(a => a.Promotion)
                .WithMany(b => b.promotion_translations)
                .HasForeignKey(a => a.promotion_id);

        }
    }
}
