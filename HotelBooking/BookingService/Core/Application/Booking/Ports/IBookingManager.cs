using Application.Booking.DTO;
using Application.Booking.Responses;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(BookingDTO booking);
        Task<BookingDTO> GetBooking(int bookingId);

    }
}
