using Happy.Data;
using Happy.Models;
using Happy.Repositories.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace Happy.Repositories.Implementations.Admin
{
    public class AdminRoomRepository : IAdminRoomRepository
    {
        private readonly AppDbContext _context;


    public AdminRoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(x => x.HotelId == hotelId)
                .ToListAsync();
        }
    }

}
