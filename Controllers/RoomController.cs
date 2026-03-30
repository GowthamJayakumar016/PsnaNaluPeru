using Happy.Filters;
using Happy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Happy.Controllers
{
    [RequireUser]
    public class RoomController : Controller
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }

        public async Task<IActionResult> List(
            int hotelId,
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            string? amenities)
        {
            var rooms = await _service.GetRoomsByHotelIdAsync(hotelId);

            // Dropdown options (based on this hotel's dataset).
            ViewBag.RoomTypeOptions = rooms
                .Select(r => r.Type)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            ViewBag.PriceOptions = rooms
                .Select(r => r.Price)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.AmenitiesOptions = rooms
                .SelectMany(r => (r.Amenities ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(t => t.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            if (!string.IsNullOrWhiteSpace(category))
                rooms = rooms.Where(r => r.Type.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            if (minPrice.HasValue)
                rooms = rooms.Where(r => r.Price >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                rooms = rooms.Where(r => r.Price <= maxPrice.Value).ToList();

            if (!string.IsNullOrWhiteSpace(amenities))
            {
                rooms = rooms.Where(r =>
                    !string.IsNullOrWhiteSpace(r.Amenities) &&
                    r.Amenities.Contains(amenities, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(rooms);
        }
    }


}