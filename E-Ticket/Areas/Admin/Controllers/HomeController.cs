using System.Diagnostics;
using E_Ticket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket.Controllers
{
        [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin,Company")]

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public ILogger<HomeController> Logger => _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
}
