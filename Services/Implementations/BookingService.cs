using Happy.DTOs.Booking;
using Happy.Models;
using Happy.Repositories.Interfaces;
using Happy.Services.Interfaces;

namespace Happy.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IRoomRepository _roomRepo;


    public BookingService(IBookingRepository repo, IRoomRepository roomRepo)
        {
            _repo = repo;
            _roomRepo = roomRepo;
        }

        public async Task<bool> CreateBookingAsync(CreateBookingDto dto, int userId)
        {
            var bookings = await _repo.GetBookingsByRoomIdAsync(dto.RoomId);

            foreach (var b in bookings)
            {
                if (b.Status == "Confirmed")
                {
                    if (dto.CheckIn < b.CheckOut && dto.CheckOut > b.CheckIn)
                        return false;
                }
            }

            var room = await _roomRepo.GetRoomByIdAsync(dto.RoomId);

            var days = (dto.CheckOut - dto.CheckIn).Days;

            var booking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                TotalPrice = room.Price * days,
                Status = "Pending"
            };

            await _repo.AddBookingAsync(booking);
            await _repo.SaveAsync();

            return true;
        }

        public async Task<List<BookingViewDto>> GetUserBookingsAsync(int userId)
        {
            var bookings = await _repo.GetBookingsByUserIdAsync(userId);

            var list = new List<BookingViewDto>();

            foreach (var b in bookings)
            {
                list.Add(new BookingViewDto
                {
                    Id = b.Id,
                    HotelName = b.Room.Hotel.Name,
                    RoomNumber = b.Room.RoomNumber,
                    RoomType = b.Room.Type,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status
                });
            }

            return list;
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _repo.GetBookingByIdAsync(bookingId);

            if (booking == null)
                return;

            var hours = (booking.CheckIn - DateTime.Now).TotalHours;

            if (hours > 4)
            {
                booking.Status = "Cancelled";
                await _repo.SaveAsync();
            }
        }
    }


}
