using Microsoft.EntityFrameworkCore;

namespace E_Ticket.Models
{
    namespace E_Ticket.Models
    {
        [PrimaryKey(nameof(MovieId), nameof(ApplicationUserId))]
        public class FavItems
        {
            public int MovieId { get; set; }
            public Movie Movie { get; set; }

            public string ApplicationUserId { get; set; }
            public ApplicationUser ApplicationUser { get; set; }


        }
    }
}
