using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POC.Domain.Entities;

namespace POC.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
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
        }
    }
}
