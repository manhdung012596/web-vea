using Microsoft.EntityFrameworkCore;

namespace EvaFashion.Web.Models
{
    public class ThoitrangnuContext : DbContext
    {
        public ThoitrangnuContext(DbContextOptions<ThoitrangnuContext> options) : base(options)
        {
        }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<KichCo> KichCos { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<BienTheSanPham> BienTheSanPhams { get; set; }
        public DbSet<AnhSanPham> AnhSanPhams { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<GioHangChiTiet> GioHangChiTiets { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<BaiViet> BaiViets { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<TrangNoiDung> TrangNoiDungs { get; set; }
        public DbSet<LienHe> LienHes { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<LichSuDonHang> LichSuDonHangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Computed column configuration for SanPham.GiaSauGiam
            modelBuilder.Entity<SanPham>()
                .Property(p => p.GiaSauGiam)
                .HasComputedColumnSql("CASE WHEN phanTramGiam > 0 THEN giaGoc - (giaGoc * phanTramGiam / 100) ELSE giaGoc END");
                
            // Configure LichSuDonHang if needed (or rely on conventions)
        }
    }
}
