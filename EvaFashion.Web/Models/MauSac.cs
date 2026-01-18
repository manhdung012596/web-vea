using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("MauSac")]
    public class MauSac
    {
        [Key]
        [Column("maMau")]
        public int MaMau { get; set; }

        [Required]
        [Column("tenMau")]
        [StringLength(50)]
        public string TenMau { get; set; }

        [Column("maHex")]
        [StringLength(10)]
        public string? MaHex { get; set; }
    }
}
