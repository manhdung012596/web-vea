using System;
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
}
