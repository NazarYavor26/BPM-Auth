using BPM_Auth.BLL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BPM_Auth.BLL
{
    public static class BLLModule
    {
        public static void InitializeBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthService, AuthService>();
        }
    }
}