using POC.Domain.Entities;

namespace POC.Application.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateAsync(User user, string accessTokenJti);
        Task<RefreshToken?> GetValidTokenAsync(string token);
        Task RevokeAsync(string token);
    }
}
