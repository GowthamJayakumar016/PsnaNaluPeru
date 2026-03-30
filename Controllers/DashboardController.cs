using Happy.Filters;
using Happy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Happy.Controllers
{
    [RequireUser]
    public class DashboardController : Controller
    {
        private readonly IBookingService _service;


    public DashboardController(IBookingService service)
        {
            _service = service;
        }

        public async Task<IActionResult> UserDashboard()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var bookings = await _service.GetUserBookingsAsync(userId);
            return View(bookings);
        }
    }


}
