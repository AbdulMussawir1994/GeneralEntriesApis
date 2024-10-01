using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralEntries.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IMessageProducer _messageProducer;

        public static readonly List<Booking> _bookings = new();

        public BookingController(ILogger<BookingController> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public IActionResult CreatingBooking(Booking booking)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            _bookings.Add(booking);
            _messageProducer.SendingMessage<Booking>(booking);

            return Ok();
        }

    }
}
