using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("LienHe")]
    public class LienHe
    {
        [Key]
        [Column("maLienHe")]
        public int MaLienHe { get; set; }

        [Column("hoTen")]
        [StringLength(100)]
        public string? HoTen { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("soDienThoai")]
        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [Column("tieuDe")]
        [StringLength(200)]
        public string? TieuDe { get; set; }

        [Column("loaiLienHe")]
        [StringLength(50)]
        public string? LoaiLienHe { get; set; }

        [Required]
        [Column("noiDung")]
        public string NoiDung { get; set; }

        [Column("daDoc")]
        public bool? DaDoc { get; set; } = false;

        [Column("trangThai")]
        [StringLength(20)]
        public string? TrangThai { get; set; } = "ChuaXuLy";

        [Column("ngayGui")]
        public DateTime? NgayGui { get; set; } = DateTime.Now;

        [Column("maNguoiDung")]
        public int? MaNguoiDung { get; set; }

        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung? NguoiDung { get; set; }
    }
}
