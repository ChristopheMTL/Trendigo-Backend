using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Mapping
{
    class DeviceRequestMap : EntityTypeConfiguration<DeviceRequest>
    {
        public DeviceRequestMap()
        {
            this.Property(a => a.Id).HasColumnName("Id");
            this.HasRequired(x => x.DeviceRequestStatu).WithMany(x => x.DeviceRequests).HasForeignKey(x => x.DeviceRequestStatutId);
            this.HasOptional(x => x.IMSCard).WithMany(x => x.DeviceRequests).HasForeignKey(x => x.IMSCardId);
            this.HasOptional(x => x.IMSCard1).WithMany(x => x.DeviceRequests1).HasForeignKey(x => x.IMSCardId);
        }
    }
}
