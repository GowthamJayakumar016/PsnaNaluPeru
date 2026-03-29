using Microsoft.AspNetCore.Mvc;
using Happy.Services.Interfaces.Admin;

namespace Happy.Controllers.Admin
{
    public class AdminBookingController : Controller
    {
        private readonly IAdminBookingService _service;


    public AdminBookingController(IAdminBookingService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId"));

            var bookings = await _service.GetBookingsByHotelIdAsync(hotelId);
            return View(bookings);
        }

        public async Task<IActionResult> Approve(int id)
        {
            await _service.ApproveBookingAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            await _service.RejectBookingAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout(int id)
        {
            await _service.CompleteBookingAsync(id);
            return RedirectToAction("Index");
        }
    }


}
