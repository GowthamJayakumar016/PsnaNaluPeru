using Happy.DTOs.Hotel;

namespace Happy.Services.Interfaces
{
    public interface IHotelService
    {
        Task<List<HotelViewDto>> GetAllHotelsAsync();
        Task<HotelViewDto> GetHotelByIdAsync(int id);
    }
}
