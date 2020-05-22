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
    class MembershipTransactionDetailMap : EntityTypeConfiguration<MembershipTransactionDetail>
    {
        public MembershipTransactionDetailMap()
        {
            
            HasRequired(a => a.MembershipTransactionHeader)
                .WithMany(b => b.MembershipTransactionDetails)
                .HasForeignKey(a => a.HeaderId);

        }
    }
}
