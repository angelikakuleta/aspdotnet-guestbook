using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IUnitOfWork
    {
        IEntryRepository Entries { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        Task Save();
    }
}
