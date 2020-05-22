using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class CategoryDTO
    {
        public long tagId { get; set; }
        public long categoryId { get; set; }
        public string name { get; set; }
        public List<categoryLocale> tagLocale { get; set; }
    }

    public class categoryLocale
    {
        public string locale { get; set; }
        public string name { get; set; }
    }
}
