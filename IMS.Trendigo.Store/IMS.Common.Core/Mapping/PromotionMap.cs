using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Mapping
{
    class PromotionMap : EntityTypeConfiguration<Promotion>
    {

        public PromotionMap()
        {
            HasMany(p => p.Locations)
                .WithMany(p => p.Promotions)
                .Map(m =>
                {
                    m.MapLeftKey("PromotionId");
                    m.MapRightKey("LocationId");
                    m.ToTable("PromotionLocations");
                });

            HasMany(p => p.Programs)
                .WithMany(p => p.Promotions)
                .Map(m =>
                {
                    m.MapLeftKey("PromotionId");
                    m.MapRightKey("ProgramId");
                    m.ToTable("PromotionPrograms");
                });
                
        }
    }
}
