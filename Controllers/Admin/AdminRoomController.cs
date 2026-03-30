using Microsoft.AspNetCore.Mvc;
using Happy.Services.Interfaces.Admin;

namespace Happy.Controllers
{
    public class AdminRoomController : Controller
    {
        private readonly IAdminRoomService _service;


    public AdminRoomController(IAdminRoomService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId"));

            var rooms = await _service.GetRoomsByHotelIdAsync(hotelId);

            // 🔥 IMPORTANT CHANGE
            return View("Rooms", rooms);
        }
    }


}
