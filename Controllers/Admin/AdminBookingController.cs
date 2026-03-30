using Happy.Filters;
using Happy.Services.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Happy.Controllers.Admin
{
    [RequireAdmin]
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
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            await _service.ApproveBookingAsync(id, hotelId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            await _service.RejectBookingAsync(id, hotelId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout(int id)
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            await _service.CompleteBookingAsync(id, hotelId);
            return RedirectToAction("Index");
        }
    }


}
