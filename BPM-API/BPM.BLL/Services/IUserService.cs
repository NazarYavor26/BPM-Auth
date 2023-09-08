using BPM.BLL.Models;

namespace BPM.BLL.Services
{
    public interface IUserService
    {
        UserDataModel GetUserData();

        Task<bool> CheckEmailAvailability(string email);
    }
}
