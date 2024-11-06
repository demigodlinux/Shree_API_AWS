using Microsoft.IdentityModel.Tokens;
using Shree_API_AWS.Models;
using Shree_API_AWS.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shree_API_AWS.Services
{
    public class TokenService: IToken
    {
        private readonly IConfiguration configuration;
        public TokenService(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public Task<string> CreateToken(Userdetailstable user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(1440),
                signingCredentials: credentials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
