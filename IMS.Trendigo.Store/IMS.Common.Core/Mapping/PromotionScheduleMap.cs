using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Mapping
{
    class PromotionScheduleMap : EntityTypeConfiguration<Promotion_Schedules>
    {

        public PromotionScheduleMap()
        {
            //HasMany(a => a.Promotion_Schedule_Pricings)
            //    .WithRequired()
            //    .Map(a => a.MapKey("PromotionScheduleId"));
            this.HasRequired(x => x.IMSUser).WithMany(x => x.Promotion_Schedules).HasForeignKey(x => x.CreatedBy);
        }
    }
}
