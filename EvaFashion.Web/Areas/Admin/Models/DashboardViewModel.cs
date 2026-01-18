using System.Collections.Generic;
using EvaFashion.Web.Models;

namespace EvaFashion.Web.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int NewOrdersCount { get; set; }
        public int ProductCount { get; set; }
        public int CustomerCount { get; set; }
        public List<DonHang> RecentOrders { get; set; }
        public List<BienTheSanPham> LowStockProducts { get; set; }
    }
}
