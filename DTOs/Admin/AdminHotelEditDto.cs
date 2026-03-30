using System.ComponentModel.DataAnnotations;

namespace Happy.DTOs.Admin
{
    public class AdminHotelEditDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

