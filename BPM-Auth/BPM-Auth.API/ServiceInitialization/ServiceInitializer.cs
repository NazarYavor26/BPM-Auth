using BPM_Auth.BLL;
using BPM_Auth.DAL;
using BPM_Auth.ServiceBus;

namespace BPM_Auth.API.ServiceInitialization
{
    public static class ServiceInitializer
    {
        public static void InitializeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.InitializeBLL(configuration);
            services.InitializeDAL(configuration);
            services.InitializeServiceBus(configuration);
        }
    }
}
