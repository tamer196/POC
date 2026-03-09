using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POC.Domain.Entities;
using POC.Domain.Enums;
using POC.Persistence.Security;

namespace POC.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync();

            if (await dbContext.Users.AnyAsync())
                return;

            var users = new List<AppUser>
        {
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = "admin@test.com",
                PasswordHash = PasswordHasher.Hash("Admin@123"),
                Role = AppRole.Admin,
                IsActive = true
            },
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "manager",
                Email = "manager@test.com",
                PasswordHash = PasswordHasher.Hash("Manager@123"),
                Role = AppRole.Manager,
                IsActive = true
            },
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "user",
                Email = "user@test.com",
                PasswordHash = PasswordHasher.Hash("User@123"),
                Role = AppRole.User,
                IsActive = true
            }
        };

            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
        }
    }
}

