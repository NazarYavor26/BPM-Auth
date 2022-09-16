using BPM.BLL.Models;
using BPM.DAL.DbContexts;
using BPM.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BLL.Providers
{
    internal class ServerContextProvider : IServerContextProvider
    {
        public UserDataModel UserDataModel { get; set; }
        private readonly AppDbContext _db;
        public ServerContextProvider(
            AppDbContext db,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            LoadData(httpContextAccessor.HttpContext);
        }

        public void LoadData(HttpContext httpContext)
        {
            LoadUserData(httpContext);
        }

        private void LoadUserData(HttpContext httpContext)
        {
            var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(id))
            {
                User UserTest = _db.Users.FirstOrDefault(x => x.Id == int.Parse(id));
                UserProfile userProfileTest = _db.UserProfiles.FirstOrDefault(x => x.Id == int.Parse(id));

                UserDataModel.Id = userProfileTest.Id;
                UserDataModel.FirstName = userProfileTest.FirstName;
                UserDataModel.LastName = userProfileTest.LastName;
                UserDataModel.Email = UserTest.Email;
                UserDataModel.RoleId = UserTest.RoleId;
                UserDataModel.StatusId = UserTest.StatusId;
                UserDataModel.LevelId = userProfileTest.LevelId;
            }
        }
    }
}
