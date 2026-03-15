using Microsoft.EntityFrameworkCore;
using POC.Application.Features.Suppliers.Interfaces;
using POC.Domain.Entities;

namespace POC.Persistence.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _db;

        public SupplierRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _db.Suppliers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _db.Suppliers.OrderBy(x => x.Name).ToListAsync(cancellationToken);

        public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default)
            => await _db.Suppliers.AddAsync(supplier, cancellationToken);

        public void Update(Supplier supplier)
            => _db.Suppliers.Update(supplier);

        public void Delete(Supplier supplier)
            => _db.Suppliers.Remove(supplier);

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
            => await _db.Suppliers.AnyAsync(x => x.Code == code, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _db.SaveChangesAsync(cancellationToken);
    }
}
