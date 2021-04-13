using DataAccessLayer.Context;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GuestbookContext _db;

        public UnitOfWork(GuestbookContext db)
        {
            _db = db;
            Entries = new EntryRepository(_db);
            ApplicationUsers = new ApplicationUserRepository(_db);
        }

        public IEntryRepository Entries { get; }
        public IApplicationUserRepository ApplicationUsers { get; }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
