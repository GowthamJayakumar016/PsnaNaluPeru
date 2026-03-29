using Happy.DTOs.Admin;

namespace Happy.Services.Interfaces.Admin
{
    public interface IAdminBookingService
    {
        Task<List<AdminBookingDto>> GetBookingsByHotelIdAsync(int hotelId);
        Task ApproveBookingAsync(int bookingId);
        Task RejectBookingAsync(int bookingId);
        Task CompleteBookingAsync(int bookingId);
    }
}
