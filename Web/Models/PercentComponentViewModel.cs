using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    [Serializable]
    public class PercentComponentViewModel
    {
        public int ID { set; get; }

        public string Purpose { set; get; }

        public int Component { set; get; }

        public int Price { set; get; }
    }
}