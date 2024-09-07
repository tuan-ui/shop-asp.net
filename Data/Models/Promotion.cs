using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Table("Promotions")]
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Code { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; } 

        [Required]
        public int Quantity { set; get; }

        public int? ProductID { set; get; }

        public DateTime? DateStart { set; get; }

        public DateTime? DateEnd { set; get; }

        [Required]
        public int DiscountPercent { set; get; }

        public bool Status { set; get; }

        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }

    }
}
