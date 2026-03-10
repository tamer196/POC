using MediatR;
using POC.Application.Common;
using POC.Application.Redis;
using POC.Application.Security;
using POC.Application.Users.DTO;
using POC.Application.Users.Interfaces;
using POC.Domain.Entities;
using POC.Domain.Enums;

namespace POC.Application.Users.Commands.Create
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public UserRole Role { get; set; } = default!;
    }

    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IRedisCacheService _cache;

        public CreateUserCommandHandler(IUserRepository repository, IRedisCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<UserDto> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = new User(
                request.UserName,
                request.Email,
                PasswordHasher.Hash(request.Password),
                request.Role);

            await _repository.AddAsync(user, cancellationToken);
            await _cache.RemoveAsync(Constants.UsersAll);

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive
            };
        }
    }
}
