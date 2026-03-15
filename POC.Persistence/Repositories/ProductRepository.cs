using Microsoft.EntityFrameworkCore;
using POC.Application.Features.Products.Interfaces;
using POC.Domain.Entities;

namespace POC.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _db.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _db.Products.OrderBy(x => x.Name).ToListAsync(cancellationToken);

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
            => await _db.Products.AddAsync(product, cancellationToken);

        public void Update(Product product)
            => _db.Products.Update(product);

        public void Delete(Product product)
            => _db.Products.Remove(product);

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken = default)
            => await _db.Products.AnyAsync(x => x.SKU == sku, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _db.SaveChangesAsync(cancellationToken);
    }
}
