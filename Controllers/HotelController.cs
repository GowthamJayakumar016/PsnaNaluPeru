using Microsoft.AspNetCore.Mvc;
using Happy.Services.Interfaces;

namespace Happy.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _service;

        public HotelController(IHotelService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var hotels = await _service.GetAllHotelsAsync();
            return View(hotels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _service.GetHotelByIdAsync(id);
            return View(hotel);
        }
    }


}