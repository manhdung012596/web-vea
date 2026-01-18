using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        [Column("maNguoiDung")]
        public int MaNguoiDung { get; set; }

        [Required]
        [Column("tenDangNhap")]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [Required]
        [Column("matKhau")]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [Column("hoTen")]
        [StringLength(100)]
        public string? HoTen { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("sdt")]
        [StringLength(20)]
        public string? Sdt { get; set; }

        [Column("vaiTro")]
        [StringLength(20)]
        public string VaiTro { get; set; } = "khachhang";

        [Column("ngayTao")]
        public DateTime? NgayTao { get; set; } = DateTime.Now;
    }

    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tenDanhMuc")]
        [StringLength(150)]
        public string TenDanhMuc { get; set; }

        [Required]
        [Column("slug")]
        [StringLength(200)]
        public string Slug { get; set; }

        [Column("createdBy")]
        public int? CreatedBy { get; set; }

        [Column("updatedBy")]
        public int? UpdatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual NguoiDung? NguoiTao { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual NguoiDung? NguoiCapNhat { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }

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
