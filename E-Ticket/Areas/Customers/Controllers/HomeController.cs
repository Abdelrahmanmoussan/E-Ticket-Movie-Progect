using System.Diagnostics;
using E_Ticket.DataAccess;
using E_Ticket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Ticket.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class HomeController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();

        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Home()
        {
            return View();
        }
    }
}
