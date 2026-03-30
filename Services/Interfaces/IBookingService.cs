using Happy.DTOs.Booking;

namespace Happy.Services.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CheckAvailabilityAsync(CreateBookingDto dto);


    Task CreateBookingAsync(CreateBookingDto dto, int userId);

        Task<List<BookingViewDto>> GetUserBookingsAsync(int userId);

        Task CancelBookingAsync(int bookingId);
    }


}
