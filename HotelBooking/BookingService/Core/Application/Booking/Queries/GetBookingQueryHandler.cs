using Application.Booking.DTO;
using Application.Booking.Responses;
using Domain.Ports;
using MediatR;

namespace Application.Booking.Queries
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            //Se fosse uma aplicação que eu começasse do 0, eu não criaria no BookingManager o comando CreateBooking, eu teria criado ele dentro do Handler da query, mas como já criamos  no manager, irei injetar a interface IBookManager.
            //Esse não é o correto, o correto seria implementar o CreateBooking aqui.
            var booking =  await _bookingRepository.Get(request.Id);

            var bookingDTO = BookingDTO.MapToDto(booking);

            return new BookingResponse
            {
                Success = true,
                Data = bookingDTO,
            };            
        }
    }
}
