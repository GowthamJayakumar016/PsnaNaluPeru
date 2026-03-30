using Happy.Data;
using Happy.Models;
using Happy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Happy.Repositories.Implementations
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;

    public HotelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels
                .Where(h => h.IsActive)
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);
        }
    }


}
