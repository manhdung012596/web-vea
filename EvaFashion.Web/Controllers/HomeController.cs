using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EvaFashion.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaFashion.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EvaFashionDbContext _context;

    public HomeController(ILogger<HomeController> logger, EvaFashionDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var featuredProducts = await _context.SanPhams
            .Where(p => p.NoiBat == true && p.IsActive == true)
            .OrderByDescending(p => p.CreatedAt)
            .Take(8)
            .ToListAsync();

        return View(featuredProducts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
