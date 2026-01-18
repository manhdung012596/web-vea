using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("GioHangChiTiet")]
    public class GioHangChiTiet
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("gioHangId")]
        public int GioHangId { get; set; }

        [Column("bienTheId")]
        public int BienTheId { get; set; }

        [Column("soLuong")]
        public int SoLuong { get; set; }

        [Column("donGia", TypeName = "decimal(12,2)")]
        public decimal DonGia { get; set; }

        [Column("thanhTien", TypeName = "decimal(12,2)")]
        public decimal ThanhTien { get; set; }

        [Column("ngayCapNhat")]
        public DateTime? NgayCapNhat { get; set; } = DateTime.Now;

        [ForeignKey("GioHangId")]
        public virtual GioHang GioHang { get; set; }

        [ForeignKey("BienTheId")]
        public virtual BienTheSanPham BienTheSanPham { get; set; }
    }
}
