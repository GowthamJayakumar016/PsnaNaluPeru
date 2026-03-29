using Happy.DTOs.Admin;

namespace Happy.Services.Interfaces.Admin
{
    public interface IAdminRoomService
    {
        Task<List<AdminRoomDto>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
