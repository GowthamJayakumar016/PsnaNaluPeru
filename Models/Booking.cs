using System;

namespace Happy.Models
{
    public class Booking
    {
        public int Id { get; set; }


    public int UserId { get; set; }

        public int RoomId { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        // Navigation
        public User User { get; set; }

        public Room Room { get; set; }

        public Payment Payment { get; set; }
    }


}
