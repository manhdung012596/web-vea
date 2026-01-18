using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("KichCo")]
    public class KichCo
    {
        [Key]
        [Column("maKichCo")]
        public int MaKichCo { get; set; }

        [Required]
        [Column("tenKichCo")]
        [StringLength(10)]
        public string TenKichCo { get; set; }
    }
}
