using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class EnterpriseStateTaxNumberMap : EntityTypeConfiguration<EnterpriseStateTaxNumber>
    {
        public EnterpriseStateTaxNumberMap() 
        {
            this.HasRequired(a => a.StateTax).WithMany(b => b.EnterpriseStateTaxNumbers).HasForeignKey(c => c.StateTaxeId);
        }
    }
}
