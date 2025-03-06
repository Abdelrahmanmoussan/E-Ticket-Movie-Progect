using E_Ticket.Models;

namespace E_Ticket.Models
{
    public class ActorMovies
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
