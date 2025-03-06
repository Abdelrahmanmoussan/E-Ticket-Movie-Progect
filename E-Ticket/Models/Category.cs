namespace E_Ticket.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
