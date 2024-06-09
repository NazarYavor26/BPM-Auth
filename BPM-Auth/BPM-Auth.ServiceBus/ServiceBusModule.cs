using BPM_Auth.ServiceBus.Publishers;
using BPM_Auth.ServiceBus.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BPM_Auth.ServiceBus
{
    public static class ServiceBusModule
    {
        public static void InitializeServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMessagePublisher, MessagePublisher>();
            services.AddTransient<IPublisherService, PublisherService>();
        }
    }
}
