using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class GuestbookContext : IdentityDbContext
    {
        public GuestbookContext(DbContextOptions<GuestbookContext> options) : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entry>().ToTable("Entry");

            modelBuilder.Entity<Entry>()
                .Property(t => t.EntryTime)
                .HasDefaultValueSql("GetDate()");
        }
    }
}
