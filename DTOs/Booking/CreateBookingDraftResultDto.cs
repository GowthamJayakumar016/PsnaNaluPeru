namespace Happy.DTOs.Booking
{
    public class CreateBookingDraftResultDto
    {
        public int? BookingId { get; set; }
        public string? Error { get; set; }
        public bool RoomUnavailable { get; set; }
    }
}

