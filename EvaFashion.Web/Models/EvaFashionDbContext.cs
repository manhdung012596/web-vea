using Microsoft.EntityFrameworkCore;

namespace EvaFashion.Web.Models
{
    public class EvaFashionDbContext : DbContext
    {
        public EvaFashionDbContext(DbContextOptions<EvaFashionDbContext> options) : base(options)
        {
        }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<KichCo> KichCos { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<BienTheSanPham> BienTheSanPhams { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<GioHangChiTiet> GioHangChiTiets { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<BaiViet> BaiViets { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<TrangNoiDung> TrangNoiDungs { get; set; }
        public DbSet<LienHe> LienHes { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Additional configuration if needed (e.g. unique constraints already in class attributes)
            
            // Computed column configuration for SanPham.GiaSauGiam
            modelBuilder.Entity<SanPham>()
                .Property(p => p.GiaSauGiam)
                .HasComputedColumnSql("CASE WHEN phanTramGiam > 0 THEN giaGoc - (giaGoc * phanTramGiam / 100) ELSE giaGoc END");
        }
    }
}
