using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class AspNetUserMap : EntityTypeConfiguration<AspNetUser>
    {
        public AspNetUserMap() 
        {
            HasMany(a => a.AspNetRoles).WithMany(b => b.AspNetUsers).Map(m =>
            {
                m.ToTable("AspNetUserRoles");
                m.MapLeftKey("UserId");
                m.MapRightKey("RoleId");
            });

            this.HasMany(a => a.Notifications).WithRequired(b => b.AspNetUser).HasForeignKey(c => c.UserId);
        }
    }
}