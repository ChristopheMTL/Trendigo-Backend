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
    class EnterpriseMap : EntityTypeConfiguration<Enterprise>
    {
        public EnterpriseMap() 
        {
            //ToTable("Enterprises");
            HasMany(t => t.Merchants)
            .WithMany(t => t.Enterprises)
            .Map(m =>
            {
                m.ToTable("EnterpriseMerchants");
                m.MapLeftKey("EnterpriseId");
                m.MapRightKey("MerchantId");
            });

            HasMany(t => t.Languages)
            .WithMany(t => t.Enterprises)
            .Map(m =>
            {
                m.ToTable("EnterpriseLanguages");
                m.MapLeftKey("EnterpriseId");
                m.MapRightKey("LanguageId");
            });

            HasMany(t => t.Telephones)
            .WithMany(t => t.Enterprises)
            .Map(m =>
            {
                m.ToTable("EnterpriseTelephones");
                m.MapLeftKey("EnterpriseId");
                m.MapRightKey("TelephoneId");
            });

            HasMany(t => t.Programs)
            .WithMany(t => t.Enterprises)
            .Map(m =>
            {
                m.ToTable("EnterprisePrograms");
                m.MapLeftKey("EnterpriseId");
                m.MapRightKey("ProgramId");
            });
        }
    }
}
