using Happy.DTOs.Admin;
using Happy.Repositories.Interfaces.Admin;
using Happy.Services.Interfaces.Admin;

namespace Happy.Services.Implementations.Admin
{
    public class AdminBookingService : IAdminBookingService
    {
        private readonly IAdminBookingRepository _repo;


    public AdminBookingService(IAdminBookingRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AdminBookingDto>> GetBookingsByHotelIdAsync(int hotelId)
        {
            var bookings = await _repo.GetBookingsByHotelIdAsync(hotelId);

            var list = new List<AdminBookingDto>();

            foreach (var b in bookings)
            {
                list.Add(new AdminBookingDto
                {
                    Id = b.Id,
                    UserName = b.User.Name,
                    RoomNumber = b.Room.RoomNumber,
                    RoomType = b.Room.Type,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    Status = b.Status
                });
            }

            return list;
        }

        public async Task ApproveBookingAsync(int bookingId)
        {
            var b = await _repo.GetBookingByIdAsync(bookingId);
            if (b != null)
            {
                b.Status = "Confirmed";
                await _repo.SaveAsync();
            }
        }

        public async Task RejectBookingAsync(int bookingId)
        {
            var b = await _repo.GetBookingByIdAsync(bookingId);
            if (b != null)
            {
                b.Status = "Cancelled";
                await _repo.SaveAsync();
            }
        }

        public async Task CompleteBookingAsync(int bookingId)
        {
            var b = await _repo.GetBookingByIdAsync(bookingId);
            if (b != null)
            {
                b.Status = "Completed";
                await _repo.SaveAsync();
            }
        }
    }


}
