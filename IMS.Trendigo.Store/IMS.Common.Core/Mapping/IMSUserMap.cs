using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    public class IMSUserMap : EntityTypeConfiguration<IMSUser>
    {
        public IMSUserMap()
        {
            HasKey(a => a.Id);
            this.HasRequired(x => x.AspNetUser).WithMany(x => x.IMSUsers).HasForeignKey(x => x.UserId);
        }
    }
}
