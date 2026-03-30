using Happy.Data;
using Happy.Models;
using Happy.Repositories.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace Happy.Repositories.Implementations.Admin
{
    public class AdminBookingRepository : IAdminBookingRepository
    {
        private readonly AppDbContext _context;


    public AdminBookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetBookingsByHotelIdAsync(int hotelId)
        {
            return await _context.Bookings
                .Include(x => x.User)
                .Include(x => x.Room)
                .Where(x => x.Room.HotelId == hotelId && x.Status != "PendingPayment")
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdWithRoomAsync(int id)
        {
            return await _context.Bookings
                .Include(x => x.Room)
                .ThenInclude(r => r.Hotel)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
