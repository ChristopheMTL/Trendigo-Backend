using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Common.Core.Mapping
{
    class TrxNonFinancialTransactionMap : EntityTypeConfiguration<TrxNonFinancialTransaction>
    {
        public TrxNonFinancialTransactionMap()
        {
            HasKey(a => a.Id).Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
