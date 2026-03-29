using Happy.Models;

namespace Happy.Repositories.Interfaces
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAllHotelsAsync();
        Task<Hotel> GetHotelByIdAsync(int id);
    }
}
