﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Mapping
{
    public class GlobalResponseCodeMap : EntityTypeConfiguration<GlobalResponseCode>
    {
        public GlobalResponseCodeMap()
        {
            HasKey(a => a.ResponseCode);
        }
    }
}
