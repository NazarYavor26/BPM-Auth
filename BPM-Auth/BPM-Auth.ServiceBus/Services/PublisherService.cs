using BPM_Auth.ServiceBus.Models;
using BPM_Auth.ServiceBus.Publishers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BPM_Auth.ServiceBus.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IConfiguration _configuration;
        private readonly IMessagePublisher _messagePublisher;

        public PublisherService(
            IConfiguration configuration,
            IMessagePublisher messagePublisher)
        {
            _configuration = configuration;
            _messagePublisher = messagePublisher;

            var hostName = _configuration["RabbitMQ:HostName"];
            _messagePublisher.Initialize(hostName);
        }

        public void PublishAdminUserToBpmCore(BpmCoreUserModel adminUserModel)
        {
            var adminToBpmCoreQueue = _configuration["RabbitMQ:RegisterAdminToBpmCoreQueue"];
            var adminUserModelJson = JsonConvert.SerializeObject(adminUserModel);

            _messagePublisher.Publish(adminToBpmCoreQueue, adminUserModelJson);
        }

        public void PublishMemberUserToBpmCore(BpmCoreUserModel memberUserModel)
        {
            var memberToBpmCoreQueue = _configuration["RabbitMQ:RegisterMemberToBpmCoreQueue"];
            var memberUserModelJson = JsonConvert.SerializeObject(memberUserModel);

            _messagePublisher.Publish(memberToBpmCoreQueue, memberUserModelJson);
        }
    }
}
