using Domain.Booking.DomainExceptions;
using Domain.Enums;
using Domain.Guest.DomainExceptions;
using Domain.Ports;
using Action = Domain.Enums.Action;

namespace Domain.Entities
{
    public class Booking
    {
        public Booking()
        {
            this.Status = Status.Created;
            this.PlaceAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public DateTime PlaceAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Room Room { get; set; }
        public Guest Guest { get; set; }

        private Status Status { get; set; }

        public Status CurrentStatus { get { return this.Status; } }

        public void ChangeState(Action action)
        {
            this.Status = (this.Status, action) switch
            {
                (Status.Created,  Action.Pay) => Status.Paid,
                (Status.Created,  Action.Cancel) => Status.Canceled,
                (Status.Paid,     Action.Finish) => Status.Finished,
                (Status.Paid,     Action.Refound) => Status.Refounded,
                (Status.Canceled, Action.Reopen) => Status.Created,
                _ => this.Status
            };
        }

        private void ValidateState()
        {
            if(this.PlaceAt == default(DateTime))
            {
                throw new PlacedAtIsARequiredInformationException();
            }

            if (this.Start == default(DateTime))
            {
                throw new StartDateTimeIsRequiredException();
            }

            if (this.End == default(DateTime))
            {
                throw new EndDateTimeIsRequiredException();
            }

            if (this.Room == null)
            {
                throw new RomIsRequiredException();
            }

            if (this.Guest == null)
            {
                throw new GuestIsRequiredException();
            }

            this.Guest.IsValid();    
        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                var resp = await bookingRepository.Create(this);
                this.Id = resp.Id;
            }
            else
            {
                //await guestRepository.Update(this);
            }
        }

    }
}
