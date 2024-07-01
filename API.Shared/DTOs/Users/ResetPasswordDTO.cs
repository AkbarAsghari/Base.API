using System.ComponentModel.DataAnnotations;

namespace API.Shared.DTOs.Users
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
