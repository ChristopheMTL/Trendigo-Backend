using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class SocialMediaUserMap : EntityTypeConfiguration<SocialMediaUser>
    {
        public SocialMediaUserMap() 
        {
            this.HasRequired(x => x.AspNetUser).WithMany(x => x.SocialMediaUsers).HasForeignKey(x => x.UserId);
        }
    }
}
