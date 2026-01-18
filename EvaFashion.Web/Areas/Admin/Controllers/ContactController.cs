using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class ContactController : BaseAdminController
    {
        private readonly EvaFashionDbContext _context;

        public ContactController(EvaFashionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.LienHes.OrderByDescending(l => l.NgayGui).ToListAsync());
        }

        public async Task<IActionResult> MarkProcessed(int id)
        {
            var lienHe = await _context.LienHes.FindAsync(id);
            if (lienHe != null)
            {
                lienHe.TrangThai = "DaXuLy";
                lienHe.DaDoc = true;
                await _context.SaveChangesAsync();
            }
             return RedirectToAction(nameof(Index));
        }
    }
}
