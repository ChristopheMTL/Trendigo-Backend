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
    class IMSHeaderMap : EntityTypeConfiguration<IMS_Header>
    {
        public IMSHeaderMap()
        {
            //HasMany(a => a.IMS_Detail)
            //    .WithRequired()
            //    .HasForeignKey(a => a.HeaderId);

            HasRequired(x => x.PaymentStatu)
            .WithMany(x => x.IMS_Header)  // Or, just .WithMany()
            .HasForeignKey(x => x.PaymentStatusId);
        }
    }
}
