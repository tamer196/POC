using Microsoft.EntityFrameworkCore;
using POC.Application.Features.Customers.Interfaces;
using POC.Domain.Entities;

namespace POC.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Customers.OrderBy(x => x.Name).ToListAsync(cancellationToken);

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
            => await _dbContext.Customers.AddAsync(customer, cancellationToken);

        public void Update(Customer customer)
            => _dbContext.Customers.Update(customer);

        public void Delete(Customer customer)
            => _dbContext.Customers.Remove(customer);

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
            => await _dbContext.Customers.AnyAsync(x => x.Code == code, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
