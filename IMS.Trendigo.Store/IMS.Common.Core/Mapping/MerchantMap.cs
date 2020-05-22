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
    class MerchantMap : EntityTypeConfiguration<Merchant>
    {
        public MerchantMap() 
        {
            Property(p => p.Name).IsRequired();
            Property(p => p.IsActive).IsRequired();
            Property(p => p.CreationDate).IsRequired();

            HasRequired(a => a.MerchantImage)
                .WithMany(b => b.Merchants)
                .HasForeignKey(a => a.LogoId);

            HasRequired(a => a.MerchantImage1)
                .WithMany(b => b.Merchants1)
                .HasForeignKey(a => a.MobileId);

            HasMany(a => a.Members).WithMany(b => b.Merchants).Map(m =>
                {
                    m.ToTable("Favorites");
                    m.MapLeftKey("MerchantId");
                    m.MapRightKey("MemberId");
                });

            HasMany(l => l.IMSUsers)
                .WithMany(a => a.Merchants)
                .Map(m =>
                {
                    m.MapLeftKey("MerchantId");
                    m.MapRightKey("IMSUserId");
                    m.ToTable("IMSUserMerchants");
                });

            //this.HasOptional(a => a.Program).WithMany(b => b.Merchants).HasForeignKey(c => c.ProgramId);
        }
    }
}
