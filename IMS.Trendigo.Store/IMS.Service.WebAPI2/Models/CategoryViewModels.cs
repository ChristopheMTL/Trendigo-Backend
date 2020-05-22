using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Service.WebAPI2.Models
{
    public class Category
    {
        public int categoryId { get; set; }
        public string name { get; set; }
        public List<categoryLocale> locale { get; set; }
    }

    public class categoryLocale
    {
        public string name { get; set; }
        public string locale { get; set; }
    }
}