using Happy.DTOs.Room;

namespace Happy.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomViewDto>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
