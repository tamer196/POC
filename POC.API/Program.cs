
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POC.API.Features.Services;
using POC.API.Infrastructure.Persistence;
using POC.API.Infrastructure.Redis;
using POC.API.Infrastructure.Security;
using POC.API.Middleware;
using StackExchange.Redis;
using System.Text;

namespace POC.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

            var jwtOptions = builder.Configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>()!;

            // Controllers
            builder.Services.AddControllers();

            // PostgreSQL
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));

            // Redis (with retry configuration)
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection = builder.Configuration.GetConnectionString("Redis");

                var options = ConfigurationOptions.Parse(redisConnection!);
                options.AbortOnConnectFail = false;
                options.ConnectRetry = 5;

                return ConnectionMultiplexer.Connect(options);
            });

            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

            // Services
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            builder.Services.AddScoped<ITokenBlocklistService, TokenBlocklistService>();

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Key))
                };
            });

            builder.Services.AddAuthorization();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "POC API",
                    Version = "v1"
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter your JWT token",

                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme,
            new List<string>()
        }
    });
            });

            builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            // Global Exception Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            // HTTPS
            app.UseHttpsRedirection();

            // Auth
            app.UseAuthentication();
            app.UseMiddleware<TokenValidationMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            // Automatic migrations + seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = services.GetRequiredService<AppDbContext>();

                await db.Database.MigrateAsync();

                await DbSeeder.SeedAsync(services);
            }

            app.Run();
        }
    }
}
