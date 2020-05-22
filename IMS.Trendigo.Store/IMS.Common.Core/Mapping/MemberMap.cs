using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using System.Data.Entity.Infrastructure.Annotations;

namespace IMS.Common.Core.Mapping
{
    class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            this.HasRequired(x => x.AspNetUser).WithMany(x => x.Members).HasForeignKey(x => x.UserId);

            HasMany(t => t.Referrals)
                .WithRequired(t => t.Member)
                .HasForeignKey(t => t.ReferredBy);

            HasMany(t => t.Referrals1)
                .WithOptional(t => t.Member1)
                .HasForeignKey(t => t.ReferredTo);
        }

    }
}
