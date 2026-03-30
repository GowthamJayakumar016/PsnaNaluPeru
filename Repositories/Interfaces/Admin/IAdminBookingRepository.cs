using Happy.Models;

namespace Happy.Repositories.Interfaces.Admin
{
    public interface IAdminBookingRepository
    {
        Task<List<Booking>> GetBookingsByHotelIdAsync(int hotelId);
        Task<Booking?> GetBookingByIdWithRoomAsync(int id);
        Task SaveAsync();
    }
}
