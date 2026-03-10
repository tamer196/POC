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

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _db.Users.AddAsync(user, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return;

            _db.Users.Remove(user);

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
