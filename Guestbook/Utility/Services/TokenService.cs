using DataAccessLayer.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.Config;

namespace Utility.Service
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        public TokenService(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        public string GenerateNewToken(Entry entry)
        {
            if (entry is null) throw new ArgumentNullException();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, entry.Id.ToString()),
                new Claim(ClaimTypes.Email, entry.Email),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfig.JwtExpireDays));
            var token = new JwtSecurityToken(
                _jwtConfig.JwtIssuer,
                _jwtConfig.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
