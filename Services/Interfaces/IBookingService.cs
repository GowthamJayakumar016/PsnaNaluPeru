using Happy.DTOs.Booking;
using Happy.DTOs.Payment;

namespace Happy.Services.Interfaces
{
    public interface IBookingService
    {
        // Step 1 (user): booking details -> create a PendingPayment draft.
        Task<CreateBookingDraftResultDto> CreateBookingDraftAsync(CreateBookingDto dto, int userId);

        // Step 2 (user): payment checkout.
        Task<PaymentCheckoutViewDto?> GetPaymentCheckoutAsync(int bookingId, int userId);
        Task<ConfirmPaymentResultDto> ConfirmPaymentAsync(int bookingId, int userId, PaymentConfirmDto dto);

        Task<List<BookingViewDto>> GetUserBookingsAsync(int userId);

        // Allowed only inside the 2-hour window set after payment success.
        Task<bool> CancelBookingAsync(int bookingId, int userId);

        Task<RebookPrefillDto?> GetRebookPrefillAsync(int bookingId, int userId);
    }


}
