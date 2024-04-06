using BPM_Auth.BLL.Models.User;

namespace BPM_Auth.BLL.Services
{
    public interface IUserService
    {
        UserDataModel GetUserData();

        Task<bool> CheckEmailAvailability(string email);
    }
}
