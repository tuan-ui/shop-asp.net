using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentDate { get; set; }
        public int ProductID { get; set; }
        public string UserID { get; set; }
        public int ParentID { get; set; }
        public int Rate { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { set; get; }
    }
}
