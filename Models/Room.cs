using System.Collections.Generic;

namespace Happy.Models
{
    public class Room
    {
        public int Id { get; set; }


    public int HotelId { get; set; }

        public string RoomNumber { get; set; }

        public string Type { get; set; }

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }

        // Navigation
        public Hotel Hotel { get; set; }

        public List<Booking> Bookings { get; set; }
    }


}
