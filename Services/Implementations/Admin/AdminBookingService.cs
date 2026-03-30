using Happy.DTOs.Admin;
using Happy.Repositories.Interfaces;
using Happy.Repositories.Interfaces.Admin;
using Happy.Services.Interfaces;
using Happy.Services.Interfaces.Admin;

namespace Happy.Services.Implementations.Admin
{
    public class AdminBookingService : IAdminBookingService
    {
        private readonly IAdminBookingRepository _repo;
        private readonly IBookingRepository _bookingRepo;

        public AdminBookingService(IAdminBookingRepository repo, IBookingRepository bookingRepo)
        {
            _repo = repo;
            _bookingRepo = bookingRepo;
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

        public async Task ApproveBookingAsync(int bookingId, int hotelId)
        {
            var b = await _repo.GetBookingByIdWithRoomAsync(bookingId);
            if (b == null || b.Room.HotelId != hotelId || b.Status != "Pending")
                return;

            var roomBookings = await _bookingRepo.GetBookingsByRoomIdAsync(b.RoomId);
            foreach (var other in roomBookings)
            {
                if (other.Id == b.Id || other.Status != "Confirmed")
                    continue;
                if (b.CheckIn < other.CheckOut && b.CheckOut > other.CheckIn)
                    return;
            }

            b.Status = "Confirmed";
            await _repo.SaveAsync();

        }

        public async Task RejectBookingAsync(int bookingId, int hotelId)
        {
            var b = await _repo.GetBookingByIdWithRoomAsync(bookingId);
            if (b == null || b.Room.HotelId != hotelId || b.Status != "Pending")
                return;

            b.Status = "Cancelled";
            await _repo.SaveAsync();

        }

        public async Task CompleteBookingAsync(int bookingId, int hotelId)
        {
            var b = await _repo.GetBookingByIdWithRoomAsync(bookingId);
            if (b == null || b.Room.HotelId != hotelId || b.Status != "Confirmed")
                return;

            b.Status = "Completed";
            await _repo.SaveAsync();

        }
    }
}
