using Happy.Models;

namespace Happy.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId);


        Task AddBookingAsync(Booking booking);

        Task<List<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId);
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<Booking?> GetBookingByIdWithRoomAndUserAsync(int id);
        Task<HashSet<int>> GetRoomIdsWithActiveConfirmedStayAsync(IReadOnlyList<int> roomIds, DateTime at);
        Task SaveAsync();
    }


}
