using MediatR;
using POC.Application.Common;
using POC.Application.Redis;
using POC.Application.Users.DTO;
using POC.Application.Users.Interfaces;
using POC.Domain.Enums;

namespace POC.Application.Users.Commands.Update
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public UserRole Role { get; set; } = default!;
    }

    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IRedisCacheService _cache;

        public UpdateUserCommandHandler(IUserRepository repository, IRedisCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<UserDto> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
                throw new Exception("User not found");

            user.UpdateEmail(request.Email);
            user.UpdateRole(request.Role);
            user.UpdateUsername(request.UserName);

            await _repository.SaveChangesAsync(cancellationToken);
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
