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
    class ContractMap : EntityTypeConfiguration<Contract>
    {
        public ContractMap() 
        {
            //HasMany(a => a.Merchants).WithMany(b => b.Contracts).Map(m =>
            //{
            //    m.ToTable("MerchantContracts");
            //    m.MapLeftKey("ContractId");
            //    m.MapRightKey("MerchantId");
            //});
        }
    }
}
