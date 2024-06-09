namespace BPM_Auth.ServiceBus.Publishers
{
    public interface IMessagePublisher
    {
        void Initialize(string hostName);

        void Publish(string queueName, string message);
    }
}
