using Happy.DTOs.Booking;
using Happy.Models;
using Happy.Repositories.Interfaces;
using Happy.Services.Interfaces;

namespace Happy.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;


    public BookingService(IBookingRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CheckAvailabilityAsync(CreateBookingDto dto)
        {
            var bookings = await _repo.GetBookingsByRoomIdAsync(dto.RoomId);

            foreach (var b in bookings)
            {
                if (b.Status == "Confirmed")
                {
                    if (dto.CheckIn < b.CheckOut && dto.CheckOut > b.CheckIn)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task CreateBookingAsync(CreateBookingDto dto, int userId)
        {
            int nights = (dto.CheckOut - dto.CheckIn).Days;

            var booking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
               
                TotalPrice = nights * 1000, // simple logic
                Status = "Pending"
            };

            await _repo.AddBookingAsync(booking);
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
            var booking = await _repo.GetByIdAsync(bookingId);

            if (booking == null)
                return;

            var hoursLeft = (booking.CheckIn - DateTime.Now).TotalHours;

            if (hoursLeft > 4)
            {
                booking.Status = "Cancelled";
                await _repo.UpdateAsync(booking);
            }
        }
    }


}
