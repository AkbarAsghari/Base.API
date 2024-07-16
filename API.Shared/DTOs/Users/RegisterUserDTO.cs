using System.ComponentModel.DataAnnotations;

namespace API.Shared.DTOs.Users
{
    public class RegisterUserDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
