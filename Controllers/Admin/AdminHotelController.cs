using Happy.Data;
using Happy.DTOs.Admin;
using Happy.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Happy.Controllers.Admin
{
    [RequireAdmin]
    public class AdminHotelController : Controller
    {
        private readonly AppDbContext _db;

        public AdminHotelController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId);
            if (hotel == null)
                return NotFound();

            var dto = new AdminHotelEditDto
            {
                Name = hotel.Name,
                Location = hotel.Location,
                Address = hotel.Address,
                Phone = hotel.Phone,
                Email = hotel.Email
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminHotelEditDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            int hotelId = int.Parse(HttpContext.Session.GetString("HotelId")!);
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId);
            if (hotel == null)
                return NotFound();

            hotel.Name = dto.Name;
            hotel.Location = dto.Location;
            hotel.Address = dto.Address;
            hotel.Phone = dto.Phone;
            hotel.Email = dto.Email;

            await _db.SaveChangesAsync();
            TempData["HotelSavedMessage"] = "Hotel details updated.";

            return RedirectToAction(nameof(Edit));
        }
    }
}

