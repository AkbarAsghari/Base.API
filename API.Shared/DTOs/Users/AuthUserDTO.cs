namespace API.Shared.DTOs.Users
{
    public class AuthUserDTO
    {
        public AuthUserDTO(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    }
}
