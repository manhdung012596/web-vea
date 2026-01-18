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

            // var products = new[]
            // {
            //     new SanPham 
            //     { 
            //         TenSanPham = "Đầm Hoa Nhí Mùa Xuân", Slug = "dam-hoa-mua-xuan", DanhMucId = catVay,
            //         ThuongHieu = "Eva Design", 
            //         MoTaNgan = "Thiết kế nhẹ nhàng, bay bổng với họa tiết hoa nhí. Chất liệu voan cao cấp.",
            //         GiaGoc = 550000, PhanTramGiam = 10, HinhAnhChinh = "/images/products/p1.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
            //     },
            //     new SanPham 
            //     { 
            //         TenSanPham = "Áo Blazer Công Sở Thanh Lịch", Slug = "ao-blazer-cong-so", DanhMucId = catAo,
            //         ThuongHieu = "Eva Office", 
            //         MoTaNgan = "Phong cách chuyên nghiệp, sang trọng. Màu kem nhã nhặn dễ phối đồ.",
            //         GiaGoc = 850000, PhanTramGiam = 0, HinhAnhChinh = "/images/products/p2.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
            //     },
            //     new SanPham 
            //     { 
            //         TenSanPham = "Chân Váy Xếp Ly Hồng", Slug = "chan-vay-xep-ly", DanhMucId = catChanVay,
            //         ThuongHieu = "Eva Young", 
            //         MoTaNgan = "Dáng dài qua gối, xếp ly tinh tế. Màu hồng pastel ngọt ngào.",
            //         GiaGoc = 420000, PhanTramGiam = 5, HinhAnhChinh = "/images/products/p3.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
            //     },
            //     new SanPham 
            //     { 
            //         TenSanPham = "Đầm Dạ Hội Satin Đỏ Rượu", Slug = "dam-da-hoi-cao-cap", DanhMucId = catVay,
            //         ThuongHieu = "Eva Luxury", 
            //         MoTaNgan = "Thiết kế trễ vai quyến rũ, chất liệu Satin bóng mượt sang trọng.",
            //         GiaGoc = 1200000, PhanTramGiam = 0, HinhAnhChinh = "/images/products/p4.png", IsActive = true, NoiBat = true, CreatedAt = DateTime.Now 
            //     }
            // };

            // foreach (var p in products)
            // {
            //     var existing = context.SanPhams.FirstOrDefault(x => x.Slug == p.Slug);
            //     if (existing == null)
            //     {
            //         context.SanPhams.Add(p);
            //     }
            //     else
            //     {
            //         existing.HinhAnhChinh = p.HinhAnhChinh;
            //         existing.NoiBat = true;
            //     }
            // }
            // context.SaveChanges();

            // 3. Colors & Sizes
            if (!context.MauSacs.Any())
            {
                context.MauSacs.AddRange(
                    new MauSac { TenMau = "Trắng", MaHex = "#FFFFFF" },
                    new MauSac { TenMau = "Đen", MaHex = "#000000" },
                    new MauSac { TenMau = "Hồng", MaHex = "#FFC0CB" },
                    new MauSac { TenMau = "Đỏ", MaHex = "#FF0000" },
                    new MauSac { TenMau = "Xanh Dương", MaHex = "#0000FF" },
                    new MauSac { TenMau = "Vàng", MaHex = "#FFFF00" },
                    new MauSac { TenMau = "Kem", MaHex = "#F5F5DC" }
                );
            }

            if (!context.KichCos.Any())
            {
                context.KichCos.AddRange(
                    new KichCo { TenKichCo = "S" },
                    new KichCo { TenKichCo = "M" },
                    new KichCo { TenKichCo = "L" },
                    new KichCo { TenKichCo = "XL" }
                );
            }
            context.SaveChanges();

            // 4. Variants (BienTheSanPham) - Ensure ALL products have variants
            // Check if ANY product needs variants, not just if table is empty
            var productsWithoutVariants = context.SanPhams
                .Include(p => p.BienTheSanPhams)
                .Where(p => !p.BienTheSanPhams.Any())
                .ToList();

            if (productsWithoutVariants.Any())
            {
                var colors = context.MauSacs.ToList();
                var sizes = context.KichCos.ToList();
                var random = new Random();

                if (colors.Any() && sizes.Any())
                {
                    foreach (var p in productsWithoutVariants)
                    {
                        // Create variants: Take 2 random colors
                        var selectedColors = colors.OrderBy(x => random.Next()).Take(2).ToList();

                        foreach (var c in selectedColors)
                        {
                            foreach (var s in sizes)
                            {
                                context.BienTheSanPhams.Add(new BienTheSanPham
                                {
                                    SanPhamId = p.Id,
                                    MauId = c.MaMau,
                                    KichCoId = s.MaKichCo,
                                    SoLuongTon = 50,
                                    HinhAnh = p.HinhAnhChinh 
                                });
                            }
                        }
                    }
                    context.SaveChanges();
                }
            }
    }
}
}
