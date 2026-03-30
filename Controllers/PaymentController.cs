using Happy.DTOs.Payment;
using Happy.Filters;
using Happy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Happy.Controllers
{
    [RequireUser]
    public class PaymentController : Controller
    {
        private readonly IBookingService _bookingService;

        public PaymentController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(int bookingId)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId")!);

            var checkout = await _bookingService.GetPaymentCheckoutAsync(bookingId, userId);
            if (checkout == null)
                return RedirectToAction("MyBookings", "Booking");

            return View(checkout);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int bookingId, PaymentConfirmDto dto)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId")!);

            var checkout = await _bookingService.GetPaymentCheckoutAsync(bookingId, userId);
            if (checkout == null)
                return RedirectToAction("MyBookings", "Booking");

            var result = await _bookingService.ConfirmPaymentAsync(bookingId, userId, dto);

            if (!result.Success)
            {
                checkout.Error = result.Error;
                checkout.DiscountAmount = result.DiscountAmount;
                checkout.TotalPrice = result.TotalPrice;
                checkout.CouponCode = dto.CouponCode;
                return View(checkout);
            }

            // Success: show the 2-hour cancellation message (deadline itself is stored in DB).
            TempData["BookingSuccessMessage"] = "Your booking is confirmed. You can cancel within 2 hours of payment. After that, contact the admin.";

            return RedirectToAction("UserDashboard", "Dashboard");
        }
    }
}

