using Microsoft.EntityFrameworkCore;
using POC.Application.Users.Interfaces;
using POC.Domain.Entities;

namespace POC.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x =>
                    x.UserName == username &&
                    x.IsActive,
                    cancellationToken);
        }
    }
}
