using BPM.BLL.Models;
using Microsoft.AspNetCore.Http;

namespace BPM.BLL.Providers
{
    public interface IServerContextProvider
    {
        public UserDataModel UserDataModel { get; set; }

        void LoadData(HttpContext httpContext);
    }
}
