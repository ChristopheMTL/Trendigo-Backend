using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Mapping
{
    class DeviceRequestStatusTranslationMap : EntityTypeConfiguration<DeviceRequestStatus_translations>
    {
        public DeviceRequestStatusTranslationMap()
        {
            this.HasRequired(a => a.DeviceRequestStatu).WithMany(a => a.DeviceRequestStatus_translations).HasForeignKey(a => a.DeviceRequestStatusId);
        }
    }
}
