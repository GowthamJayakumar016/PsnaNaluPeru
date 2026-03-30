using Happy.DTOs.Admin;

namespace Happy.Services.Interfaces.Admin
{
    public interface IAdminBookingService
    {
        Task<List<AdminBookingDto>> GetBookingsByHotelIdAsync(int hotelId);
        Task ApproveBookingAsync(int bookingId, int hotelId);
        Task RejectBookingAsync(int bookingId, int hotelId);
        Task CompleteBookingAsync(int bookingId, int hotelId);
    }
}
