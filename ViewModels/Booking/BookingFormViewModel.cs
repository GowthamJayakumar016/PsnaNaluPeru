using System;
using System.ComponentModel.DataAnnotations;

namespace Happy.ViewModels.Booking
{
    public class BookingFormViewModel
    {
        public int RoomId { get; set; }

    public string RoomNumber { get; set; }

        public string RoomType { get; set; }

        public decimal Price { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }
    }


}
