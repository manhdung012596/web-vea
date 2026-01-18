using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;
using EvaFashion.Web.Areas.Admin.Models;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        private readonly ThoitrangnuContext _context;

        public DashboardController(ThoitrangnuContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                // Calculate Total Revenue (only Completed orders)
                TotalRevenue = await _context.DonHangs
                    .Where(o => o.TrangThai == "HoanThanh")
                    .SumAsync(o => o.TongTien ?? 0),

                // Count New Orders (Waiting for Confirmation)
                NewOrdersCount = await _context.DonHangs
                    .CountAsync(o => o.TrangThai == "ChoXacNhan"),

                // Count Active Products
                ProductCount = await _context.SanPhams
                    .CountAsync(p => p.IsActive == true),

                // Count Customers
                CustomerCount = await _context.NguoiDungs
                    .CountAsync(u => u.VaiTro == "khachhang"),

                // Recent 5 Orders
                RecentOrders = await _context.DonHangs
                    .OrderByDescending(o => o.NgayDat)
                    .Take(5)
                    .ToListAsync(),

                // Low Stock Products (< 10)
                LowStockProducts = await _context.BienTheSanPhams
                    .Include(b => b.SanPham)
                    .Include(b => b.MauSac)
                    .Include(b => b.KichCo)
                    .Where(b => b.SoLuongTon < 10)
                    .OrderBy(b => b.SoLuongTon)
                    .Take(10)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}
