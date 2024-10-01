using GeneralEntries.RepositoryLayer.InterfaceClass;
using NLog;
using RabbitMQ.Client;
using System.Text;

namespace GeneralEntries.RepositoryLayer.ServiceClass;

public class RabbitMqMessageService : IRabbitMqMessageService
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public async Task PublishMessage(string queueConnStr, string queueName, List<Tuple<Dictionary<string, object>, string>> messages)
    {
        await Task.Run(() =>
        {
            try
            {
                var factory = new ConnectionFactory() { Uri = new Uri($"{queueConnStr}") };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    foreach (var message in messages)
                    {
                        PublishMessage(channel, connection, queueName, message.Item1, message.Item2);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"RabbitMqMessageService::PublishMessage error: {ex.Message}");
            }
        });
    }

    public async Task PublishMessage(string queueConnStr, string queueName, Dictionary<string, object> header, string message)
    {
        await Task.Run(() =>
        {
            try
            {
                _logger.Info($"RabbitMqMessageService::PublishMessage info: {header} | {message}");
                var factory = new ConnectionFactory() { Uri = new Uri($"{queueConnStr}") };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    PublishMessage(channel, connection, queueName, header, message);
                    _logger.Info($"RabbitMqMessageService::PublishMessage done: {header} | {message}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"RabbitMqMessageService::PublishMessage error: {ex.Message} | {message}");
            }
        });
    }

    private void PublishMessage(IModel channel, IConnection connection, string queueName, Dictionary<string, object> header, string message)
    {
        try
        {
            var model = connection.CreateModel();
            var properties = model.CreateBasicProperties();
            properties.Persistent = false;
            properties.Headers = header;

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
        }
        catch (Exception ex)
        {
            _logger.Error($"RabbitMqMessageService::PublishMessage error: {ex.Message} | {message}");
        }
    }
}
