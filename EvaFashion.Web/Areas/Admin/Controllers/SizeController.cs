using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class SizeController : BaseAdminController
    {
        private readonly ThoitrangnuContext _context;

        public SizeController(ThoitrangnuContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.KichCos.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KichCo kichCo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kichCo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kichCo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var kichCo = await _context.KichCos.FindAsync(id);
            if (kichCo == null) return NotFound();
            return View(kichCo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KichCo kichCo)
        {
            if (id != kichCo.MaKichCo) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kichCo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.KichCos.Any(e => e.MaKichCo == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kichCo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var kichCo = await _context.KichCos.FirstOrDefaultAsync(m => m.MaKichCo == id);
            if (kichCo == null) return NotFound();
            return View(kichCo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kichCo = await _context.KichCos.FindAsync(id);
            if (kichCo != null)
            {
                _context.KichCos.Remove(kichCo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
