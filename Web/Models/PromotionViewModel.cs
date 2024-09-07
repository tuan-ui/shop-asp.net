using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class PromotionViewModel
    {
        public int ID { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public int Quantity { set; get; }

        public int? ProductID { set; get; }

        public DateTime? DateStart { set; get; }

        public DateTime? DateEnd { set; get; }

        public int DiscountPercent { set; get; }

        public bool Status { set; get; }

        public virtual ProductViewModel Product { set; get; }
    }
}