using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQReceiver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQReceiver : ControllerBase
    {
        // This endpoint is just for testing if the service is running.
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok("Receiver is running...");
        }
    }
}
