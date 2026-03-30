namespace Happy.DTOs.Room
{
    public class RoomViewDto
    {
        public int Id { get; set; }

    public string RoomNumber { get; set; }

        public string Type { get; set; }

        public string Amenities { get; set; }

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        /// <summary>
        /// True if there is a Confirmed stay active at this moment (dynamic availability).
        /// </summary>
        public bool IsOccupiedNow { get; set; }
    }
}

