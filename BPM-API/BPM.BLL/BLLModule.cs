using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BPM.DAL;
using BPM.BLL.Services;

namespace BPM.BLL
{
    public class BLLModule
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthService, AuthService>();
            DALModule.Load(services, configuration);
        }
    }
}