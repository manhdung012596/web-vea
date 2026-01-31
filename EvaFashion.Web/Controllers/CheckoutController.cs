using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;
using System.Text.Json;

namespace EvaFashion.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ThoitrangnuContext _context;

        public CheckoutController(ThoitrangnuContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (role == "admin")
            {
                context.Result = new RedirectToActionResult("Index", "Dashboard", new { area = "Admin" });
            }
            base.OnActionExecuting(context);
        }

        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart)) return new List<CartItem>();
            return JsonSerializer.Deserialize<List<CartItem>>(sessionCart);
        }

        // GET: /Checkout
        public IActionResult Index()
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.Cart = cart;
            ViewBag.Total = cart.Sum(i => i.Total);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(DonHang order)
        {
            var cart = GetCart();
            if (cart == null || !cart.Any()) return RedirectToAction("Index", "Cart");

            try 
            {
                // 1. Create Order
                order.MaDonHangCode = "DH" + DateTime.Now.Ticks.ToString().Substring(10); // Simple Code
                order.NgayDat = DateTime.Now;
                order.NgayCapNhat = DateTime.Now;
                order.TongTien = cart.Sum(i => i.Total);
                order.TrangThai = "ChoXacNhan";
                order.TrangThaiThanhToan = "ChuaThanhToan";

                // If logged in, link user
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {
                    order.MaNguoiDung = userId.Value;
                }

                _context.Add(order);
                await _context.SaveChangesAsync();

                // 2. Create Order Details
                foreach (var item in cart)
                {
                    var detail = new ChiTietDonHang
                    {
                        MaDonHang = order.MaDonHang,
                        MaBienThe = item.VariantId,
                        SoLuong = item.Quantity,
                        DonGia = item.Price,
                        ThanhTien = item.Total
                    };
                    _context.Add(detail);

                    // Optional: Decrease Stock
                    var variant = await _context.BienTheSanPhams.FindAsync(item.VariantId);
                    if (variant != null)
                    {
                        variant.SoLuongTon -= item.Quantity;
                        _context.Update(variant);
                    }
                }
                await _context.SaveChangesAsync();

                // 3. Clear Cart
                HttpContext.Session.Remove("Cart");

                return RedirectToAction("Success", new { id = order.MaDonHangCode });
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Lỗi đặt hàng: " + ex.Message);
                ViewBag.Cart = cart;
                ViewBag.Total = cart.Sum(i => i.Total);
                return View("Index", order);
            }
        }

        public async Task<IActionResult> Success(string id)
        {
            var order = await _context.DonHangs.FirstOrDefaultAsync(d => d.MaDonHangCode == id);
            if (order == null) return RedirectToAction("Index", "Home");
            return View(order);
        }
    }
}
