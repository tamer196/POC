using Microsoft.EntityFrameworkCore;
using POC.API.Domain.Entities;

namespace POC.API.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.UserName).IsUnique();
                entity.HasIndex(x => x.Email).IsUnique();

                entity.Property(x => x.UserName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Email)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.Property(x => x.Role)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Token)
                    .IsUnique();

                entity.Property(x => x.Token)
                    .IsRequired();

                entity.Property(x => x.AccessTokenJti)
                    .IsRequired();

                entity.HasIndex(x => x.AccessTokenJti);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.RefreshTokens)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
