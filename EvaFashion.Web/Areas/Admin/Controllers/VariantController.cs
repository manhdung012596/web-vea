using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class VariantController : BaseAdminController
    {
        private readonly ThoitrangnuContext _context;

        public VariantController(ThoitrangnuContext context)
        {
            _context = context;
        }

        // GET: Admin/Variant?productId=5
        public async Task<IActionResult> Index(int? productId)
        {
            if (productId == null) return NotFound();

            var sanPham = await _context.SanPhams.FindAsync(productId);
            if (sanPham == null) return NotFound();

            ViewData["SanPham"] = sanPham;

            var variants = await _context.BienTheSanPhams
                .Include(b => b.MauSac)
                .Include(b => b.KichCo)
                .Where(b => b.SanPhamId == productId)
                .OrderBy(b => b.MauSac.TenMau)
                .ToListAsync();

            return View(variants);
        }

        // GET: Admin/Variant/Create?productId=5
        public IActionResult Create(int? productId)
        {
            if (productId == null) return NotFound();

            ViewData["ProductId"] = productId;
            ViewData["MauId"] = new SelectList(_context.MauSacs, "MaMau", "TenMau");
            ViewData["KichCoId"] = new SelectList(_context.KichCos, "MaKichCo", "TenKichCo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BienTheSanPham variant)
        {
            if (ModelState.IsValid)
            {
                // Check if this variant already exists
                var exists = await _context.BienTheSanPhams.AnyAsync(
                    x => x.SanPhamId == variant.SanPhamId && 
                         x.MauId == variant.MauId && 
                         x.KichCoId == variant.KichCoId);

                if (exists)
                {
                    ModelState.AddModelError("", "Biến thể này (Màu + Size) đã tồn tại!");
                }
                else
                {
                    _context.Add(variant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { productId = variant.SanPhamId });
                }
            }

            ViewData["ProductId"] = variant.SanPhamId;
            ViewData["MauId"] = new SelectList(_context.MauSacs, "MaMau", "TenMau", variant.MauId);
            ViewData["KichCoId"] = new SelectList(_context.KichCos, "MaKichCo", "TenKichCo", variant.KichCoId);
            return View(variant);
        }

        // POST: Admin/Variant/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var variant = await _context.BienTheSanPhams.FindAsync(id);
            if (variant != null)
            {
                int productId = variant.SanPhamId;

                // Check dependencies
                bool isOrdered = await _context.ChiTietDonHangs.AnyAsync(ct => ct.MaBienThe == id);
                bool isInCart = await _context.GioHangChiTiets.AnyAsync(ct => ct.BienTheId == id);

                if (isOrdered || isInCart)
                {
                    // Cannot delete, redirect with error (needs TempData support in View, skipping for now to just avoid crash)
                    // Just return to Index, maybe add a query param for error?
                    return RedirectToAction(nameof(Index), new { productId = productId, error = "CannotDelete" });
                }

                _context.BienTheSanPhams.Remove(variant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { productId = productId });
            }
            return NotFound();
        }
    }
}
