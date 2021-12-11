using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Services
{
    public class JwtService
    {
        public string GenerateJwt()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "oauth_sample_id")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Auth.Secret));
            var shalgorithm = SecurityAlgorithms.HmacSha256;

            var credentials = new SigningCredentials(key, shalgorithm);

            var token = new JwtSecurityToken(
                Constants.Auth.Issuer,
                Constants.Auth.Audience,
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
