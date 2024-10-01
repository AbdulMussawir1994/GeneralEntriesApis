using MassTransit;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost"); // Use your RabbitMQ host here
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

var app = builder.Build();

// Sample API endpoint to send messages
app.MapPost("/send", (string message) =>
{
    var body = Encoding.UTF8.GetBytes(message);

    // Publish the message to the queue
    channel.BasicPublish(exchange: "",
                         routingKey: "testQueue2",
                         basicProperties: null,
                         body: body);

    return Results.Ok("Message sent to RabbitMQ: " + message);
});

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
