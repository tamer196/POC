using POC.Domain.Entities;

namespace POC.Application.Features.Suppliers.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default);
        void Update(Supplier supplier);
        void Delete(Supplier supplier);
        Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
