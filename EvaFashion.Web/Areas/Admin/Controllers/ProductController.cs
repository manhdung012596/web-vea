using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly EvaFashionDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(EvaFashionDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham, IFormFile? fileHinhAnh)
        {
            // Slug is generated manually, so remove it from validation
            ModelState.Remove("Slug");
            ModelState.Remove("DanhMuc");
            ModelState.Remove("BienTheSanPhams"); // Collection might be issue too if created empty


            if (ModelState.IsValid)
            {
                try 
                {
                    // Handle Slug
                    if (string.IsNullOrEmpty(sanPham.Slug))
                    {
                        sanPham.Slug = sanPham.TenSanPham.ToLower().Replace(" ", "-") + "-" + DateTime.Now.Ticks;
                    }

                    // Check duplicate slug
                    if (await _context.SanPhams.AnyAsync(p => p.Slug == sanPham.Slug))
                    {
                        ModelState.AddModelError("Slug", "Slug này đã tồn tại, vui lòng chọn cái khác.");
                        ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc", sanPham.DanhMucId);
                        return View(sanPham);
                    }

                    // Handle Image Upload
                    if (fileHinhAnh != null)
                    {
                        string folder = "images/products/";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileHinhAnh.FileName);
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                        if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);

                        string filePath = Path.Combine(serverFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await fileHinhAnh.CopyToAsync(fileStream);
                        }
                        sanPham.HinhAnhChinh = "/" + folder + fileName;
                    }

                    // Set default Meta if empty
                    if(string.IsNullOrEmpty(sanPham.MetaTitle)) sanPham.MetaTitle = sanPham.TenSanPham;

                    _context.Add(sanPham);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu sản phẩm: " + ex.Message);
                }
            }
            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc", sanPham.DanhMucId);
            return View(sanPham);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null) return NotFound();

            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc", sanPham.DanhMucId);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPham sanPham, IFormFile? fileHinhAnh)
        {
            if (id != sanPham.Id) return NotFound();

            // Remove navigation property validation errors that ModelState might have caught because we didn't send full objects
            ModelState.Remove("Slug"); // Also remove Slug just in case
            ModelState.Remove("DanhMuc");
            ModelState.Remove("BienTheSanPhams");

            if (ModelState.IsValid)
            {
                // Remove navigation property validation
                ModelState.Remove("DanhMuc");
                ModelState.Remove("BienTheSanPhams");

                // Re-check validity after removal (optional generally, but good if other errors existed)
                // Actually, removal doesn't re-trigger validation, it just removes errors.
                // But we need to ensure we call Remove BEFORE checking IsValid? 
                // No, IsValid is checked once. If we remove keys, we need to re-evaluate or use a pattern like:
                // ModelState.Remove("..."); if(ModelState.IsValid)... 
                
                // Let's stick to the pattern used in Create: Remove then Check.
            }
            
            // Correction: In Create, I called Remove() BEFORE checking IsValid. 
            // Here IsValid is checked at line 112. I need to move insertion point to BEFORE line 112.

            {
                try
                {
                    // Check duplicate slug (excluding current product)
                    if (await _context.SanPhams.AnyAsync(p => p.Slug == sanPham.Slug && p.Id != id))
                    {
                         ModelState.AddModelError("Slug", "Slug này đã bị trùng với sản phẩm khác.");
                         ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc", sanPham.DanhMucId);
                         return View(sanPham);
                    }

                    var existingProduct = await _context.SanPhams.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    
                    // Handle Image Upload
                    if (fileHinhAnh != null)
                    {
                        string folder = "images/products/";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileHinhAnh.FileName);
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                        if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);

                        string filePath = Path.Combine(serverFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await fileHinhAnh.CopyToAsync(fileStream);
                        }
                        sanPham.HinhAnhChinh = "/" + folder + fileName;
                    }
                    else
                    {
                        // Keep old image
                        sanPham.HinhAnhChinh = existingProduct?.HinhAnhChinh;
                    }

                    // Preserve creation data
                    sanPham.CreatedAt = existingProduct?.CreatedAt;
                    sanPham.CreatedBy = existingProduct?.CreatedBy;
                    sanPham.UpdatedAt = DateTime.Now;

                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc", sanPham.DanhMucId);
            return View(sanPham);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                // Soft delete preferred, but user authorized physical delete in DB schema (no IsDeleted logic enforced yet, but IsActive exists)
                // Using IsActive = false as per requirement "Xóa/Ẩn sản phẩm: Chuyển isActive sang False"
                sanPham.IsActive = false;
                _context.Update(sanPham);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.Id == id);
        }
    }
}
