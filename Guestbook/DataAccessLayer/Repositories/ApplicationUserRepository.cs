using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(GuestbookContext db) : base(db)
        {
        }
    }
}
