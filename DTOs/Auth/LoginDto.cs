using System.ComponentModel.DataAnnotations;

namespace Happy.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

<<<<<<< HEAD

=======
>>>>>>> d06a45075b50152851f83015625ddbc5ebc9a16d
    [Required]
        public string Password { get; set; }
    }

}
