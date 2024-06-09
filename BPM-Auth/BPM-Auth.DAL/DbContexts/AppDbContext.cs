using BPM_Auth.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM_Auth.DAL.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
    }
}
