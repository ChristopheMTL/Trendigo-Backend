﻿using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Mapping
{
    class TaggingMap : EntityTypeConfiguration<tagging>
    {
        public TaggingMap() 
        {
            HasRequired(c => c.tag) 
            .WithMany(d => d.taggings) 
            .HasForeignKey(c => c.tag_id);
        }
    }
}