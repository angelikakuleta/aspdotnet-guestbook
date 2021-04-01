using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<Entry>> GetAll(string? searchString, int? pageNumber, int? pageSize)
        {
            IQueryable<Entry> query = _db.Entries.OrderByDescending(x => x.EntryTime);

            if (searchString != null)
            {
                query = query.Where(x => x.Comment.Contains(searchString));
            }

            if (pageNumber != null && pageSize != null)
            {
                query = query.Skip(((int)pageNumber - 1) * (int)pageSize).Take((int)pageSize);
            }

            return await query  
                .AsNoTracking().
                ToListAsync();
        }

        public async Task<int> Count(string? searchString)
        {
            IQueryable<Entry> query = _db.Entries;

            if (searchString != null)
            {
                query = query.Where(x => x.Comment.Contains(searchString));
            }

            return await query.CountAsync();
        }
    }
}
