using Application.Room.DTO;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Room.Responses;
using Application.Responses;
using Domain.DomainExceptions;
using Domain.Ports;

namespace Application
{
    public class RoomManager : IRoomManager
    {
        private IRoomRepository _roomRepository;
        public RoomManager(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public async Task<RoomResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                var room = RoomDTO.MapToEntity(request.Data);

                await room.Save(_roomRepository);

                request.Data.Id = room.Id;

                return new RoomResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (InvalidRoomDataException e)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (Exception)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
        public async Task<RoomResponse> GetRoom(int roomId)
        {
            var room = await _roomRepository.Get(roomId);

            if (room == null)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_NOT_FOUND,
                    Message = "No Rooms Record was found with the given Id"
                };
            }

            return new RoomResponse
            {
                Data = RoomDTO.MapToDto(room),
                Success = true
            };
        }
    }
}
