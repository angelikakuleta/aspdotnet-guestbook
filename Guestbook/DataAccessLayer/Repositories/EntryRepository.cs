using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class EntryRepository : Repository<Entry>, IEntryRepository
    {
        private readonly GuestbookContext _db;

        public EntryRepository(GuestbookContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Entry>> GetAll()
        {
            return await base.GetAll(orderBy: e => e.OrderByDescending(x => x.EntryTime));
        }

        public async Task<IEnumerable<Entry>> GetAllWithPhrase(string phrase)
        {
            return await base.GetAll(
                filter:x => x.Comment.Contains(phrase), 
                orderBy:e => e.OrderByDescending(x => x.EntryTime));
        }
    }
}
