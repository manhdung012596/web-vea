using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly ThoitrangnuContext _context;

        public ShopController(ThoitrangnuContext context)
        {
            _context = context;
        }

        // GET: /Shop
        public async Task<IActionResult> Index(int? categoryId, string? u, string sortOrder)
        {
            var products = _context.SanPhams
                .Include(p => p.DanhMuc)
                .Where(p => p.IsActive == true)
                .AsQueryable();

            // Filter by Category
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.DanhMucId == categoryId);
                ViewData["CurrentCategory"] = categoryId;
            }

            // Search
            if (!string.IsNullOrEmpty(u))
            {
                products = products.Where(p => p.TenSanPham.Contains(u));
                ViewData["CurrentSearch"] = u;
            }

            // Sort
            ViewData["PriceSortParm"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            switch (sortOrder)
            {
                case "price_desc":
                    products = products.OrderByDescending(s => s.GiaSauGiam);
                    break;
                case "price_asc":
                    products = products.OrderBy(s => s.GiaSauGiam);
                    break;
                case "date_asc":
                    products = products.OrderBy(s => s.CreatedAt);
                    break;
                default: // date_desc
                    products = products.OrderByDescending(s => s.CreatedAt);
                    break;
            }

            ViewData["Categories"] = await _context.DanhMucs.ToListAsync();
            return View(await products.ToListAsync());
        }

        // GET: /Shop/Detail/5
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.SanPhams
                .Include(p => p.DanhMuc)
                .Include(p => p.BienTheSanPhams)
                    .ThenInclude(btc => btc.MauSac)
                .Include(p => p.BienTheSanPhams)
                    .ThenInclude(btc => btc.KichCo)
                .Include(p => p.DanhGias)
                    .ThenInclude(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            // Get unique colors and sizes available for this product
            ViewBag.Colors = product.BienTheSanPhams
                .Select(b => b.MauSac)
                .DistinctBy(m => m.MaMau) // Requires .NET 6+
                .ToList();
                
            ViewBag.Sizes = product.BienTheSanPhams
                .Select(b => b.KichCo)
                .DistinctBy(k => k.MaKichCo)
                .OrderBy(k => k.MaKichCo) // Simple sort, ideally custom sort (S, M, L)
                .ToList();

            // Related products
            ViewBag.RelatedProducts = await _context.SanPhams
                .Where(p => p.DanhMucId == product.DanhMucId && p.Id != product.Id)
                .Take(4)
                .ToListAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, int rating, string content)
        {
            // Check login via Session (UserId stored as Int32)
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account"); 
            }

            // Check if user has purchased the product and order is "HoanThanh"
            var hasPurchased = await _context.DonHangs
                .AnyAsync(dh => dh.MaNguoiDung == userId.Value 
                                && dh.TrangThai == "HoanThanh" 
                                && dh.ChiTietDonHangs.Any(ct => ct.BienTheSanPham.SanPhamId == productId));

            if (!hasPurchased)
            {
                // Optionally set an error message in TempData
                return RedirectToAction("Detail", new { id = productId });
            }

            if (rating < 1) rating = 1;
            if (rating > 5) rating = 5;

            var review = new DanhGia
            {
                SanPhamId = productId,
                MaNguoiDung = userId.Value,
                SoSao = rating,
                NoiDung = content,
                NgayDanhGia = DateTime.Now
            };

            _context.DanhGias.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = productId });
        }
    }
}
