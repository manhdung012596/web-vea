using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class SeedController : BaseAdminController
    {
        private readonly EvaFashionDbContext _context;

        public SeedController(EvaFashionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await SeedAttributes();
            await SeedCategories();
            await SeedProducts();

            TempData["SuccessMessage"] = "Đã thêm dữ liệu mẫu thành công!";
            return RedirectToAction("Index", "Dashboard");
        }

        private async Task SeedAttributes()
        {
            // Colors
            if (!await _context.MauSacs.AnyAsync())
            {
                var colors = new List<MauSac>
                {
                    new MauSac { TenMau = "Trắng", MaHex = "#FFFFFF" },
                    new MauSac { TenMau = "Đen", MaHex = "#000000" },
                    new MauSac { TenMau = "Đỏ", MaHex = "#FF0000" },
                    new MauSac { TenMau = "Xanh Dương", MaHex = "#0000FF" },
                    new MauSac { TenMau = "Hồng Pastel", MaHex = "#FFD1DC" }
                };
                _context.MauSacs.AddRange(colors);
            }

            // Sizes
            if (!await _context.KichCos.AnyAsync())
            {
                var sizes = new List<KichCo>
                {
                    new KichCo { TenKichCo = "S" },
                    new KichCo { TenKichCo = "M" },
                    new KichCo { TenKichCo = "L" },
                    new KichCo { TenKichCo = "XL" }
                };
                _context.KichCos.AddRange(sizes);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedCategories()
        {
            if (!await _context.DanhMucs.AnyAsync(c => c.Slug == "ao-thoi-trang"))
            {
                _context.DanhMucs.Add(new DanhMuc { TenDanhMuc = "Áo Thời Trang", Slug = "ao-thoi-trang" });
            }
            if (!await _context.DanhMucs.AnyAsync(c => c.Slug == "vay-xinh"))
            {
                _context.DanhMucs.Add(new DanhMuc { TenDanhMuc = "Váy Xinh", Slug = "vay-xinh" });
            }
            if (!await _context.DanhMucs.AnyAsync(c => c.Slug == "dam-da-hog"))
            {
                _context.DanhMucs.Add(new DanhMuc { TenDanhMuc = "Đầm Dạ Hội", Slug = "dam-da-ho" });
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedProducts()
        {
            var catAo = await _context.DanhMucs.FirstOrDefaultAsync(c => c.Slug == "ao-thoi-trang");
            var catVay = await _context.DanhMucs.FirstOrDefaultAsync(c => c.Slug == "vay-xinh");
            var catDam = await _context.DanhMucs.FirstOrDefaultAsync(c => c.Slug == "dam-da-ho");

            // Helper to create product
            async Task CreateProductIfNotExists(DanhMuc cat, string name, decimal price, string img)
            {
                if (cat == null) return;
                string slug = name.ToLower().Replace(" ", "-") + "-" + DateTime.Now.Ticks; // simple slug
                
                if (!await _context.SanPhams.AnyAsync(p => p.TenSanPham == name))
                {
                    var p = new SanPham
                    {
                        TenSanPham = name,
                        Slug = slug,
                        DanhMucId = cat.Id,
                        GiaGoc = price,
                        PhanTramGiam = 0,
                        MoTaNgan = $"Mô tả ngắn cho {name}",
                        MoTaChiTiet = $"Mô tả chi tiết đầy đủ cho sản phẩm {name}. Chất liệu cao cấp, thiết kế hiện đại.",
                        HinhAnhChinh = img, // Using placeholder or handled manually
                        NoiBat = true,
                        IsActive = true
                    };
                    _context.SanPhams.Add(p);
                    await _context.SaveChangesAsync();

                    // Seed Variants
                    var colors = await _context.MauSacs.Take(2).ToListAsync();
                    var sizes = await _context.KichCos.Take(2).ToListAsync();
                    foreach(var c in colors)
                    {
                        foreach(var s in sizes)
                        {
                            _context.BienTheSanPhams.Add(new BienTheSanPham
                            {
                                SanPhamId = p.Id,
                                MauId = c.MaMau,
                                KichCoId = s.MaKichCo,
                                SoLuongTon = 20
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }

            // Aos
            await CreateProductIfNotExists(catAo, "Áo Thun Basic Trắng", 150000, "https://placehold.co/400x400?text=Ao+Thun");
            await CreateProductIfNotExists(catAo, "Áo Sơ Mi Công Sở", 350000, "https://placehold.co/400x400?text=Ao+So+Mi");
            await CreateProductIfNotExists(catAo, "Áo Croptop Năng Động", 200000, "https://placehold.co/400x400?text=Ao+Croptop");

            // Vays
            await CreateProductIfNotExists(catVay, "Chân Váy Xếp Ly", 250000, "https://placehold.co/400x400?text=Chan+Vay");
            await CreateProductIfNotExists(catVay, "Váy Jeans Dài", 320000, "https://placehold.co/400x400?text=Vay+Jeans");
            await CreateProductIfNotExists(catVay, "Váy Chữ A Caro", 280000, "https://placehold.co/400x400?text=Vay+Chu+A");

            // Dams
            await CreateProductIfNotExists(catDam, "Đầm Body Quyến Rũ", 550000, "https://placehold.co/400x400?text=Dam+Body");
            await CreateProductIfNotExists(catDam, "Đầm Xòe Công Chúa", 600000, "https://placehold.co/400x400?text=Dam+Xoe");
            await CreateProductIfNotExists(catDam, "Đầm Maxi Đi Biển", 450000, "https://placehold.co/400x400?text=Dam+Maxi");
        }
    }
}
