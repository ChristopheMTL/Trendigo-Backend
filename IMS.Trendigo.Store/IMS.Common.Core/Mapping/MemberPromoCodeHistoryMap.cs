using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    public class MemberPromoCodeHistoryMap : EntityTypeConfiguration<MemberPromoCodeHistory>
    {
        public MemberPromoCodeHistoryMap() 
        {
            this.HasRequired(x => x.IMSCard).WithMany(x => x.MemberPromoCodeHistories).HasForeignKey(x => x.CardId);
        }
    }
}
