using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaFashion.Web.Models;
using System.Security.Cryptography;
using System.Text;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly ThoitrangnuContext _context;

        public UserController(ThoitrangnuContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.NguoiDungs.ToListAsync();
            return View(users);
        }

        // Feature: Lock/Unlock User (using simple logic, maybe toggle role or password reset in real app, here assuming just listing)
        // Since there is no IsLocked column in schema, we will skip locking for now or implement Role change.
        
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
             var user = await _context.NguoiDungs.FindAsync(id);
             if(user != null)
             {
                 user.VaiTro = role;
                 await _context.SaveChangesAsync();
             }
             return RedirectToAction(nameof(Index));
        }
    }
}
