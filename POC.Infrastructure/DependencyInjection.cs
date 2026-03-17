using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POC.Application.Journal.Interfaces;
using POC.Application.Redis;
using POC.Application.Services;
using POC.Infrastructure.Kafka;
using POC.Infrastructure.Mongo;
using POC.Infrastructure.Mongo.Repositories;
using POC.Infrastructure.Presence;
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
            services.Configure<MongoSettings>(configuration.GetSection("MongoDb"));
            services.AddSingleton<MongoDbContext>();
            services.AddScoped<IJournalRepository, JournalRepository>();
            services.Configure<KafkaOptions>(configuration.GetSection(KafkaOptions.SectionName));
            services.AddSingleton<IOnlineUserStore, InMemoryOnlineUserStore>();
            services.AddHostedService<KafkaConsumerBackgroundService>();

            return services;
        }
    }
}

