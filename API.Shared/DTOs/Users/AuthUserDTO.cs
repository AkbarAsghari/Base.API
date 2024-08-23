namespace API.Shared.DTOs.Users
{
    public class AuthUserDTO
    {
        public AuthUserDTO(string token,string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
