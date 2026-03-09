using MediatR;
using Microsoft.EntityFrameworkCore;
using POC.Application.Redis;
using POC.Application.Users.DTO;
using POC.Persistence;

namespace POC.Application.Users.Queries
{
    public class GetAllUsersHandler
    : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly AppDbContext _db;
        private readonly IRedisCacheService _cache;

        private const string USERS_CACHE_KEY = "users:all";

        public GetAllUsersHandler(AppDbContext db, IRedisCacheService cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var cached = await _cache.GetAsync<List<UserDto>>(USERS_CACHE_KEY);

            if (cached != null)
                return cached;

            var users = await _db.Users
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    Role = x.Role,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            await _cache.SetAsync(USERS_CACHE_KEY, users, TimeSpan.FromMinutes(10));

            return users;
        }
    }
}
