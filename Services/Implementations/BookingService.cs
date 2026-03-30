using Happy.Data;
using Happy.DTOs.Booking;
using Happy.DTOs.Payment;
using Happy.Models;
using Happy.Repositories.Interfaces;
using Happy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Happy.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IRoomRepository _roomRepo;
        private readonly ICouponService _couponService;
        private readonly AppDbContext _db;

        public BookingService(
            IBookingRepository repo,
            IRoomRepository roomRepo,
            ICouponService couponService,
            AppDbContext db)
        {
            _repo = repo;
            _roomRepo = roomRepo;
            _couponService = couponService;
            _db = db;
        }

        // Step 1: booking details -> create a PendingPayment draft.
        public async Task<CreateBookingDraftResultDto> CreateBookingDraftAsync(CreateBookingDto dto, int userId)
        {
            if (dto.CheckOut <= dto.CheckIn)
                return new CreateBookingDraftResultDto { BookingId = null, Error = "Invalid dates", RoomUnavailable = false };

            // Total days = ceiling of datetime duration (min 1 day).
            var totalDaysDouble = (dto.CheckOut - dto.CheckIn).TotalHours / 24d;
            var totalDays = (int)Math.Ceiling(totalDaysDouble);
            if (totalDays < 1)
                totalDays = 1;

            var bookings = await _repo.GetBookingsByRoomIdAsync(dto.RoomId);

            foreach (var b in bookings)
            {
                if (b.Status == "Confirmed" || b.Status == "Pending" || b.Status == "PendingPayment")
                {
                    // overlap query: [newIn, newOut) overlaps [bIn, bOut)
                    if (dto.CheckIn < b.CheckOut && dto.CheckOut > b.CheckIn)
                        return new CreateBookingDraftResultDto { BookingId = null, Error = "Room already booked for selected dates.", RoomUnavailable = true };
                }
            }

            var room = await _roomRepo.GetRoomByIdAsync(dto.RoomId);
            if (room == null || !room.IsActive)
                return new CreateBookingDraftResultDto { BookingId = null, Error = "Selected room is not available.", RoomUnavailable = true };

            if (dto.NumberOfPersons > room.Capacity)
                return new CreateBookingDraftResultDto { BookingId = null, Error = "Guest count exceeds room capacity.", RoomUnavailable = true };

            var totalPrice = room.Price * totalDays;

            // Loyalty: if user rebooks within the same hotel, apply 7% discount.
            if (dto.RebookedFromBookingId.HasValue)
            {
                var original = await _repo.GetBookingByIdWithRoomAndUserAsync(dto.RebookedFromBookingId.Value);
                if (original != null && original.Room != null && original.Room.HotelId == room.HotelId)
                {
                    totalPrice = totalPrice * 0.93m; // 7% off
                }
            }

            var bookingDraft = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                NumberOfPersons = dto.NumberOfPersons,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                TotalPrice = totalPrice,
                Status = "PendingPayment",
                RebookedFromBookingId = dto.RebookedFromBookingId
            };

            await _repo.AddBookingAsync(bookingDraft);
            await _repo.SaveAsync();

            return new CreateBookingDraftResultDto { BookingId = bookingDraft.Id, Error = null, RoomUnavailable = false };
        }

        // Step 2: payment checkout page needs booking info.
        public async Task<PaymentCheckoutViewDto?> GetPaymentCheckoutAsync(int bookingId, int userId)
        {
            var booking = await _repo.GetBookingByIdWithRoomAndUserAsync(bookingId);
            if (booking == null || booking.UserId != userId)
                return null;

            if (booking.Status != "PendingPayment")
                return null;

            var totalDaysDouble = (booking.CheckOut - booking.CheckIn).TotalHours / 24d;
            var totalDays = (int)Math.Ceiling(totalDaysDouble);
            if (totalDays < 1)
                totalDays = 1;

            return new PaymentCheckoutViewDto
            {
                BookingId = booking.Id,
                HotelName = booking.Room.Hotel.Name,
                RoomNumber = booking.Room.RoomNumber,
                RoomType = booking.Room.Type,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                NumberOfPersons = booking.NumberOfPersons,
                TotalDays = totalDays,
                SubtotalPrice = booking.TotalPrice,
                DiscountAmount = 0m,
                TotalPrice = booking.TotalPrice,
                CouponCode = null,
                Error = null
            };
        }

        public async Task<ConfirmPaymentResultDto> ConfirmPaymentAsync(int bookingId, int userId, PaymentConfirmDto dto)
        {
            var booking = await _repo.GetBookingByIdWithRoomAndUserAsync(bookingId);
            if (booking == null || booking.UserId != userId)
                return new ConfirmPaymentResultDto { Success = false, Error = "Booking not found" };

            if (booking.Status != "PendingPayment")
                return new ConfirmPaymentResultDto { Success = false, Error = "Booking not ready for payment" };

            // Re-check availability right before confirming payment.
            var roomBookings = await _repo.GetBookingsByRoomIdAsync(booking.RoomId);
            foreach (var other in roomBookings)
            {
                if (other.Id == booking.Id)
                    continue;

                if (other.Status == "Confirmed" || other.Status == "Pending" || other.Status == "PendingPayment")
                {
                    if (booking.CheckIn < other.CheckOut && booking.CheckOut > other.CheckIn)
                    {
                        booking.Status = "Cancelled";
                        booking.CancelledAt = DateTime.UtcNow;
                        await _repo.SaveAsync();

                        return new ConfirmPaymentResultDto
                        {
                            Success = false,
                            Error = "Room is no longer available. Please try booking another room or dates.",
                            DiscountAmount = 0m,
                            TotalPrice = booking.TotalPrice
                        };
                    }
                }
            }

            var subtotal = booking.TotalPrice;
            var (ok, coupon, discountAmount, error) = await _couponService.ValidateAndComputeAsync(dto.CouponCode, subtotal);
            if (!ok)
                return new ConfirmPaymentResultDto { Success = false, Error = error, DiscountAmount = 0m, TotalPrice = subtotal };

            var finalTotal = subtotal - discountAmount;
            if (finalTotal < 0) finalTotal = 0;

            // Update booking
            booking.TotalPrice = finalTotal;
            booking.Status = "Pending";
            booking.CancelledAt = DateTime.UtcNow.AddHours(2);

            if (coupon != null)
                coupon.UsageCount += 1;

            // Create payment record
            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = finalTotal,
                DiscountAmount = discountAmount,
                CouponCodeApplied = coupon?.Code,
                Method = dto.Method,
                Status = "Paid",
                TransactionId = Guid.NewGuid().ToString("N"),
            };

            await _db.Payments.AddAsync(payment);
            await _repo.SaveAsync();

            return new ConfirmPaymentResultDto
            {
                Success = true,
                Error = null,
                DiscountAmount = discountAmount,
                TotalPrice = finalTotal,
                CouponCodeApplied = coupon?.Code
            };
        }

        public async Task<List<BookingViewDto>> GetUserBookingsAsync(int userId)
        {
            var bookings = await _repo.GetBookingsByUserIdAsync(userId);

            // Hide drafts from "My Bookings"
            bookings = bookings.Where(b => b.Status != "PendingPayment").ToList();

            var list = new List<BookingViewDto>();
            foreach (var b in bookings)
            {
                list.Add(new BookingViewDto
                {
                    Id = b.Id,
                    HotelName = b.Room.Hotel.Name,
                    RoomNumber = b.Room.RoomNumber,
                    RoomType = b.Room.Type,
                    RoomId = b.RoomId,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    NumberOfPersons = b.NumberOfPersons,
                    CancelledAt = b.CancelledAt
                });
            }

            return list;
        }

        public async Task<bool> CancelBookingAsync(int bookingId, int userId)
        {
            var booking = await _repo.GetBookingByIdAsync(bookingId);
            if (booking == null || booking.UserId != userId)
                return false;

            // Only allow cancellation after successful booking (within window)
            if (booking.Status != "Pending")
                return false;

            if (booking.CancelledAt == null)
                return false;

            if (DateTime.UtcNow > booking.CancelledAt.Value)
                return false;

            booking.Status = "Cancelled";
            await _repo.SaveAsync();
            return true;
        }

        public async Task<RebookPrefillDto?> GetRebookPrefillAsync(int bookingId, int userId)
        {
            var booking = await _repo.GetBookingByIdWithRoomAndUserAsync(bookingId);
            if (booking == null || booking.UserId != userId)
                return null;

            if (booking.Status == "PendingPayment")
                return null;

            return new RebookPrefillDto
            {
                OriginalBookingId = booking.Id,
                RoomId = booking.RoomId,
                NumberOfPersons = booking.NumberOfPersons,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut
            };
        }
    }
}
