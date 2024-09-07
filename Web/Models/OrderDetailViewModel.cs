using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { set; get; }

        public int ProductID { set; get; }

        public int Quantity { set; get; }

        public DateTime ExpiredDate { set; get; }

        public string ProductName { set; get; }

        public decimal Price { set; get; }
    }
}