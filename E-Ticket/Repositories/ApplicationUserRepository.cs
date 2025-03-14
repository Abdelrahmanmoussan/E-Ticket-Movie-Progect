using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;

namespace E_Ticket.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
        }
    }
}
