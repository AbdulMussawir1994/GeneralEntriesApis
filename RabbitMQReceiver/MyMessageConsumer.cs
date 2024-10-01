using MassTransit;

namespace RabbitMQReceiver
{
    public class MyMessageConsumer : IConsumer<MyMessage>
    {
        public Task Consume(ConsumeContext<MyMessage> context)
        {
            // Handle the received message
            Console.WriteLine("Received message: " + context.Message.Text);
            return Task.CompletedTask;
        }
    }

    public class MyMessage
    {
        public string Text { get; set; }
    }

    public class Message
    {
        public string Text { get; set; }
    }
}
