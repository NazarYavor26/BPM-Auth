using BPM.BLL.Models;
using BPM.BLL.Providers;
using BPM.DAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BLL.Services
{
    public class UserService : IUserService
    {
        public readonly AppDbContext _db;
        public readonly IServerContextProvider _serverContext;

        public UserService(
            AppDbContext db,
            IServerContextProvider serverContext)
        {
            _db = db;
            _serverContext = serverContext;
        }

        public UserDataModel GetUserData()
        {
            var user = _serverContext.UserDataModel;

            if (user == null)
            {
                //Create class AppException
                throw new Exception();
            }

            return user;
        }

        public Task<bool> CheckEmailAvailability(string email)
        {
            throw new NotImplementedException();
        }
    }
}
