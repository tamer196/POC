using POC.API.Domain.Entities;

namespace POC.API.Features.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(AppUser user, string jti);
        string GenerateRefreshToken();
    }
}
