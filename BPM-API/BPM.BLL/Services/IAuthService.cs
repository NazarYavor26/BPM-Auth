using BPM.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BLL.Services
{
    public interface IAuthService
    {
        string Register(UserRegisterModel userRegisterModel);

        string Login(UserLoginModel userLoginModel);
    }
}
