using BPM_Auth.BLL.Models.User;
using BPM_Auth.DAL.DbContexts;

namespace BPM_Auth.BLL.Services
{
    public class UserService : IUserService
    {
        public readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public UserDataModel GetUserData()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckEmailAvailability(string email)
        {
            throw new NotImplementedException();
        }
    }
}
