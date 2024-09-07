using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CommentViewModel
    {
        public int ID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentDate { get; set; }
        public int ProductID { get; set; }
        public string UserID { get; set; }
        public int ParentID { get; set; }
        public int Rate { get; set; }
        public virtual ProductViewModel Product { set; get; }
        public virtual ApplicationUserViewModel User { set; get; }
    }
}