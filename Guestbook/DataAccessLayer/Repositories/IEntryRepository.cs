using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IEntryRepository : IRepository<Entry>
    {
        Task<IEnumerable<Entry>> GetAll();
        Task<IEnumerable<Entry>> GetAll(string? searchString, int? pageNumber, int? pageSize);
        Task<int> Count(string? searchString);
    }
}
