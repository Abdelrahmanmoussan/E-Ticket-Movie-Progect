using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;

namespace E_Ticket.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateRange(IEnumerable<OrderItem> orderItems)
        {
            dbContext.AddRange(orderItems);
        }
    }
}
