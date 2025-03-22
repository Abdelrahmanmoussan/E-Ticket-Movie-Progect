using E_Ticket.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Ticket.Areas.Customers.Controllers
{
        [Area("Customers")]
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Success(int orderId)
        {
            var order = _orderRepository.GetOne(e => e.Id == orderId);

            if (order != null)
            {
                var service = new SessionService();
                var session = service.Get(order.SessionId);

                order.PaymentStripeId = session.PaymentIntentId;
                order.Status = true;
                order.PaymentStatus = true;

                _orderRepository.Commit();
            }

            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}
