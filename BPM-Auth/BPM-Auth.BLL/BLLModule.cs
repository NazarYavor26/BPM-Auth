using BPM_Auth.BLL.Services;
using BPM_Auth.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BPM_Auth.BLL
{
    public class BLLModule
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            DALModule.Load(services, configuration);
        }
    }
}