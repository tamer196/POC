using Microsoft.EntityFrameworkCore;
using POC.Application.Features.Warehouses.Interfaces;
using POC.Domain.Entities;

namespace POC.Persistence.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly AppDbContext _db;

        public WarehouseRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _db.Warehouses.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<List<Warehouse>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _db.Warehouses.OrderBy(x => x.Name).ToListAsync(cancellationToken);

        public async Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken = default)
            => await _db.Warehouses.AddAsync(warehouse, cancellationToken);

        public void Update(Warehouse warehouse)
            => _db.Warehouses.Update(warehouse);

        public void Delete(Warehouse warehouse)
            => _db.Warehouses.Remove(warehouse);

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
            => await _db.Warehouses.AnyAsync(x => x.Code == code, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _db.SaveChangesAsync(cancellationToken);
    }
}
