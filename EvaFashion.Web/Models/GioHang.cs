using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("GioHang")]
    public class GioHang
    {
        [Key]
        [Column("maGioHang")]
        public int MaGioHang { get; set; }

        [Column("sessionId")]
        [StringLength(255)]
        public string? SessionId { get; set; }

        [Column("maNguoiDung")]
        public int? MaNguoiDung { get; set; }

        [Column("tongTien", TypeName = "decimal(12,2)")]
        public decimal? TongTien { get; set; } = 0;

        [Column("trangThai")]
        [StringLength(20)]
        public string? TrangThai { get; set; } = "DangSuDung";

        [Column("ghiChu")]
        public string? GhiChu { get; set; }

        [Column("ngayTao")]
        public DateTime? NgayTao { get; set; } = DateTime.Now;

        [Column("ngayCapNhat")]
        public DateTime? NgayCapNhat { get; set; } = DateTime.Now;

        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung? NguoiDung { get; set; }

        public virtual ICollection<GioHangChiTiet> GioHangChiTiets { get; set; } = new List<GioHangChiTiet>();
    }
}
