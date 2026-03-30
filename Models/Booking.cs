using System;

namespace Happy.Models
{
    public class Booking
    {
        public int Id { get; set; }

    public int UserId { get; set; }

        public int RoomId { get; set; }

        public int NumberOfPersons { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        // 2-hour cancellation deadline (set after payment succeeds).
        // If null, cancellation window isn't active yet.
        public DateTime? CancelledAt { get; set; }

        // Links a rebooked booking to the booking it was created from.
        public int? RebookedFromBookingId { get; set; }
        public Booking? RebookedFromBooking { get; set; }

        // Navigation
        public User User { get; set; }

        public Room Room { get; set; }

        public Payment Payment { get; set; }
    }


}
