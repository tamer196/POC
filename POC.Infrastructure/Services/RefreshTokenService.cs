using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using POC.Application.Security;
using POC.Application.Services;
using POC.Domain.Entities;
using POC.Persistence;

namespace POC.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly JwtOptions _jwtOptions;

        public RefreshTokenService(
            AppDbContext dbContext,
            IJwtService jwtService,
            IOptions<JwtOptions> jwtOptions)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<RefreshToken> CreateAsync(User user, string accessTokenJti)
        {
            var refreshToken = new RefreshToken(
                _jwtService.GenerateRefreshToken(),
                DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays),
                accessTokenJti,
                user.Id
            );

            _dbContext.RefreshTokens.Add(refreshToken);

            await _dbContext.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken?> GetValidTokenAsync(string token)
        {
            return await _dbContext.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    !x.IsRevoked &&
                    x.ExpiresAtUtc > DateTime.UtcNow);
        }

        public async Task RevokeAsync(string token)
        {
            var refreshToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            if (refreshToken is null)
                return;

            refreshToken.Revoke();

            await _dbContext.SaveChangesAsync();
        }
    }
}
