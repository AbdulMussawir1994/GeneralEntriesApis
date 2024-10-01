using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQSender : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMQSender(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            // Publish the message to RabbitMQ
            await _publishEndpoint.Publish(message);
            return Ok($"Message sent to RabbitMQ: {message.Text}");
        }
    }
}
