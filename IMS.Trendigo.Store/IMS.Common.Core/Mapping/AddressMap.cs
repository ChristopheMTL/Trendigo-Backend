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
    class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap() 
        {
            Property(d => d.StreetAddress).IsRequired();
            Property(d => d.App).IsOptional();
            Property(d => d.City).IsRequired();
            Property(d => d.StateId).IsRequired();
            Property(d => d.CountryId).IsRequired();
            Property(d => d.Longitude).IsRequired().HasPrecision(9,6);
            Property(d => d.Latitude).IsRequired().HasPrecision(9,6);
            Property(d => d.IsActive).IsRequired();
            Property(d => d.CreationDate).IsRequired();

        }
    }
}
