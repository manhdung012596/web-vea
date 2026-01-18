using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("BienTheSanPham")]
    public class BienTheSanPham
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sanPhamId")]
        public int SanPhamId { get; set; }

        [Column("mauId")]
        public int? MauId { get; set; }

        [Column("kichCoId")]
        public int? KichCoId { get; set; }

        [Column("soLuongTon")]
        public int? SoLuongTon { get; set; } = 0;

        [Column("hinhAnh")]
        [StringLength(255)]
        public string? HinhAnh { get; set; }

        [ForeignKey("SanPhamId")]
        public virtual SanPham SanPham { get; set; }

        [ForeignKey("MauId")]
        public virtual MauSac? MauSac { get; set; }

        [ForeignKey("KichCoId")]
        public virtual KichCo? KichCo { get; set; }
    }
}
