using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POC.Domain.Entities;

namespace POC.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.SKU).IsUnique();

            entity.Property(x => x.SKU).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500).IsRequired();
            entity.Property(x => x.CostPrice).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.SellingPrice).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Status).HasConversion<string>().IsRequired();

            entity.HasOne(x => x.Supplier)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Warehouse)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
