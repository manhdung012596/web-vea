using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("DanhGia")]
    public class DanhGia
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sanPhamId")]
        public int? SanPhamId { get; set; }

        [Column("maNguoiDung")]
        public int? MaNguoiDung { get; set; }

        [Column("soSao")]
        public int? SoSao { get; set; }

        [Column("noiDung")]
        public string? NoiDung { get; set; }

        [Column("ngayDanhGia")]
        public DateTime? NgayDanhGia { get; set; } = DateTime.Now;

        [ForeignKey("SanPhamId")]
        public virtual SanPham? SanPham { get; set; }

        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung? NguoiDung { get; set; }
    }
}
