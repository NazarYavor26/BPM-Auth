using BPM_Auth.BLL.Models;
using BPM_Auth.BLL.Models.User;
using Microsoft.AspNetCore.Http;

namespace BPM_Auth.BLL.Services
{
    public interface IAuthService
    {
        string RegisterAdmin(AdminRegisterModel adminRegisterModel);

        string RegisterMember(UserRegisterModel userRegisterModel);

        string RegisterTeam(TeamRegisterModel teamRegisterModel);

        string Login(UserLoginModel userLoginModel, HttpResponse response);

        string RefreshToken(HttpRequest request, HttpResponse response);
    }
}
