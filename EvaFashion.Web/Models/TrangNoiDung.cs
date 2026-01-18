using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("TrangNoiDung")]
    public class TrangNoiDung
    {
        [Key]
        [Column("maTrang")]
        public int MaTrang { get; set; }

        [Column("tieuDe")]
        [StringLength(200)]
        public string? TieuDe { get; set; }

        [Column("slug")]
        [StringLength(200)]
        public string? Slug { get; set; }

        [Column("loaiTrang")]
        [StringLength(50)]
        public string? LoaiTrang { get; set; }

        [Column("moTaNgan")]
        public string? MoTaNgan { get; set; }

        [Column("noiDung")]
        public string? NoiDung { get; set; }

        [Column("thuTu")]
        public int? ThuTu { get; set; } = 0;

        [Column("hienThi")]
        public bool? HienThi { get; set; } = true;

        [Column("noiBat")]
        public bool? NoiBat { get; set; } = false;

        [Column("ngayCapNhat")]
        public DateTime? NgayCapNhat { get; set; } = DateTime.Now;

        [Column("maNguoiTao")]
        public int? MaNguoiTao { get; set; }

        [Column("maNguoiCapNhat")]
        public int? MaNguoiCapNhat { get; set; }

        [ForeignKey("MaNguoiTao")]
        public virtual NguoiDung? NguoiTao { get; set; }

        [ForeignKey("MaNguoiCapNhat")]
        public virtual NguoiDung? NguoiCapNhat { get; set; }
    }
}
