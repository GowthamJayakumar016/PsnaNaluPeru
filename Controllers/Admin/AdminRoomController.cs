using Happy.Filters;
using Happy.Data;
using Happy.DTOs.Admin;
using Happy.Services.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Happy.Models;

namespace Happy.Controllers
{
    [RequireAdmin]
    public class AdminRoomController : Controller
    {
        private readonly IAdminRoomService _service;
        private readonly AppDbContext _db;

    public AdminRoomController(IAdminRoomService service, AppDbContext db)
        {
            _service = service;
        _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId"));

            var rooms = await _service.GetRoomsByHotelIdAsync(hotelId);

            // 🔥 IMPORTANT CHANGE
            return View("Rooms", rooms);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new AdminRoomDto { IsActive = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminRoomDto dto)
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            if (!ModelState.IsValid)
                return View(dto);

            var room = new Room
            {
                HotelId = hotelId,
                RoomNumber = dto.RoomNumber,
                Type = dto.Type,
                Amenities = dto.Amenities,
                Price = dto.Price,
                Capacity = dto.Capacity,
                IsActive = dto.IsActive
            };

            _db.Rooms.Add(room);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch
            {
                ModelState.AddModelError("", "Could not save room. Room number may already exist.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == id && r.HotelId == hotelId);
            if (room == null)
                return NotFound();

            var dto = new AdminRoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Type = room.Type,
                Amenities = room.Amenities,
                Price = room.Price,
                Capacity = room.Capacity,
                IsActive = room.IsActive
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminRoomDto dto)
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            if (!ModelState.IsValid)
                return View(dto);

            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == dto.Id && r.HotelId == hotelId);
            if (room == null)
                return NotFound();

            room.RoomNumber = dto.RoomNumber;
            room.Type = dto.Type;
            room.Amenities = dto.Amenities;
            room.Price = dto.Price;
            room.Capacity = dto.Capacity;
            room.IsActive = dto.IsActive;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch
            {
                ModelState.AddModelError("", "Could not save changes. Room number may already exist.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == id && r.HotelId == hotelId);
            if (room == null)
                return NotFound();

            room.IsActive = false;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }


}
