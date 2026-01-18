using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class OrderController : BaseAdminController
    {
        private readonly EvaFashionDbContext _context;

        public OrderController(EvaFashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index(string? status)
        {
            var ordersQuery = _context.DonHangs
                .Include(o => o.NguoiDung)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                ordersQuery = ordersQuery.Where(o => o.TrangThai == status);
            }

            var orders = await ordersQuery.OrderByDescending(o => o.NgayDat).ToListAsync();
            ViewData["CurrentStatus"] = status ?? "All";
            return View(orders);
        }

        // GET: Admin/Order/Detail/5
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.DonHangs
                .Include(o => o.NguoiDung)
                .Include(o => o.ChiTietDonHangs)
                    .ThenInclude(d => d.BienTheSanPham)
                        .ThenInclude(b => b.SanPham)
                .Include(o => o.ChiTietDonHangs)
                    .ThenInclude(d => d.BienTheSanPham)
                        .ThenInclude(b => b.MauSac)
                 .Include(o => o.ChiTietDonHangs)
                    .ThenInclude(d => d.BienTheSanPham)
                        .ThenInclude(b => b.KichCo)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Admin/Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string trangThai)
        {
            var order = await _context.DonHangs
                .Include(o => o.ChiTietDonHangs)
                .ThenInclude(ct => ct.BienTheSanPham)
                .FirstOrDefaultAsync(o => o.MaDonHang == id);

            if (order == null) return NotFound();

            string oldStatus = order.TrangThai;
            order.TrangThai = trangThai;
            order.NgayCapNhat = DateTime.Now;

            // Logic: If Cancelling (DaHuy) and not already cancelled
            if (trangThai == "DaHuy" && oldStatus != "DaHuy")
            {
                // Restore Stock
                foreach (var item in order.ChiTietDonHangs)
                {
                    if (item.BienTheSanPham != null)
                    {
                        item.BienTheSanPham.SoLuongTon += item.SoLuong ?? 0;
                        _context.Update(item.BienTheSanPham);
                    }
                }
            }
            // Note: Deducting stock usually happens at Checkout. 
            // If you want to support "Re-open" cancelled orders, you'd need opposite logic here.
            // For now, assuming simple 1-way flow or manual adjustment for complex cases.

            // Logic: Payment Status Update if Completed
            if (trangThai == "HoanThanh")
            {
                order.TrangThaiThanhToan = "DaThanhToan";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), new { id = id });
        }
    }
}
