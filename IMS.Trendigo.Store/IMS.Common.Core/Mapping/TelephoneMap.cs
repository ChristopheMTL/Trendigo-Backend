using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Mapping
{
    class TelephoneMap : EntityTypeConfiguration<Telephone>
    {
        public TelephoneMap() 
        {
            
            Property(d => d.Number).HasColumnName("Number").IsRequired();
            Property(d => d.TelephoneTypeId).HasColumnName("TelephoneTypeId").IsRequired();
            Property(d => d.IsActive).HasColumnName("IsActive").IsRequired();
            //Property(d => d.CreatonDate).HasColumnName("CreationDate").HasColumnType("datetime2").IsRequired();
        }
    }
}
