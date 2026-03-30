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

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<Booking?> GetBookingByIdWithRoomAndUserAsync(int id)
        {
            return await _context.Bookings
                .Include(x => x.Room)
                .ThenInclude(r => r.Hotel)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<HashSet<int>> GetRoomIdsWithActiveConfirmedStayAsync(IReadOnlyList<int> roomIds, DateTime at)
        {
            if (roomIds.Count == 0)
                return new HashSet<int>();

            var ids = await _context.Bookings
                .AsNoTracking()
                .Where(b => roomIds.Contains(b.RoomId)
                    && (b.Status == "Confirmed" || b.Status == "Pending" || b.Status == "PendingPayment")
                    && b.CheckIn <= at
                    && b.CheckOut > at)
                .Select(b => b.RoomId)
                .Distinct()
                .ToListAsync();

            return ids.ToHashSet();
        }

        public async Task SaveAsync()
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }


}
