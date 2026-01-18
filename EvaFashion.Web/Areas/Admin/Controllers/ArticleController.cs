using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class ArticleController : BaseAdminController
    {
        private readonly EvaFashionDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleController(EvaFashionDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BaiViets.OrderByDescending(b => b.CreatedAt).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaiViet baiViet, IFormFile? fileHinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(baiViet.Slug) && !string.IsNullOrEmpty(baiViet.TieuDe))
                {
                    baiViet.Slug = baiViet.TieuDe.ToLower().Replace(" ", "-") + "-" + DateTime.Now.Ticks;
                }

                if (fileHinhAnh != null)
                {
                    string folder = "images/articles/";
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileHinhAnh.FileName);
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);
                    string filePath = Path.Combine(serverFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileHinhAnh.CopyToAsync(fileStream);
                    }
                    baiViet.HinhAnh = "/" + folder + fileName;
                }

                baiViet.CreatedBy = HttpContext.Session.GetInt32("UserId");
                _context.Add(baiViet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(baiViet);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null) return NotFound();
            return View(baiViet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BaiViet baiViet, IFormFile? fileHinhAnh)
        {
            if (id != baiViet.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.BaiViets.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                     if (fileHinhAnh != null)
                    {
                        string folder = "images/articles/";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileHinhAnh.FileName);
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                        if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);
                        string filePath = Path.Combine(serverFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await fileHinhAnh.CopyToAsync(fileStream);
                        }
                        baiViet.HinhAnh = "/" + folder + fileName;
                    }
                    else
                    {
                        baiViet.HinhAnh = existing?.HinhAnh;
                    }
                    baiViet.CreatedAt = existing?.CreatedAt;
                    baiViet.CreatedBy = existing?.CreatedBy;

                    _context.Update(baiViet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaiVietExists(baiViet.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(baiViet);
        }

         public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null) return NotFound();
            return View(baiViet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var baiViet = await _context.BaiViets.FindAsync(id);
             if (baiViet != null)
             {
                 _context.BaiViets.Remove(baiViet);
                 await _context.SaveChangesAsync();
             }
             return RedirectToAction(nameof(Index));
        }

        private bool BaiVietExists(int id)
        {
            return _context.BaiViets.Any(e => e.Id == id);
        }
    }
}
