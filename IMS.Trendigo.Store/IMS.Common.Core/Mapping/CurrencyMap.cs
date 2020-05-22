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
    class CurrencyMap: EntityTypeConfiguration<Currency>
    {
        public CurrencyMap()
        {
            Property(d => d.Id).HasColumnName("Id");
        }
    }
}
