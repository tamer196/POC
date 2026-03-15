using Microsoft.EntityFrameworkCore;
using POC.Application.Features.PurchaseOrders.Interfaces;
using POC.Domain.Entities;

namespace POC.Persistence.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly AppDbContext _db;

        public PurchaseOrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.PurchaseOrders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.PurchaseOrders
                .Include(x => x.Items)
                .OrderByDescending(x => x.OrderDateUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken)
        {
            await _db.PurchaseOrders.AddAsync(order, cancellationToken);
        }

        public void Update(PurchaseOrder order)
        {
            _db.PurchaseOrders.Update(order);
        }

        public void Delete(PurchaseOrder order)
        {
            _db.PurchaseOrders.Remove(order);
        }

        public async Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
        {
            return await _db.PurchaseOrders
                .AnyAsync(x => x.OrderNumber == orderNumber, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
