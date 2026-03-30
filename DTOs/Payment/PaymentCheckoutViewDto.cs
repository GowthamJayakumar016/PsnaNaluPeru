using System;

namespace Happy.DTOs.Payment
{
    public class PaymentCheckoutViewDto
    {
        public int BookingId { get; set; }

        public string HotelName { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public int NumberOfPersons { get; set; }

        public int TotalDays { get; set; }

        public decimal SubtotalPrice { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }

        public string? CouponCode { get; set; }
        public string? Error { get; set; }
    }
}

