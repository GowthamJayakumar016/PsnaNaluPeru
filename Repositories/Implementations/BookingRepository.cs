using Happy.Data;
using Happy.Models;
using Happy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Happy.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;


    public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId)
        {
            return await _context.Bookings
                .Where(b => b.RoomId == roomId)
                .ToListAsync();
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }


}
