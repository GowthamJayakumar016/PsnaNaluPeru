using Happy.Filters;
using Happy.Services.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Happy.Controllers.Admin
{
    [RequireAdmin]
    public class AdminDashboardController : Controller
    {
        private readonly IAdminBookingService _service;


    public AdminDashboardController(IAdminBookingService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Dashboard()
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId"));

            var bookings = await _service.GetBookingsByHotelIdAsync(hotelId);
            return View(bookings);
        }
    }


}
