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
    class LocationMap : EntityTypeConfiguration<Location>
    {
        public LocationMap() 
        {
            HasMany(l => l.IMSUsers)
                .WithMany(a => a.Locations)
                .Map(m =>
                {
                    m.MapLeftKey("LocationId");
                    m.MapRightKey("IMSUserId");
                    m.ToTable("IMSUserLocations");
                });

        }
    }
}
