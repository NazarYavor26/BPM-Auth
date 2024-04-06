using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BPM.DAL;

namespace BPM.BLL
{
    public class BLLModule
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            DALModule.Load(services, configuration);
        }
    }
}