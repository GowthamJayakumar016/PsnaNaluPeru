using Happy.Models;

namespace Happy.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetRoomsByHotelIdAsync(int hotelId);
        Task<Room> GetRoomByIdAsync(int id);
    }
}
