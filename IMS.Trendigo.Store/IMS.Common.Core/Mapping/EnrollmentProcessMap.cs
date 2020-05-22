using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class EnrollmentProcessMap : EntityTypeConfiguration<EnrollmentProcess>
    {
        public EnrollmentProcessMap()
        {
            ToTable("EnrollmentProcess");
        }
    }
}
