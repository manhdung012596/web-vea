using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class ColorController : BaseAdminController
    {
        private readonly EvaFashionDbContext _context;

        public ColorController(EvaFashionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.MauSacs.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MauSac mauSac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mauSac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mauSac);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var mauSac = await _context.MauSacs.FindAsync(id);
            if (mauSac == null) return NotFound();
            return View(mauSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MauSac mauSac)
        {
            if (id != mauSac.MaMau) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mauSac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MauSacs.Any(e => e.MaMau == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mauSac);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var mauSac = await _context.MauSacs.FirstOrDefaultAsync(m => m.MaMau == id);
            if (mauSac == null) return NotFound();
            return View(mauSac);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mauSac = await _context.MauSacs.FindAsync(id);
            if (mauSac != null)
            {
                _context.MauSacs.Remove(mauSac);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
