using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tenDanhMuc")]
        [StringLength(150)]
        public string TenDanhMuc { get; set; }

        [Required]
        [Column("slug")]
        [StringLength(200)]
        public string Slug { get; set; }

        [Column("createdBy")]
        public int? CreatedBy { get; set; }

        [Column("updatedBy")]
        public int? UpdatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual NguoiDung? NguoiTao { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual NguoiDung? NguoiCapNhat { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
