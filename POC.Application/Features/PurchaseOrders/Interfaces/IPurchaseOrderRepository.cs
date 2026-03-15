using POC.Domain.Entities;

namespace POC.Application.Features.PurchaseOrders.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken);

        Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken);

        void Update(PurchaseOrder order);

        void Delete(PurchaseOrder order);

        Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}

