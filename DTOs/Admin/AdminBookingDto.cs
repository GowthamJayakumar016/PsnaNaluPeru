using System;

namespace Happy.DTOs.Admin
{
    public class AdminBookingDto
    {
        public int Id { get; set; }

    public string UserName { get; set; }

        public string RoomNumber { get; set; }

        public string RoomType { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public string Status { get; set; }
    }


}
