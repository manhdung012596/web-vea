using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class VoucherController : BaseAdminController
    {
         private readonly EvaFashionDbContext _context;

        public VoucherController(EvaFashionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Vouchers.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                // Check dup code
                if (await _context.Vouchers.AnyAsync(v => v.MaCode == voucher.MaCode))
                {
                    ModelState.AddModelError("MaCode", "Mã code này đã tồn tại!");
                    return View(voucher);
                }

                _context.Add(voucher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var voucher = await _context.Vouchers.FindAsync(id);
             if (voucher != null)
             {
                 _context.Vouchers.Remove(voucher);
                 await _context.SaveChangesAsync();
             }
             return RedirectToAction(nameof(Index));
        }
    }
}
