using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Models.E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;

namespace E_Ticket.Repositories
{
    public class FavItemRepository : Repository<FavItems>, IFavItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public FavItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
    
    
}
