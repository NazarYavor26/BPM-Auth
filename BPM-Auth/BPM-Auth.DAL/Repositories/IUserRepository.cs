using BPM_Auth.DAL.Entities;

namespace BPM_Auth.DAL.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        User? GetById(Guid userId);
    }
}
