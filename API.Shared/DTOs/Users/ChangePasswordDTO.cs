using System.ComponentModel.DataAnnotations;

namespace API.Shared.DTOs.Users
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
