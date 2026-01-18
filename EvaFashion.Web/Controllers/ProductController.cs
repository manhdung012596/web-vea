using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly EvaFashionDbContext _context;

        public ProductController(EvaFashionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.SanPhams
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
