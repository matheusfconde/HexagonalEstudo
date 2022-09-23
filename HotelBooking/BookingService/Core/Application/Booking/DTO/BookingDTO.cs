using Application.Guest.DTO;
using Application.Room.DTO;
using Domain.Enums;
using Entities = Domain.Entities;


namespace Application.Booking.DTO
{
    public class BookingDTO
    {
        public BookingDTO()
        {
            this.PlaceAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime PlaceAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }

        private Status Status { get; set; }

        public static Entities.Booking MapToEntity(BookingDTO bookingDTO)
        {
            return new Entities.Booking
            {
                Id = bookingDTO.Id,
                Start = bookingDTO.Start,
                Guest = new Entities.Guest { Id = bookingDTO.GuestId },
                Room = new Entities.Room { Id = bookingDTO.RoomId },
                End = bookingDTO.End,
                PlaceAt = bookingDTO.PlaceAt,                                               
            };
        }
    }
}
