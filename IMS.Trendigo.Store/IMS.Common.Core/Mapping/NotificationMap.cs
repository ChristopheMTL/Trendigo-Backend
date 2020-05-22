using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    public class NotificationMap : EntityTypeConfiguration<Notification>
    {
        public NotificationMap()
        {
            this.HasRequired(x => x.NotificationStatu).WithMany(x => x.Notifications).HasForeignKey(x => x.NotificationStatusId);
            
        }
    }
}
