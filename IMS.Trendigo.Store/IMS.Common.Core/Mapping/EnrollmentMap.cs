using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    public class EnrollmentMap : EntityTypeConfiguration<Enrollment>
    {
        public EnrollmentMap()
        {
            this.HasRequired(x => x.IMSMembership).WithMany(x => x.Enrollments).HasForeignKey(x => x.NewMembershipId);
            this.HasOptional(x => x.IMSMembership1).WithMany(x => x.Enrollments1).HasForeignKey(x => x.OldMembershipId);
            this.HasRequired(x => x.EnrollmentStatu).WithMany(x => x.Enrollments).HasForeignKey(x => x.EnrollmentStatusId);
            this.HasRequired(x => x.EnrollmentProcess).WithMany(x => x.Enrollments).HasForeignKey(x => x.EnrollmentProcessId);
        }
    }
}
