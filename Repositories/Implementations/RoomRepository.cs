using Happy.Data;
using Happy.Models;
using Happy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Happy.Repositories.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(x => x.HotelId == hotelId && x.IsActive)
                .OrderBy(x => x.RoomNumber)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
        }
    }

}
