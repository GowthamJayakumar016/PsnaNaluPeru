using Happy.DTOs.Hotel;
using Happy.Repositories.Interfaces;
using Happy.Services.Interfaces;

namespace Happy.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _repo;


    public HotelService(IHotelRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<HotelViewDto>> GetAllHotelsAsync()
        {
            var hotels = await _repo.GetAllHotelsAsync();

            var list = new List<HotelViewDto>();

            foreach (var h in hotels)
            {
                list.Add(new HotelViewDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Location = h.Location,
                    Address = h.Address
                });
            }

            return list;
        }

        public async Task<HotelViewDto> GetHotelByIdAsync(int id)
        {
            var h = await _repo.GetHotelByIdAsync(id);

            if (h == null)
                return null;

            return new HotelViewDto
            {
                Id = h.Id,
                Name = h.Name,
                Location = h.Location,
                Address = h.Address
            };
        }
    }


}
