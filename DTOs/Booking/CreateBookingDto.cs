using System;
using System.ComponentModel.DataAnnotations;

namespace Happy.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
<<<<<<< HEAD


    [Required]
=======
        [Required]
>>>>>>> d06a45075b50152851f83015625ddbc5ebc9a16d
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }
    }

}
