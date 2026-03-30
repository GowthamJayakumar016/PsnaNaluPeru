using Happy.DTOs.Admin;
using Happy.Repositories.Interfaces.Admin;
using Happy.Services.Interfaces.Admin;

namespace Happy.Services.Implementations.Admin
{
    public class AdminRoomService : IAdminRoomService
    {
        private readonly IAdminRoomRepository _repo;


    public AdminRoomService(IAdminRoomRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AdminRoomDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var rooms = await _repo.GetRoomsByHotelIdAsync(hotelId);

            var list = new List<AdminRoomDto>();

            foreach (var r in rooms)
            {
                list.Add(new AdminRoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    Amenities = r.Amenities,
                    Price = r.Price,
                    Capacity = r.Capacity,
                    IsActive = r.IsActive
                });
            }

            return list;
        }
    }


}
