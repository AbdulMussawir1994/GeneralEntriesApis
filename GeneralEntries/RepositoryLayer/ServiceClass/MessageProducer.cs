using GeneralEntries.RepositoryLayer.InterfaceClass;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeneralEntries.RepositoryLayer.ServiceClass;

public class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/"
        };

        var con = factory.CreateConnection();

        using var channel = con.CreateModel();

        channel.QueueDeclare("booking", durable: true, exclusive: true);

        var jsonString = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(jsonString);

        channel.BasicPublish("", "bookings", body: body);

    }
}