using Happy.Models;

namespace Happy.Repositories.Interfaces.Admin
{
    public interface IAdminRoomRepository
    {
        Task<List<Room>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
