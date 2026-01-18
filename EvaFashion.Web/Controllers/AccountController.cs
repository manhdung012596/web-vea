using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;
using Microsoft.AspNetCore.Http;

namespace EvaFashion.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ThoitrangnuContext _context;

        public AccountController(ThoitrangnuContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
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
                .FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap && u.MatKhau == passwordHash);

            if (user != null)
            {
                // Set Session
                HttpContext.Session.SetInt32("UserId", user.MaNguoiDung);
                HttpContext.Session.SetString("UserName", user.HoTen ?? user.TenDangNhap);
                HttpContext.Session.SetString("Role", user.VaiTro);

                if (user.VaiTro == "admin")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(NguoiDung model, string confirmPassword)
        {
            if (string.IsNullOrEmpty(model.TenDangNhap) || string.IsNullOrEmpty(model.MatKhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            if (model.MatKhau != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp";
                return View();
            }

            if (await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == model.TenDangNhap))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại";
                return View();
            }

            model.MatKhau = HashPassword(model.MatKhau);
            model.VaiTro = "khachhang";
            model.NgayTao = DateTime.Now;

            _context.NguoiDungs.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
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
