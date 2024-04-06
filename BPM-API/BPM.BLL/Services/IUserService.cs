using BPM.BLL.Models.User;

namespace BPM.BLL.Services
{
    public interface IUserService
    {
        UserDataModel GetUserData();

        Task<bool> CheckEmailAvailability(string email);
    }
}
