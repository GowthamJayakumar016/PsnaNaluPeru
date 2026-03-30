using Happy.DTOs.Booking;
using Happy.Filters;
using Happy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Happy.Repositories.Interfaces;

namespace Happy.Controllers
{
    [RequireUser]
    public class BookingController : Controller
    {
        private readonly IBookingService _service;
        private readonly IRoomRepository _roomRepo;

    public BookingController(IBookingService service, IRoomRepository roomRepo)
        {
            _service = service;
            _roomRepo = roomRepo;
        }

        public async Task<IActionResult> Create(int roomId)
        {
            // If user clicked "Rebook", TempData may contain prefill values.
            var prefill = TempData.Peek("RebookPrefill") as RebookPrefillDto;
            var dto = new CreateBookingDto
            {
                RoomId = roomId
            };

            if (prefill != null)
            {
                dto.NumberOfPersons = prefill.NumberOfPersons;
                dto.CheckIn = prefill.CheckIn;
                dto.CheckOut = prefill.CheckOut;
                dto.RebookedFromBookingId = prefill.OriginalBookingId;
            }

            // For pricing UX: show all active rooms of the same "type" for this hotel.
            var selectedRoom = await _roomRepo.GetRoomByIdAsync(roomId);
            if (selectedRoom != null)
            {
                var hotelRooms = await _roomRepo.GetRoomsByHotelIdAsync(selectedRoom.HotelId);
                var options = hotelRooms
                    .Where(r => r.Type == selectedRoom.Type && (r.IsActive || r.Id == selectedRoom.Id))
                    .OrderBy(r => r.RoomNumber)
                    .Select(r => new { r.Id, r.RoomNumber, r.Price })
                    .ToList();
                ViewBag.RoomOptions = options;
            }

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            int userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var result = await _service.CreateBookingDraftAsync(dto, userId);

            if (result.BookingId == null)
            {
                ModelState.AddModelError("", result.Error ?? "Room not available");

                return View(dto);
            }

            return RedirectToAction("Checkout", "Payment", new { bookingId = result.BookingId.Value });
        }

        public async Task<IActionResult> MyBookings()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var bookings = await _service.GetUserBookingsAsync(userId);
            return View(bookings);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId")!);
            var ok = await _service.CancelBookingAsync(id, userId);
            if (!ok)
                TempData["CancelError"] = "Cancellation window has passed. Please contact the admin.";
            return RedirectToAction("MyBookings");
        }

        public async Task<IActionResult> Rebook(int bookingId)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId")!);
            var prefill = await _service.GetRebookPrefillAsync(bookingId, userId);
            if (prefill == null)
                return RedirectToAction("MyBookings");

            TempData["RebookPrefill"] = prefill;
            return RedirectToAction("Create", new { roomId = prefill.RoomId });
        }
    }


}
