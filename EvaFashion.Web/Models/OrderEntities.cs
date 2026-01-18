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

    [Table("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        [Key]
        [Column("maChiTiet")]
        public int MaChiTiet { get; set; }

        [Column("maDonHang")]
        public int? MaDonHang { get; set; }

        [Column("maBienThe")]
        public int? MaBienThe { get; set; }

        [Column("soLuong")]
        public int? SoLuong { get; set; }

        [Column("donGia", TypeName = "decimal(12,2)")]
        public decimal? DonGia { get; set; }

        [Column("thanhTien", TypeName = "decimal(12,2)")]
        public decimal? ThanhTien { get; set; }

        [Column("ghiChu")]
        public string? GhiChu { get; set; }

        [ForeignKey("MaDonHang")]
        public virtual DonHang? DonHang { get; set; }

        [ForeignKey("MaBienThe")]
        public virtual BienTheSanPham? BienTheSanPham { get; set; }
    }
}
