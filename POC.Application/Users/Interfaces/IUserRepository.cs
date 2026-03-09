using POC.Domain.Entities;

namespace POC.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken);
    }
}
