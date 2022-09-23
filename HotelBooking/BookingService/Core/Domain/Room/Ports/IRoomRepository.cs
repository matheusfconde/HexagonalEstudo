using Domain.Entities;

namespace Domain.Ports
{
    public interface IRoomRepository
    {
        Task<Entities.Room?> Get(int Id);
        Task<Domain.Entities.Room?> GetAggreagate(int Id);
        Task<int> Create(Entities.Room room);
    }
}
