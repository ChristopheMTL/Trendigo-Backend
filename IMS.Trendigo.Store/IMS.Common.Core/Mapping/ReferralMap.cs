using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    public class ReferralMap : EntityTypeConfiguration<Referral>
    {
        public ReferralMap()
        {
            //this.HasRequired(x => x.Member).WithMany(x => x.Referrals).HasForeignKey(x => x.ReferredBy);
            //this.HasOptional(x => x.Member1).WithMany(x => x.Referrals1).HasForeignKey(x => x.ReferredTo);
        }
    }
}
