using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using POC.Application.Services;
using POC.Domain.Entities;
using POC.Persistence.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POC.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateAccessToken(AppUser user, string jti)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, jti),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
