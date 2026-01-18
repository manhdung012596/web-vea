using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("LichSuDonHang")]
    public class LichSuDonHang
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("maDonHang")]
        public int? MaDonHang { get; set; }

        [Column("trangThai")]
        [StringLength(20)]
        public string? TrangThai { get; set; }

        [Column("ghiChu")]
        [StringLength(255)]
        public string? GhiChu { get; set; }

        [Column("ngayCapNhat")]
        public DateTime? NgayCapNhat { get; set; } = DateTime.Now;

        [ForeignKey("MaDonHang")]
        public virtual DonHang? DonHang { get; set; }
    }
}
