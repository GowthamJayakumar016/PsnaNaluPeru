using Microsoft.AspNetCore.Mvc;
using Happy.Services.Interfaces;

namespace Happy.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }

        public async Task<IActionResult> List(int hotelId)
        {
            var rooms = await _service.GetRoomsByHotelIdAsync(hotelId);
            return View(rooms);
        }
    }


}