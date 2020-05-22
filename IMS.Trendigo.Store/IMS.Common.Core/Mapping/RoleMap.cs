using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class RoleMap : EntityTypeConfiguration<AspNetRole>
    {
        public RoleMap() 
        {
            //configure key and properties
            HasKey(c => c.Id);

            //configure table map
            ToTable("AspNetRole");
        }
    }
}
