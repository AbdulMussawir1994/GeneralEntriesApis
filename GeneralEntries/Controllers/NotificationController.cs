using GeneralEntries.RepositoryLayer.ServiceClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralEntries.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly RabbitMqService _rabbitMqService;

        public NotificationController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost("publish-email")]
        public IActionResult PublishEmail([FromBody] string message)
        {
            _rabbitMqService.PublishMessage("email", message);
            return Ok("Email message published.");
        }

        [HttpPost("publish-sms")]
        public IActionResult PublishSms([FromBody] string message)
        {
            _rabbitMqService.PublishMessage("sms", message);
            return Ok("SMS message published.");
        }
    }
}
