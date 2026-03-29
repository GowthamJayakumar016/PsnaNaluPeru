using Happy.Models;

namespace Happy.Repositories.Interfaces.Admin
{
    public interface IAdminBookingRepository
    {
        Task<List<Booking>> GetBookingsByHotelIdAsync(int hotelId);
        Task<Booking> GetBookingByIdAsync(int id);
        Task SaveAsync();
    }
}
