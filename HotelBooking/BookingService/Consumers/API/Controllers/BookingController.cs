using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Payment.Responses;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Booking.Commands;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingManager _bookingManager;
        private readonly IMediator _mediator;

        public BookingController(ILogger<BookingController> logger, IBookingManager bookingManager, IMediator mediator)
        {
            _logger = logger;
            _bookingManager = bookingManager;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("{bookingId}/Pay")]
        public async Task<ActionResult<PaymentResponse>> Pay(PaymentRequestDto paymentRequestDto, int bookingId)
        {
            paymentRequestDto.BookingId = bookingId;

            var res = await _bookingManager.PayForABooking(paymentRequestDto);

            if (res.Success) return Ok(res.Data);

            return BadRequest(res);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDTO>> Post(BookingDTO booking)
        {
            var command = new CreateBookingCommand
            {
                BookingDTO = booking,
            };

            //var res = await _bookingManager.CreateBooking(booking); // utilizava a porta

            var res = await _mediator.Send(command); //agora utilizando o mediatR;

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION)
            {
                //res.Message = "Eu quis trocar a mensagem para dar exemplo";
                return BadRequest(res);
            }

            else if (res.ErrorCode == ErrorCodes.BOOKING_COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            else if (res.ErrorCode == ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED)

                _logger.LogError("Response with unknown ErrorCode Returned", res);

            return BadRequest(500);

        }
    }
}
