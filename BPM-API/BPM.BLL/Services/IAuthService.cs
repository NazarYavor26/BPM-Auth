using BPM.BLL.Models.User;
using Microsoft.AspNetCore.Http;

namespace BPM.BLL.Services
{
    public interface IAuthService
    {
        string Register(UserRegisterModel userRegisterModel);

        string Login(UserLoginModel userLoginModel, HttpResponse response);

        string RefreshToken(HttpRequest request, HttpResponse response);
    }
}
