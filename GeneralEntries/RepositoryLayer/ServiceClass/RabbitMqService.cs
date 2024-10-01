using RabbitMQ.Client;
using System.Text;

namespace GeneralEntries.RepositoryLayer.ServiceClass
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(IConfiguration configuration)
        {
            var connectionString = configuration["RabbitMq:ConnectionString"];
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString),
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = true
            };

            _connection = connectionFactory.CreateConnection("DemoApp");
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("CustomerNotification", ExchangeType.Direct, true, false);
        }

        public void CreateQueues()
        {
            _channel.QueueDeclare("Email", true, false, false);
            _channel.QueueDeclare("Sms", true, false, false);
        }

        public void BindQueues()
        {
            _channel.QueueBind("Email", "CustomerNotification", "email");
            _channel.QueueBind("Sms", "CustomerNotification", "sms");
        }

        public void PublishMessage(string routingKey, string message)
        {
            var properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("CustomerNotification", routingKey, properties, body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
