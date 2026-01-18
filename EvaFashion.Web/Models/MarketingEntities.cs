using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("BaiViet")]
    public class BaiViet
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("tieuDe")]
        [StringLength(200)]
        public string? TieuDe { get; set; }

        [Column("slug")]
        [StringLength(250)]
        public string? Slug { get; set; }

        [Column("hinhAnh")]
        [StringLength(255)]
        public string? HinhAnh { get; set; }

        [Column("moTa")]
        public string? MoTa { get; set; }

        [Column("noiDung")]
        public string? NoiDung { get; set; }

        [Column("metaTitle")]
        [StringLength(200)]
        public string? MetaTitle { get; set; }

        [Column("metaDescription")]
        [StringLength(300)]
        public string? MetaDescription { get; set; }

        [Column("metaKeyword")]
        [StringLength(300)]
        public string? MetaKeyword { get; set; }

        [Column("isPublished")]
        public bool? IsPublished { get; set; } = true;

        [Column("noiBat")]
        public bool? NoiBat { get; set; } = false;

        [Column("createdAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Column("createdBy")]
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual NguoiDung? NguoiTao { get; set; }
    }

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
