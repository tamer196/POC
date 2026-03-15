using POC.Domain.Entities;

namespace POC.Application.Features.Warehouses.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Warehouse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken = default);
        void Update(Warehouse warehouse);
        void Delete(Warehouse warehouse);
        Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
