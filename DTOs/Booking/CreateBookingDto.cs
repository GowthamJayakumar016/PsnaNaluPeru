using System;
using System.ComponentModel.DataAnnotations;

namespace Happy.DTOs.Booking
{
    public class CreateBookingDto
    {
        [Required]
        public int RoomId { get; set; }



 
        [Required]

        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }

        [Required]
        [Range(1, 20)]
        public int NumberOfPersons { get; set; }

        // Links this draft to the original booking when user clicks "Rebook".
        public int? RebookedFromBookingId { get; set; }
    }

}
