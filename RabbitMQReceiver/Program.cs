using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using MassTransit;
using RabbitMQReceiver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MyMessageConsumer>(); // Register the consumer

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost"); // Use your RabbitMQ host here

        cfg.ReceiveEndpoint("testQueue1", e =>
        {
            e.ConfigureConsumer<MyMessageConsumer>(context); // Configure the consumer
        });
    });
});

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a queue
channel.QueueDeclare(queue: "testQueue2",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
 arguments: null);

// Event-based consumer to receive messages from RabbitMQ
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);
};

// Consume the queue
channel.BasicConsume(queue: "testQueue2",
                     autoAck: true,
consumer: consumer);

var app = builder.Build();


app.MapGet("/", () => "Receiver is running...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
