using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ThoitrangnuContext _context;

        public ProductController(ThoitrangnuContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.SanPhams
                .Include(p => p.AnhSanPhams)
                .Include(p => p.BienTheSanPhams)
                .ThenInclude(bt => bt.MauSac)
                .Include(p => p.BienTheSanPhams)
                .ThenInclude(bt => bt.KichCo)
                .Include(p => p.DanhMuc)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
