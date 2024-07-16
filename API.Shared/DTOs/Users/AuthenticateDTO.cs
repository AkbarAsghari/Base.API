using System.ComponentModel.DataAnnotations;

namespace API.Shared.DTOs.Users
{
    public class AuthenticateDTO
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
