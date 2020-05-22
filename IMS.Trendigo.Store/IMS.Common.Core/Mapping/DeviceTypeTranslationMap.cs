using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class DeviceTypeTranslationMap : EntityTypeConfiguration<DeviceTypeTranslation>
    {
        public DeviceTypeTranslationMap()
        {
            this.Property(a => a.Id).HasColumnName("Id");
            this.Property(a => a.DeviceTypeId).HasColumnName("DeviceTypeId");
            //this.HasRequired(x => x.DeviceType).WithMany(x => x.DeviceTypeTranslations).HasForeignKey(x => x.DeviceTypeId);
        }
    }
}
