using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC.Application.Auth.Login;
using POC.Application.Auth.Logout;
using POC.Application.Auth.Refresh;
using POC.Application.Redis;
using POC.Application.Services;
using System.IdentityModel.Tokens.Jwt;

namespace POC.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITokenBlocklistService _tokenBlocklistService;
        private readonly IConfiguration _configuration;

        public AuthController(
            IMediator mediator,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            ITokenBlocklistService tokenBlocklistService,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _tokenBlocklistService = tokenBlocklistService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var result = await _mediator.Send(new LoginCommand(request.UserName, request.Password));

            return Ok(result);
        }


        [Authorize]
        [HttpPost("refresh")]
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
