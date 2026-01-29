using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaFashion.Web.Models
{
    [Table("AnhSanPham")]
    public class AnhSanPham
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sanPhamId")]
        public int SanPhamId { get; set; }

        [Column("url")]
        [StringLength(255)]
        public string Url { get; set; }

        [Column("createdAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("SanPhamId")]
        public virtual SanPham SanPham { get; set; }
    }
}
