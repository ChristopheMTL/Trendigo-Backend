using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    public class CardPointHistoryMap: EntityTypeConfiguration<CardPointHistory>
    {
        public CardPointHistoryMap()
        {
            this.HasRequired(x => x.IMSUser).WithMany(x => x.CardPointHistories).HasForeignKey(x => x.CreatedBy);
        }
    }
}
