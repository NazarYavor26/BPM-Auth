using BPM_Auth.DAL.Entities;

namespace BPM_Auth.DAL.Repositories
{
    public interface IUserRepository
    {
        void SaveChanges();

        void Add(User user);

        User? GetById(Guid userId);

        User? GetByEmail(string email);
    }
}
