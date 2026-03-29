using Happy.DTOs.Room;
using Happy.Repositories.Interfaces;
using Happy.Services.Interfaces;

namespace Happy.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;

    public RoomService(IRoomRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RoomViewDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var rooms = await _repo.GetRoomsByHotelIdAsync(hotelId);

            var list = new List<RoomViewDto>();

            foreach (var r in rooms)
            {
                list.Add(new RoomViewDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    Price = r.Price,
                    Capacity = r.Capacity
                });
            }

            return list;
        }
    }


}
