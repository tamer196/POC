using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POC.Application.Security;
using POC.Domain.Entities;
using POC.Domain.Enums;

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

            var users = new List<User>
            {
                new User("admin", "admin@test.com", PasswordHasher.Hash("Admin@123"), UserRole.Admin),
                new User("manager", "manager@test.com", PasswordHasher.Hash("Manager@123"), UserRole.Manager),
                new User("user", "user@test.com", PasswordHasher.Hash("User@123"), UserRole.User)
            };

            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
        }
    }
}

