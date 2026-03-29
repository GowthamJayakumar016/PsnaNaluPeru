using System.Collections.Generic;

namespace Happy.Models
{
    public class User
    {
        public int Id { get; set; }


    public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public int? HotelId { get; set; }

        // Navigation
        public Hotel Hotel { get; set; }

        public List<Booking> Bookings { get; set; }
    }


}
