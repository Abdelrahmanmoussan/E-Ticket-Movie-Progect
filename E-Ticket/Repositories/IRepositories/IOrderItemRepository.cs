using E_Ticket.Models;

namespace E_Ticket.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public void CreateRange(IEnumerable<OrderItem> orderItems);
    }
}
