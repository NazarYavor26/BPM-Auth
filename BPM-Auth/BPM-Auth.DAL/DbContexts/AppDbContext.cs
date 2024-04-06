using BPM_Auth.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM_Auth.DAL.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
    }
}
