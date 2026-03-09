using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POC.Application.Auth.Login;
using POC.Application.Auth.Logout;
using POC.Application.Auth.Refresh;
using POC.Application.Redis;
using POC.Application.Services;
using POC.Persistence;
using POC.Persistence.Security;
using System.IdentityModel.Tokens.Jwt;

namespace POC.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITokenBlocklistService _tokenBlocklistService;
        private readonly IConfiguration _configuration;

        public AuthController(
            AppDbContext dbContext,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            ITokenBlocklistService tokenBlocklistService,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _tokenBlocklistService = tokenBlocklistService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.UserName == request.UserName && x.IsActive);

            if (user is null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password." });

            var jti = Guid.NewGuid().ToString();
            var accessToken = _jwtService.GenerateAccessToken(user, jti);
            var refreshToken = await _refreshTokenService.CreateAsync(user, jti);

            return Ok(new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                UserName = user.UserName,
                Role = user.Role
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var existingRefreshToken = await _refreshTokenService.GetValidTokenAsync(request.RefreshToken);

            if (existingRefreshToken is null)
                return Unauthorized(new { message = "Invalid refresh token." });

            await _refreshTokenService.RevokeAsync(request.RefreshToken);

            // 🔴 revoke old access token
            await _tokenBlocklistService.BlockTokenAsync(
                existingRefreshToken.AccessTokenJti,
                TimeSpan.FromMinutes(int.Parse(_configuration["Jwt:AccessTokenMinutes"]!))
            );

            // generate new tokens
            var newJti = Guid.NewGuid().ToString();

            var newAccessToken = _jwtService.GenerateAccessToken(existingRefreshToken.User, newJti);

            var newRefreshToken = await _refreshTokenService.CreateAsync(
                existingRefreshToken.User,
                newJti
            );

            return Ok(new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var jti = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (!string.IsNullOrWhiteSpace(jti))
            {
                var accessTokenMinutes = int.Parse(_configuration["Jwt:AccessTokenMinutes"]!);
                await _tokenBlocklistService.BlockTokenAsync(jti, TimeSpan.FromMinutes(accessTokenMinutes));
            }

            if (!string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                await _refreshTokenService.RevokeAsync(request.RefreshToken);
            }

            return Ok(new { message = "Logged out successfully." });
        }
    }
}
