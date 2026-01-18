using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        [Key]
        [Column("maChiTiet")]
        public int MaChiTiet { get; set; }

        [Column("maDonHang")]
        public int? MaDonHang { get; set; }

        [Column("maBienThe")]
        public int? MaBienThe { get; set; }

        [Column("soLuong")]
        public int? SoLuong { get; set; }

        [Column("donGia", TypeName = "decimal(12,2)")]
        public decimal? DonGia { get; set; }

        [Column("thanhTien", TypeName = "decimal(12,2)")]
        public decimal? ThanhTien { get; set; }

        [Column("ghiChu")]
        public string? GhiChu { get; set; }

        [ForeignKey("MaDonHang")]
        public virtual DonHang? DonHang { get; set; }

        [ForeignKey("MaBienThe")]
        public virtual BienTheSanPham? BienTheSanPham { get; set; }
    }
}
