using Domain.DomainExceptions;
using Domain.Ports;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public bool IsAvailable
        {
            get
            {
                if (this.InMaintenance || !this.HasGuest)
                {
                    return false;
                }

                return true;
            }
        }
        public bool HasGuest
        {
            //verificar se existem Bookings abertos para esta Room
            get { return true; }
        }

        public void ValidateState()
        {
            if(String.IsNullOrEmpty(this.Name))
            {
                throw new InvalidRoomDataException();
            }
        }

        public async Task Save(IRoomRepository roomRepository)
        {
            this.ValidateState();

            if(this.Id == 0)
            {
                this.Id = await roomRepository.Create(this);
            }
        }
    }
}
