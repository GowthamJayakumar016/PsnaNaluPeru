using Happy.DTOs.Booking;

namespace Happy.Services.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CreateBookingAsync(CreateBookingDto dto, int userId);
        Task<List<BookingViewDto>> GetUserBookingsAsync(int userId);
        Task CancelBookingAsync(int bookingId);
    }
}
