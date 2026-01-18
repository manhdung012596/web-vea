using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;
using System.Text.Json;

namespace EvaFashion.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ThoitrangnuContext _context;

        public CartController(ThoitrangnuContext context)
        {
            _context = context;
        }

        // Get Cart from Session
        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart))
            {
                return new List<CartItem>();
            }
            return JsonSerializer.Deserialize<List<CartItem>>(sessionCart);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int colorId, int sizeId, int quantity, bool buyNow = false)
        {
            // check variant
            var variant = await _context.BienTheSanPhams
                .Include(b => b.SanPham)
                .Include(b => b.MauSac)
                .Include(b => b.KichCo)
                .FirstOrDefaultAsync(b => b.SanPhamId == productId && b.MauId == colorId && b.KichCoId == sizeId);

            if (variant == null)
            {
                // Variant not found 
                return RedirectToAction("Detail", "Shop", new { id = productId });
                // Should add flash message here "Out of stock / Invalid"
            }

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.VariantId == variant.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = variant.SanPhamId,
                    ProductName = variant.SanPham?.TenSanPham ?? "Unknown",
                    Image = variant.SanPham?.HinhAnhChinh ?? "",
                    VariantId = variant.Id,
                    ColorName = variant.MauSac?.TenMau ?? "",
                    SizeName = variant.KichCo?.TenKichCo ?? "",
                    Price = variant.SanPham?.GiaSauGiam ?? variant.SanPham?.GiaGoc ?? 0,
                    Quantity = quantity
                });
            }

            SaveCart(cart);

            SaveCart(cart);

            if (buyNow)
            {
                return RedirectToAction("Index", "Checkout");
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int variantId)
        {
             var cart = GetCart();
             var item = cart.FirstOrDefault(c => c.VariantId == variantId);
             if (item != null)
             {
                 cart.Remove(item);
                 SaveCart(cart);
             }
             return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction(nameof(Index));
        }
    }
}
