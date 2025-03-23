using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;
using E_Ticket.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Stripe.Checkout;

namespace E_Ticket.Areas.Customers.Controllers
{
        [Area("Customers")]
        [Authorize]
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

    public ActionResult Index()
    {
        var cart = _cartRepository.Get(e=>e.ApplicationUserId == _userManager.GetUserId(User), includes: [e=>e.Movie, e=>e.ApplicationUser]);
            if(!cart.Any())
            {
                ViewBag.Message = "Your cart is empty.";
            }
            var totalPrice = cart.Sum(e=>e.Movie.Price *e.Count);

            ViewBag.TotalPrice = totalPrice;
        return View(cart);      
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

        public IActionResult Increment(int MovieId)
        {
            var cart = _cartRepository.GetOne(e => e.ApplicationUserId == _userManager.GetUserId(User) && e.MovieId == MovieId);

            if (cart != null)
            {
                cart.Count++;
                _cartRepository.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrement(int MovieId)
        {
            var cart = _cartRepository.GetOne(e => e.ApplicationUserId == _userManager.GetUserId(User) && e.MovieId == MovieId);
            if (cart != null && cart.Count >1)
            {
                cart.Count--;
                _cartRepository.Commit();
            }
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Pay()
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartRepository.Get(e => e.ApplicationUserId == userId, includes: [e => e.Movie, e => e.ApplicationUser]);

            var order = new Order();
            order.ApplicationUserId = userId;
            order.OrderDate = DateTime.Now;
            order.OrderTotal = (double)cart.Sum(e => e.Movie.Price * e.Count);

            _orderRepository.Create(order);
            _orderRepository.Commit();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customers/Checkout/Success?orderId={order.Id}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customers/Checkout/Cancel",
            };

            foreach (var item in cart)
            {
                options.LineItems.Add(
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Movie.Name,
                                Description = item.Movie.Description,
                            },
                            UnitAmount = (long)item.Movie.Price * 100 ,
                        },
                        Quantity = item.Count,
                    }
                );
            }

            var service = new SessionService();
            var session = service.Create(options);
            order.SessionId = session.Id;
            _orderRepository.Commit();

            List<OrderItem> orderItems = [];
            foreach (var item in cart)
            {
                var orderItem = new OrderItem()
                {
                    OrderId = order.Id,
                    Count = item.Count,
                    Price = (double)item.Movie.Price,
                    MovieId = item.MovieId,
                };

                orderItems.Add(orderItem);
            }
            _orderItemRepository.CreateRange(orderItems);
            _orderRepository.Commit();

            return Redirect(session.Url);

        }



        public IActionResult Delete(int MovieId)
        {
            var user = _userManager.GetUserId(User);
            var cart = _cartRepository.GetOne(e => e.ApplicationUserId == user && e.MovieId == MovieId);

            if (cart != null && user != null)
            {

                _cartRepository.Delete(cart);
                _cartRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFoundPage");

        }




        public IActionResult NotFoundPage()
        {
            return View();
        }

    }
}
