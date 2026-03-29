using System.Collections.Generic;

namespace Happy.Models
{
    public class Hotel
    {
        public int Id { get; set; }


    public string Name { get; set; }

        public string Location { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        // Navigation
        public List<Room> Rooms { get; set; }

        public List<User> Admins { get; set; }
    }

}
