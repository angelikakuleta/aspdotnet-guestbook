using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IEntryRepository : IRepository<Entry>
    {
        Task<IEnumerable<Entry>> GetAll();
        Task<IEnumerable<Entry>> GetAllWithPhrase(string phrase);
    }
}
