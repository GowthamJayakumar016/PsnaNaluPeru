using Happy.DTOs.Room;
using Happy.Repositories.Interfaces;
using Happy.Services.Interfaces;

namespace Happy.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;
        private readonly IBookingRepository _bookingRepo;

        public RoomService(IRoomRepository repo, IBookingRepository bookingRepo)
        {
            _repo = repo;
            _bookingRepo = bookingRepo;
        }

        public async Task<List<RoomViewDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var rooms = await _repo.GetRoomsByHotelIdAsync(hotelId);
            var roomIds = rooms.Select(r => r.Id).ToList();
            var occupiedNow = await _bookingRepo.GetRoomIdsWithActiveConfirmedStayAsync(roomIds, DateTime.Now);

            var list = new List<RoomViewDto>();

            foreach (var r in rooms)
            {
                list.Add(new RoomViewDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    Amenities = r.Amenities,
                    Price = r.Price,
                    Capacity = r.Capacity,
                    IsOccupiedNow = occupiedNow.Contains(r.Id)
                });
            }

            return list;
        }
    }
}

