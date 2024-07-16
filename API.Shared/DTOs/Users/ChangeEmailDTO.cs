using System.ComponentModel.DataAnnotations;

namespace API.Shared.DTOs.Users
{
    public class ChangeEmailDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
