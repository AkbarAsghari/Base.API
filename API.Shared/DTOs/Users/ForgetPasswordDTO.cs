using System.ComponentModel.DataAnnotations;

namespace API.Shared.DTOs.Users
{
    public class ForgetPasswordDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
