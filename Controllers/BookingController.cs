using Microsoft.AspNetCore.Mvc;
using Happy.DTOs.Booking;
using Happy.Services.Interfaces;

namespace Happy.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _service;

    public BookingController(IBookingService service)
        {
            _service = service;
        }

        public IActionResult Create(int roomId)
        {
            var dto = new CreateBookingDto
            {
                RoomId = roomId
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            int userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var result = await _service.CreateBookingAsync(dto, userId);

            if (!result)
            {
                ModelState.AddModelError("", "Room not available");
                return View(dto);
            }

            return RedirectToAction("MyBookings");
        }

        public async Task<IActionResult> MyBookings()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var bookings = await _service.GetUserBookingsAsync(userId);
            return View(bookings);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            await _service.CancelBookingAsync(id);
            return RedirectToAction("MyBookings");
        }
    }


}
