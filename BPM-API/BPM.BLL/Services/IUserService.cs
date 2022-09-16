using BPM.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BLL.Services
{
    public interface IUserService
    {
        UserDataModel GetUserData();

        Task<bool> CheckEmailAvailability(string email);
    }
}
