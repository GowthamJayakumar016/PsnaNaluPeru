namespace Happy.DTOs.Admin
{
    public class AdminRoomDto
    {
        public int Id { get; set; }
    public string RoomNumber { get; set; }

        public string Type { get; set; }

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }
    }

}
