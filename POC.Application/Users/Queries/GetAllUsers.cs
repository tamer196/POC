using MediatR;
using POC.Application.Common;
using POC.Application.Redis;
using POC.Application.Users.DTO;
using POC.Application.Users.Interfaces;

namespace POC.Application.Users.Queries
{
    public record GetAllUsersQuery() : IRequest<List<UserDto>>;

    public class GetAllUsersHandler
        : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _repository;
        private readonly IRedisCacheService _cache;

        public GetAllUsersHandler(
            IUserRepository repository,
            IRedisCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<UserDto>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var cached = await _cache.GetAsync<List<UserDto>>(Constants.UsersAll);

            if (cached != null)
                return cached;

            var users = await _repository.GetAllAsync(cancellationToken);

            var result = users.Select(x => new UserDto
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Role = x.Role.ToString(),
                IsActive = x.IsActive
            }).ToList();

            await _cache.SetAsync(Constants.UsersAll, result, TimeSpan.FromMinutes(10));

            return result;
        }
    }
}
