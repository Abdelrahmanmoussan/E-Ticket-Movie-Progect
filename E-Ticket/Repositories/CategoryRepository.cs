using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;

namespace E_Ticket.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) 
        {
            this.dbContext = dbContext;
        }
    }
}
