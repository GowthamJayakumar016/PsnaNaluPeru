using System;

namespace Happy.DTOs.Booking
{
    public class BookingViewDto
    {
        public int Id { get; set; }

    public string HotelName { get; set; }

        public string RoomNumber { get; set; }

        public string RoomType { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }
    }


}
