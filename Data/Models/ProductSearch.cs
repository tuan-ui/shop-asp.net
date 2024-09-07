using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("ProductSearchs")]
    public class ProductSearch
    {
        [Key]
        [Column(Order = 1)]
        public int CategoryID { set; get; }

        [Key]
        [Column(TypeName = "varchar", Order = 2)]
        [MaxLength(50)]
        public string SearchID { set; get; }

        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }

        [ForeignKey("SearchID")]
        public virtual Search Search { set; get; }
    }
}
