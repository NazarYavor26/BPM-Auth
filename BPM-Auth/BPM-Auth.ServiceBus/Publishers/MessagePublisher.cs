using RabbitMQ.Client;
using System.Text;

namespace BPM_Auth.ServiceBus.Publishers
{
    public class MessagePublisher : IMessagePublisher
    {
        private string _hostName;

        public void Initialize(string hostName)
        {
            _hostName = hostName;
        }

        public void Publish(string queueName, string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
