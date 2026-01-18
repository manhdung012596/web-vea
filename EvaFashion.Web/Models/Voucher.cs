using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("Voucher")]
    public class Voucher
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("maCode")]
        [StringLength(50)]
        public string? MaCode { get; set; }

        [Column("giamGia", TypeName = "decimal(12,2)")]
        public decimal? GiamGia { get; set; }

        [Column("loai")]
        [StringLength(20)]
        public string? Loai { get; set; } // Tien, PhanTram

        [Column("ngayBatDau")]
        public DateTime? NgayBatDau { get; set; }

        [Column("ngayKetThuc")]
        public DateTime? NgayKetThuc { get; set; }

        [Column("soLuong")]
        public int? SoLuong { get; set; }

        [Column("trangThai")]
        public bool? TrangThai { get; set; } = true;
    }
}
