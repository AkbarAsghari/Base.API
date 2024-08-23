using API.Infrastructure.Entities;
using API.Shared.Enums;
using API.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace API.Core.Utilities
{
    internal class TokenUtility
    {
        public static string GenerateToken(User user)
        {
            // generate token that is valid for 365 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWTSettings.Secret);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username == null ? user.Email : user.Username));
            claims.Add(new Claim("FullName", string.IsNullOrEmpty((user.FirstName + user.LastName).Trim()) ? user.Email : $"{user.FirstName} {user.LastName}"));
            claims.Add(new Claim(ClaimTypes.Role, ((RolesEnum)user.Role).ToString()));
            claims.Add(new Claim("Permissions", "Weather.View"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(JWTSettings.TokenExpiryTimeValidMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
