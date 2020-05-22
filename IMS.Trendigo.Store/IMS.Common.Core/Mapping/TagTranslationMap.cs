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
    class TagTranslationMap : EntityTypeConfiguration<tag_translations>
    {
        public TagTranslationMap()
        {
            HasRequired(c => c.tag)
            .WithMany(d => d.tag_translations)
            .HasForeignKey(c => c.tag_id);


        }
    }
}