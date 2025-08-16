

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HireTech.Uitilities.DTO.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace HireTech.Uitilities.Helpers
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(TokenDTO tokenDTO)
        {
            if (tokenDTO == null)
                throw new ArgumentNullException(nameof(tokenDTO));

            if (string.IsNullOrEmpty(tokenDTO.Email))
                throw new ArgumentException("Email cannot be null or empty", nameof(tokenDTO.Email));

            if (string.IsNullOrEmpty(tokenDTO.Role))
                throw new ArgumentException("Role cannot be null or empty", nameof(tokenDTO.Role));

            if (string.IsNullOrEmpty(tokenDTO.Id))
                throw new ArgumentException("Id cannot be null or empty", nameof(tokenDTO.Id));
            var JWTSection = _configuration.GetSection("JWT");
            var secretKey = JWTSection["Key"];
            var issuer = JWTSection["ValidIssuer"];
            var audience = JWTSection["ValidAudience"];
            var expirationIn = JWTSection["expirationIn"];
            if (string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("JWT Key is not configured");

            if (string.IsNullOrEmpty(issuer))
                throw new InvalidOperationException("JWT ValidIssuer is not configured");

            if (string.IsNullOrEmpty(audience))
                throw new InvalidOperationException("JWT ValidAudience is not configured");

            var claims = new List<Claim>
            {
                    new Claim("email", tokenDTO.Email),
                    new Claim(ClaimTypes.Role, tokenDTO.Role),
                    new Claim("id", tokenDTO.Id),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Use a secure key
            var Token = new JwtSecurityToken(
          issuer: issuer, // Issuer of the token
          audience: audience, // Audience for the token
          claims: claims, // Claims to include in the token
          expires: DateTime.Now.AddDays(double.Parse(expirationIn)), // Token expiration time
          signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256) // Signing credentials using HMAC SHA256
          );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
