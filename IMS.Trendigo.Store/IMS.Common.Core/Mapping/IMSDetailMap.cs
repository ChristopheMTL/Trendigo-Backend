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
    class IMSDetailMap : EntityTypeConfiguration<IMS_Detail>
    {
        public IMSDetailMap() 
        {
            HasRequired(a => a.IMS_Header)
                .WithMany(b => b.IMS_Detail)
                .HasForeignKey(a => a.HeaderId);

            HasRequired(a => a.IMS_LineItem)
                .WithMany(b => b.IMS_Detail)
                .HasForeignKey(a => a.LineItemId);
        }
    }
}
