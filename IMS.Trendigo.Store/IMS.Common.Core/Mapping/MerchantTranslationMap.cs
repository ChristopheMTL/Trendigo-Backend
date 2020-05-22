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
    class MerchantTranslationMap : EntityTypeConfiguration<merchant_translations>
    {
        public MerchantTranslationMap() 
        {
            this.Property(a => a.id).HasColumnName("id");
            this.Property(a => a.merchant_id).HasColumnName("merchant_id");
            this.HasRequired(x => x.Merchant).WithMany(x => x.merchant_translations).HasForeignKey(x => x.merchant_id);
        }
    }
}
