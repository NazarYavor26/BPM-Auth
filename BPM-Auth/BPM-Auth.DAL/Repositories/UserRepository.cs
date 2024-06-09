using BPM_Auth.DAL.DbContexts;
using BPM_Auth.DAL.Entities;

namespace BPM_Auth.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Add(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User? GetById(Guid userId)
        {
            return _db.Users.FirstOrDefault(x => x.UserId == userId);
        }

        public User? GetByEmail(string email)
        {
            return _db.Users.FirstOrDefault(x => x.Email == email);
        }
    }
}
