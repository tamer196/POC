using POC.Application.Redis;
using StackExchange.Redis;

namespace POC.Infrastructure.Redis
{
    public class TokenBlocklistService : ITokenBlocklistService
    {
        private readonly IDatabase _database;
        private const string PREFIX = "blocked_token:";

        public TokenBlocklistService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task BlockTokenAsync(string jti, TimeSpan ttl)
        {
            await _database.StringSetAsync($"{PREFIX}{jti}", "1", ttl);
        }

        public async Task<bool> IsBlockedAsync(string jti)
        {
            return await _database.KeyExistsAsync($"{PREFIX}{jti}");
        }
    }
}
