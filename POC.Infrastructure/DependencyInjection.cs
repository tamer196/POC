using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POC.Application.Redis;
using POC.Application.Services;
using POC.Infrastructure.Redis;
using POC.Infrastructure.Services;
using StackExchange.Redis;

namespace POC.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ITokenBlocklistService, TokenBlocklistService>();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection = configuration.GetConnectionString("Redis");

                var options = ConfigurationOptions.Parse(redisConnection!);
                options.AbortOnConnectFail = false;
                options.ConnectRetry = 5;

                return ConnectionMultiplexer.Connect(options);
            });

            return services;
        }
    }
}

