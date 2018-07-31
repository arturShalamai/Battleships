using Battleships.BLL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IPlayerService _playerSvc;

        public TokenService(IConfiguration configuration, IPlayerService playerSvc)
        {
            _configuration = configuration;
            _playerSvc = playerSvc;
        }

        public async Task<string> GetTokenAsync(string userEmail)
        {
            var player = await _playerSvc.GetPlayer(p => p.Email == userEmail);

            var claims = new[]
            {
                    new Claim("sub", player.Id.ToString()),
                    new Claim("NickName", player.NickName),
                    new Claim(ClaimTypes.Name, player.FirstName),
                    new Claim(ClaimTypes.Email, player.Email)
                };

            var securityKey = _configuration["SecurityKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:44310",
                audience: "https://localhost:44310",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
