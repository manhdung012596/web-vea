using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {
        private readonly ThoitrangnuContext _context;

        public CategoryController(ThoitrangnuContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.DanhMucs.ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                // Simple slug generation if not provided (for simplicity)
                if (string.IsNullOrEmpty(danhMuc.Slug))
                {
                    danhMuc.Slug = danhMuc.TenDanhMuc.ToLower().Replace(" ", "-");
                }

                if (await _context.DanhMucs.AnyAsync(d => d.Slug == danhMuc.Slug))
                {
                    ModelState.AddModelError("Slug", "Slug đã tồn tại");
                    return View(danhMuc);
                }

                _context.Add(danhMuc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null) return NotFound();

            return View(danhMuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DanhMuc danhMuc)
        {
            if (id != danhMuc.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMuc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucExists(danhMuc.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var danhMuc = await _context.DanhMucs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhMuc == null) return NotFound();

            return View(danhMuc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc != null)
            {
                _context.DanhMucs.Remove(danhMuc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucExists(int id)
        {
            return _context.DanhMucs.Any(e => e.Id == id);
        }
    }
}
