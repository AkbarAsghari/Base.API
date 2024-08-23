namespace API.Shared.DTOs.Users
{
    public class AuthUserDTO
    {
        public AuthUserDTO(string token,string refreshToken, DateTime refreshTokenExpiryTime)
        {
            Token = token;
            RefreshToken = refreshToken;
            TokenExpiryTime = DateTime.UtcNow.AddMinutes(JWTSettings.TokenExpiryTimeValidMinutes);
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiryTime { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
