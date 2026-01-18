using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("DonHang")]
    public class DonHang
    {
        [Key]
        [Column("maDonHang")]
        public int MaDonHang { get; set; }

        [Column("maDonHangCode")]
        [StringLength(20)]
        public string? MaDonHangCode { get; set; }

        [Column("hoTen")]
        [StringLength(100)]
        public string? HoTen { get; set; }

        [Column("soDienThoai")]
        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [Column("diaChi")]
        [StringLength(255)]
        public string? DiaChi { get; set; }

        [Column("tongTien", TypeName = "decimal(12,2)")]
        public decimal? TongTien { get; set; }

        [Column("phiVanChuyen", TypeName = "decimal(12,2)")]
        public decimal? PhiVanChuyen { get; set; } = 0;

        [Column("phuongThucThanhToan")]
        [StringLength(20)]
        public string? PhuongThucThanhToan { get; set; }

        [Column("trangThaiThanhToan")]
        [StringLength(20)]
        public string? TrangThaiThanhToan { get; set; } = "ChuaThanhToan";

        [Column("trangThai")]
        [StringLength(20)]
        public string? TrangThai { get; set; } = "ChoXacNhan";

        [Column("ghiChu")]
        public string? GhiChu { get; set; }

        [Column("ngayDat")]
        public DateTime? NgayDat { get; set; } = DateTime.Now;

        [Column("ngayCapNhat")]
        public DateTime? NgayCapNhat { get; set; } = DateTime.Now;

        [Column("maNguoiDung")]
        public int? MaNguoiDung { get; set; }

        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung? NguoiDung { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
