using System;
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
}
