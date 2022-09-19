using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {

        private HotelDbContext _hotelDbContext;
        public BookingRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }
        public async Task<Domain.Entities.Booking> Create(Domain.Entities.Booking booking)
        {
            _hotelDbContext.Bookings.Add(booking);
            await _hotelDbContext.SaveChangesAsync();
            return booking;
        }

        Task<Domain.Entities.Booking?> IBookingRepository.Get(int Id)
        {
            return _hotelDbContext.Bookings.Where(g => g.Id == Id).FirstAsync();
        }
    }
}