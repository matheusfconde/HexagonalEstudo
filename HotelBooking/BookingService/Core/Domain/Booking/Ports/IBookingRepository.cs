using Domain.Entities;

namespace Domain.Ports
{
    public interface IBookingRepository
    {
        Task<Entities.Booking?> Get(int Id);
        Task<Entities.Booking> Create(Entities.Booking booking);
    }
}
