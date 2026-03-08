namespace POC.API.Infrastructure.Redis
{
    public interface ITokenBlocklistService
    {
        Task BlockTokenAsync(string jti, TimeSpan ttl);
        Task<bool> IsBlockedAsync(string jti);
    }
}
