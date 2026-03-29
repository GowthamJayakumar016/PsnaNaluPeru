using System;
using System.ComponentModel.DataAnnotations;

namespace Happy.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }


    [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }
    }

}
