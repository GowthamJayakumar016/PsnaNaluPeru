using Happy.Filters;
using Happy.Repositories.Interfaces;
using Happy.Models;
using Happy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Happy.Controllers
{
    [RequireUser]
    public class HotelController : Controller
    {
        private readonly IHotelService _service;
        private readonly IRoomRepository _roomRepo;

        public HotelController(IHotelService service, IRoomRepository roomRepo)
        {
            _service = service;
            _roomRepo = roomRepo;
        }

        public async Task<IActionResult> Index(
            string? location,
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            string? amenities)
        {
            var hotels = await _service.GetAllHotelsAsync();

            // Filter dropdown options (computed from the whole dataset).
            var locations = hotels
                .Select(h => h.Location)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            var allRooms = new List<Room>();
            foreach (var h in hotels)
            {
                var rooms = await _roomRepo.GetRoomsByHotelIdAsync(h.Id);
                allRooms.AddRange(rooms);
            }

            var roomTypes = allRooms
                .Select(r => r.Type)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            var amenityTokens = allRooms
                .SelectMany(r => (r.Amenities ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(t => t.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            var priceOptions = allRooms
                .Select(r => r.Price)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.LocationOptions = locations;
            ViewBag.RoomTypeOptions = roomTypes;
            ViewBag.AmenitiesOptions = amenityTokens;
            ViewBag.PriceOptions = priceOptions;

            // Optional in-memory filter (works for small demo data).
            if (!string.IsNullOrWhiteSpace(location) ||
                !string.IsNullOrWhiteSpace(category) ||
                minPrice.HasValue ||
                maxPrice.HasValue ||
                !string.IsNullOrWhiteSpace(amenities))
            {
                var filtered = new List<Happy.DTOs.Hotel.HotelViewDto>();
                foreach (var h in hotels)
                {
                    if (!string.IsNullOrWhiteSpace(location) &&
                        !h.Location.Contains(location, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var rooms = await _roomRepo.GetRoomsByHotelIdAsync(h.Id);
                    var matches = rooms.Any(r =>
                        (string.IsNullOrWhiteSpace(category) ||
                         r.Type.Equals(category, StringComparison.OrdinalIgnoreCase))
                        &&
                        (!minPrice.HasValue || r.Price >= minPrice.Value)
                        &&
                        (!maxPrice.HasValue || r.Price <= maxPrice.Value)
                        &&
                        (string.IsNullOrWhiteSpace(amenities) ||
                         (!string.IsNullOrWhiteSpace(r.Amenities) &&
                          r.Amenities.Contains(amenities, StringComparison.OrdinalIgnoreCase)))
                    );

                    if (matches)
                        filtered.Add(h);
                }

                hotels = filtered;
            }

            return View(hotels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _service.GetHotelByIdAsync(id);
            if (hotel == null)
                return NotFound();
            return View(hotel);
        }
    }


}