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
        private readonly EvaFashion.Web.Services.IEmailService _emailService;

        public AccountController(ThoitrangnuContext context, EvaFashion.Web.Services.IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var orders = await _context.DonHangs
                .Where(o => o.MaNguoiDung == userId.Value)
                .OrderByDescending(o => o.NgayDat)
                .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống";
                return View();
            }

            var token = Guid.NewGuid().ToString();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1);
            await _context.SaveChangesAsync();

            var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);
            var message = $"Vui lòng nhấp vào link sau để đặt lại mật khẩu: <a href='{resetLink}'>{resetLink}</a>";
            
            await _emailService.SendEmailAsync(email, "Đặt lại mật khẩu", message);

            ViewBag.Message = "Vui lòng kiểm tra email để đặt lại mật khẩu";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }
            return View(new { Token = token }); // Passing token via model or ViewBag is fine, using anonymous object here for View usage flexibility or just ViewBag explicitly below
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp";
                return View((object)new { Token = token }); // Keep token in view if error
            }

            var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);
            if (user == null)
            {
                ViewBag.Error = "Link đặt lại mật khẩu không hợp lệ hoặc đã hết hạn";
                return View((object)new { Token = token });
            }

            user.MatKhau = HashPassword(password);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();

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
