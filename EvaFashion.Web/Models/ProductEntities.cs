using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("danhMucId")]
        public int DanhMucId { get; set; }

        [Required]
        [Column("tenSanPham")]
        [StringLength(200)]
        public string TenSanPham { get; set; }

        [Required]
        [Column("slug")]
        [StringLength(250)]
        public string Slug { get; set; }

        [Column("thuongHieu")]
        [StringLength(100)]
        public string? ThuongHieu { get; set; }

        [Column("moTaNgan")]
        public string? MoTaNgan { get; set; }

        [Column("moTaChiTiet")]
        public string? MoTaChiTiet { get; set; }

        [Column("giaGoc", TypeName = "decimal(12,2)")]
        public decimal GiaGoc { get; set; }

        [Column("phanTramGiam")]
        public int? PhanTramGiam { get; set; } = 0;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("giaSauGiam", TypeName = "decimal(12,2)")]
        public decimal? GiaSauGiam { get; set; }

        [Column("hinhAnhChinh")]
        [StringLength(255)]
        public string? HinhAnhChinh { get; set; }

        [Column("metaTitle")]
        [StringLength(200)]
        public string? MetaTitle { get; set; }

        [Column("metaDescription")]
        [StringLength(300)]
        public string? MetaDescription { get; set; }

        [Column("metaKeyword")]
        [StringLength(300)]
        public string? MetaKeyword { get; set; }

        [Column("noiBat")]
        public bool? NoiBat { get; set; } = false;

        [Column("isActive")]
        public bool? IsActive { get; set; } = true;

        [Column("createdAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Column("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Column("createdBy")]
        public int? CreatedBy { get; set; }

        [Column("updatedBy")]
        public int? UpdatedBy { get; set; }

        [ForeignKey("DanhMucId")]
        public virtual DanhMuc DanhMuc { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual NguoiDung? NguoiTao { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual NguoiDung? NguoiCapNhat { get; set; }

        public virtual ICollection<BienTheSanPham> BienTheSanPhams { get; set; } = new List<BienTheSanPham>();
    }

    [Table("BienTheSanPham")]
    public class BienTheSanPham
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sanPhamId")]
        public int SanPhamId { get; set; }

        [Column("mauId")]
        public int? MauId { get; set; }

        [Column("kichCoId")]
        public int? KichCoId { get; set; }

        [Column("soLuongTon")]
        public int? SoLuongTon { get; set; } = 0;

        [Column("hinhAnh")]
        [StringLength(255)]
        public string? HinhAnh { get; set; }

        [ForeignKey("SanPhamId")]
        public virtual SanPham SanPham { get; set; }

        [ForeignKey("MauId")]
        public virtual MauSac? MauSac { get; set; }

        [ForeignKey("KichCoId")]
        public virtual KichCo? KichCo { get; set; }
    }
}
