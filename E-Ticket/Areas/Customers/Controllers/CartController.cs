using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;
using E_Ticket.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket.Areas.Customers.Controllers
{
        [Area("Customers")]
    public class CartController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private ICartRepository _cartRepository;
        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;
        public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public ActionResult AddToCart(int MovieId, int count)
        {
            var user = _userManager.GetUserId(User);

            var cart = new Cart()
            {
                MovieId = MovieId,
                Count = count,
                ApplicationUserId = user

            };

            var cartDb = _cartRepository.GetOne(e=>e.MovieId == MovieId && e.ApplicationUserId == user);
            if (cartDb != null)
                cartDb.Count += count;
            else
            _cartRepository.Create(cart);
            _cartRepository.Commit();

            return RedirectToAction("Index", "Movies", new { area = "Customers" });

        }

    public ActionResult Index()
    {
        var cart = _cartRepository.Get();
            if( cart == null)
            {
                RedirectToAction("NotFoundPage");
            }
        return View(cart);
            
    }
        public IActionResult NotFoundPage()
        {
            return View();
        }

    }
}
