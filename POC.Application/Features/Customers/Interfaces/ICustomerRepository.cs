using POC.Domain.Entities;

namespace POC.Application.Features.Customers.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
        void Update(Customer customer);
        void Delete(Customer customer);
        Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
