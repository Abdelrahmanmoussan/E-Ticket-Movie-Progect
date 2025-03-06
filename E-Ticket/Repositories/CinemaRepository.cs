using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;
using System.Linq.Expressions;

namespace E_Ticket.Repositories
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}
