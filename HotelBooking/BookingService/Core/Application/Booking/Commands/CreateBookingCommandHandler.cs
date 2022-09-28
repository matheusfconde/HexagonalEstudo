using Application.Booking.Responses;
using MediatR;
using Application.Booking.Commands;
using Application.Booking.Ports;

namespace Application.Booking.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
    {
        private IBookingManager _bookingManager;

        public CreateBookingCommandHandler(IBookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        public Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            //Se fosse uma aplicação que eu começasse do 0, eu não criaria no BookingManager o comando CreateBooking, eu teria criado ele dentro do Handler do command, mas como já criamos  no manager, irei injetar a interface IBookManager.
            //Esse não é o correto, o correto seria implementar o CreateBooking aqui.
            return _bookingManager.CreateBooking(request.BookingDTO);
        }
    }
}
