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
    class TrxFinancialTransactionMap : EntityTypeConfiguration<TrxFinancialTransaction>
    {
        public TrxFinancialTransactionMap() 
        {
            HasKey(a => a.Id).Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            

            //HasRequired(a => a.TrxNonFinancialTransaction)
            //    .WithMany(b => b.TrxFinancialTransactions)
            //    .HasForeignKey(a => a.legTransaction);

            HasRequired(a => a.GlobalResponseCode)
                .WithMany(b => b.TrxFinancialTransactions)
                .HasForeignKey(a => a.acquirerResponseCode);

            HasRequired(a => a.IMSTransferStatu)
                .WithMany(b => b.TrxFinancialTransactions)
                .HasForeignKey(a => a.IMSTransferStatusId);

        }
    }
}
