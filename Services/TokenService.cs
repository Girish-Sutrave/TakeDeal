using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TakeDeal.Models;


namespace TakeDeal.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            this._config = config;
        }

        public string CreateToken(User user)
        {
            // 1️⃣ Define claims
            var claims = new[]
            { 
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };
            // 2️⃣ Create signing key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // 3️⃣ Create token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );
            // 4️⃣ Return serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
