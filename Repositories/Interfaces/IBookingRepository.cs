using Happy.Models;

namespace Happy.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId);


        Task AddBookingAsync(Booking booking);

        Task<List<Booking>> GetBookingsByUserIdAsync(int userId);

        Task<Booking> GetByIdAsync(int id);

        Task UpdateAsync(Booking booking);
    }


}
