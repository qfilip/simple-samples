using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAuth.Models;

namespace WebAuth.Services
{
    public class JwtService
    {
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "sub"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SecurityKey));
            var creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                creadentials);

            var handler = new JwtSecurityTokenHandler();
            
            return handler.WriteToken(token);
        }
    }
}
