namespace E_Ticket.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public string News { get; set; }

        public List<ActorMovies> ActorMovies { get; set; } = new List<ActorMovies>();

        public List<Movie> Movies { get; set; }
    }
}
