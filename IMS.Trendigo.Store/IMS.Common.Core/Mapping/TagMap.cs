using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class TagMap : EntityTypeConfiguration<tag>
    {
        public TagMap() 
        {
            this.HasOptional(a => a.City).WithMany(a => a.tags).HasForeignKey(a => a.CityId);
            
            //this.Property(a => a.CityId).HasColumnName("CityId");

            this.HasOptional(a => a.tag1).WithMany(a => a.tags1).HasForeignKey(a => a.ParentId);
        }
    }
}
