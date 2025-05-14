using System.ComponentModel.DataAnnotations;

namespace E_Library.Dtos
{
    public class InsertUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
