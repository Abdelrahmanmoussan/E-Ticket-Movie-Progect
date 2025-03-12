using Microsoft.AspNetCore.Identity;

namespace E_Ticket.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public int Age { get; set; }
    }
}
