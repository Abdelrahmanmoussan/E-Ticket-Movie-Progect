using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Ticket.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string FName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string LName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Address { get; set; }
        public int Age { get; set; }
    }
}
