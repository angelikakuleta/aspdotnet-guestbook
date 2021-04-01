using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Context
{
    public class GuestbookContext : DbContext
    {
        public GuestbookContext(DbContextOptions<GuestbookContext> options) : base(options)
        {

        }

        public DbSet<Entry> Entries { get; set; }
    }
}
