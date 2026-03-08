using StackExchange.Redis;

namespace POC.API.Infrastructure.Redis
{
    public class TokenBlocklistService : ITokenBlocklistService
    {
        private readonly IDatabase _database;

        public TokenBlocklistService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task BlockTokenAsync(string jti, TimeSpan ttl)
        {
            await _database.StringSetAsync($"blocked_token:{jti}", "1", ttl);
        }

        public async Task<bool> IsBlockedAsync(string jti)
        {
            return await _database.KeyExistsAsync($"blocked_token:{jti}");
        }
    }
}
