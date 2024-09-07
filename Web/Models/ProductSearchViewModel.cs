using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ProductSearchViewModel
    {
        public int CategoryID { set; get; }

        public string SearchID { set; get; }

        public virtual ProductCategoryViewModel ProductCategory { set; get; }

        public virtual SearchViewModel Search { set; get; }
    }
}