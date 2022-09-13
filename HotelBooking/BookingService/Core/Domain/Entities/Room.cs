namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public bool InMaintenance { get; set; }
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
    }
}
