using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POC.Application.Users.Interfaces;
using POC.Persistence.Repositories;

namespace POC.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
