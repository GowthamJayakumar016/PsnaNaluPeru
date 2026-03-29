using System.ComponentModel.DataAnnotations;

namespace Happy.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }
<<<<<<< HEAD


    [Required]
=======
        [Required]
>>>>>>> d06a45075b50152851f83015625ddbc5ebc9a16d
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }

<<<<<<< HEAD

=======
>>>>>>> d06a45075b50152851f83015625ddbc5ebc9a16d
}
