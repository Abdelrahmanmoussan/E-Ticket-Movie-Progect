using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;

namespace E_Ticket.Repositories
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ActorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
