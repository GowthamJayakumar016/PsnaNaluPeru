using Happy.Models;

namespace Happy.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task AddBookingAsync(Booking booking);
        Task<List<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId);
        Task<Booking> GetBookingByIdAsync(int id);
        Task SaveAsync();
    }
}
