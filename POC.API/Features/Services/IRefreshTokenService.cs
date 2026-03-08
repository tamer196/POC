using POC.API.Domain.Entities;

namespace POC.API.Features.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateAsync(AppUser user, string accessTokenJti);
        Task<RefreshToken?> GetValidTokenAsync(string token);
        Task RevokeAsync(string token);
    }
}
