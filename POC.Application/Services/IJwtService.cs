using POC.Domain.Entities;

namespace POC.Application.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user, string jti);
        string GenerateRefreshToken();
    }
}
