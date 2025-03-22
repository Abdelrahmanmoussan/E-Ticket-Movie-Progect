using E_Ticket.Models;

namespace E_Ticket.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public Movie Movie { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public int MovieId { get; set; }
    }
}
