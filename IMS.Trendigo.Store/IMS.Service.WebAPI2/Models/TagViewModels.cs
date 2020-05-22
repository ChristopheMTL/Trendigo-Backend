using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Service.WebAPI2.Models
{
    public class TagTranslationViewModel
    {
        public int id { get; set; }
        public int tag_id { get; set; }
        public string locale { get; set; }
        public string name { get; set; }
    }

    public class TagViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> CityId { get; set; }
        public virtual ICollection<TagTranslationViewModel> tag_translations { get; set; }
    }

    public class TagCategoryViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<TagCategoryTranslationViewModel> Translations { get; set; }
    }

    public class TagCategoryTranslationViewModel
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
    }
}