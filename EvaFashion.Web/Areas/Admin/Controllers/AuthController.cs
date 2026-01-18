using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;
using Microsoft.AspNetCore.Http;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly ThoitrangnuContext _context;

        public AuthController(ThoitrangnuContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Role") == "admin")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            var passwordHash = HashPassword(matKhau);
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap && u.MatKhau == passwordHash && u.VaiTro == "admin");

            if (user != null)
            {
                // Set Session
                HttpContext.Session.SetInt32("UserId", user.MaNguoiDung);
                HttpContext.Session.SetString("UserName", user.HoTen ?? user.TenDangNhap);
                HttpContext.Session.SetString("Role", user.VaiTro);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Tài khoản không hợp lệ hoặc không có quyền truy cập";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
