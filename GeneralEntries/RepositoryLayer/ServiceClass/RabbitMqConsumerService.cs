using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace GeneralEntries.RepositoryLayer.ServiceClass
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _emailChannel;
        private readonly IModel _smsChannel;

        public RabbitMqConsumerService(IConfiguration configuration)
        {
            var connectionString = configuration["RabbitMq:ConnectionString"];
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString),
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = true
            };

            _connection = connectionFactory.CreateConnection("DemoApp");

            // Email Channel
            _emailChannel = _connection.CreateModel();
            _emailChannel.QueueDeclare("Email", true, false, false);
            _emailChannel.BasicQos(0, 1, false);

            // Sms Channel
            _smsChannel = _connection.CreateModel();
            _smsChannel.QueueDeclare("Sms", true, false, false);
            _smsChannel.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => ConsumeEmailQueue(stoppingToken));
            Task.Run(() => ConsumeSmsQueue(stoppingToken));
            return Task.CompletedTask;
        }

        private void ConsumeEmailQueue(CancellationToken stoppingToken)
        {
            var emailConsumer = new AsyncEventingBasicConsumer(_emailChannel);
            emailConsumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Received Email: {message}");

                // Acknowledge
                _emailChannel.BasicAck(ea.DeliveryTag, false);
            };

            _emailChannel.BasicConsume("Email", false, emailConsumer);
        }

        private void ConsumeSmsQueue(CancellationToken stoppingToken)
        {
            var smsConsumer = new AsyncEventingBasicConsumer(_smsChannel);
            smsConsumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Received SMS: {message}");

                // Acknowledge
                _smsChannel.BasicAck(ea.DeliveryTag, false);
            };

            _smsChannel.BasicConsume("Sms", false, smsConsumer);
        }

        public override void Dispose()
        {
            _emailChannel?.Dispose();
            _smsChannel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
