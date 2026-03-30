using System;

namespace Happy.Models
{
    public class Payment
    {
        public int Id { get; set; }


    public int BookingId { get; set; }

        public decimal Amount { get; set; }

        public decimal DiscountAmount { get; set; }
        public string? CouponCodeApplied { get; set; }

        public string Method { get; set; }

        public string Status { get; set; }

        public string TransactionId { get; set; }

        // Navigation
        public Booking Booking { get; set; }
    }


}
