using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class MonthlyBonusPointMap : EntityTypeConfiguration<MonthlyBonusPoint>
    {
        public MonthlyBonusPointMap()
        {
            this.Property(a => a.Id).HasColumnName("Id");
            this.HasRequired(x => x.CommandStatu).WithMany(x => x.MonthlyBonusPoints).HasForeignKey(x => x.CommandStatusId);
        }
    }
}
