using POC.Domain.Entities;

namespace POC.Application.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(AppUser user, string jti);
        string GenerateRefreshToken();
    }
}
