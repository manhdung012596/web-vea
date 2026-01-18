using EvaFashion.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaFashion.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ThoitrangnuContext context)
        {
            context.Database.EnsureCreated();

            // 0. Users (Admin)
            if (!context.NguoiDungs.Any(u => u.TenDangNhap == "admin"))
            {
                context.NguoiDungs.Add(new NguoiDung
                {
                    TenDangNhap = "admin",
                    // SHA256 for '123456'
                    MatKhau = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
                    HoTen = "Quản Trị Viên",
                    Email = "admin@evafashion.vn",
                    VaiTro = "admin",
                    NgayTao = DateTime.Now
                });
                context.SaveChanges();
            }

            // 1. Categories
            var categories = new[]
            {
                new DanhMuc { TenDanhMuc = "Váy Đầm", Slug = "vay-dam", CreatedBy = 1, UpdatedBy = 1 },
                new DanhMuc { TenDanhMuc = "Áo Thời Trang", Slug = "ao-thoi-trang", CreatedBy = 1, UpdatedBy = 1 },
                new DanhMuc { TenDanhMuc = "Chân Váy", Slug = "chan-vay", CreatedBy = 1, UpdatedBy = 1 }
            };

            foreach (var c in categories)
            {
                if (!context.DanhMucs.Any(x => x.Slug == c.Slug))
                {
                    context.DanhMucs.Add(c);
                }
            }
            context.SaveChanges();

            // 2. Products
            var catVay = context.DanhMucs.First(c => c.Slug == "vay-dam").Id;
            var catAo = context.DanhMucs.First(c => c.Slug == "ao-thoi-trang").Id;
            var catChanVay = context.DanhMucs.First(c => c.Slug == "chan-vay").Id;

            var products = new[]
            {
                new SanPham 
                { 
                    TenSanPham = "Đầm Hoa Nhí Mùa Xuân", Slug = "dam-hoa-mua-xuan", DanhMucId = catVay,
                    ThuongHieu = "Eva Design", 
                    MoTaNgan = "Thiết kế nhẹ nhàng, bay bổng với họa tiết hoa nhí. Chất liệu voan cao cấp.",
                    GiaGoc = 550000, PhanTramGiam = 10, HinhAnhChinh = "/images/products/p1.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
                },
                new SanPham 
                { 
                    TenSanPham = "Áo Blazer Công Sở Thanh Lịch", Slug = "ao-blazer-cong-so", DanhMucId = catAo,
                    ThuongHieu = "Eva Office", 
                    MoTaNgan = "Phong cách chuyên nghiệp, sang trọng. Màu kem nhã nhặn dễ phối đồ.",
                    GiaGoc = 850000, PhanTramGiam = 0, HinhAnhChinh = "/images/products/p2.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
                },
                new SanPham 
                { 
                    TenSanPham = "Chân Váy Xếp Ly Hồng", Slug = "chan-vay-xep-ly", DanhMucId = catChanVay,
                    ThuongHieu = "Eva Young", 
                    MoTaNgan = "Dáng dài qua gối, xếp ly tinh tế. Màu hồng pastel ngọt ngào.",
                    GiaGoc = 420000, PhanTramGiam = 5, HinhAnhChinh = "/images/products/p3.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
                },
                new SanPham 
                { 
                    TenSanPham = "Đầm Dạ Hội Satin Đỏ Rượu", Slug = "dam-da-hoi-cao-cap", DanhMucId = catVay,
                    ThuongHieu = "Eva Luxury", 
                    MoTaNgan = "Thiết kế trễ vai quyến rũ, chất liệu Satin bóng mượt sang trọng.",
                    GiaGoc = 1200000, PhanTramGiam = 0, HinhAnhChinh = "/images/products/p4.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
                }
            };

            foreach (var p in products)
            {
                var existing = context.SanPhams.FirstOrDefault(x => x.Slug == p.Slug);
                if (existing == null)
                {
                    context.SanPhams.Add(p);
                }
                else
                {
                    existing.HinhAnhChinh = p.HinhAnhChinh;
                    existing.NoiBat = true;
                }
            }
            context.SaveChanges();
        }
    }
    
    // Add dummy property for compile if needed, but SanPham should match model
}
