using System;

namespace Happy.DTOs.Booking
{
    public class RebookPrefillDto
    {
        public int OriginalBookingId { get; set; }
        public int RoomId { get; set; }
        public int NumberOfPersons { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}

