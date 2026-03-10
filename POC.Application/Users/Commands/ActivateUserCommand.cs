using MediatR;
using POC.Application.Common;
using POC.Application.Redis;
using POC.Application.Users.Interfaces;

namespace POC.Application.Users.Commands
{
    public record ActivateUserCommand(Guid Id) : IRequest;

    public class ActivateUserCommandHandler
        : IRequestHandler<ActivateUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IRedisCacheService _cache;

        public ActivateUserCommandHandler(IUserRepository repository, IRedisCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task Handle(
            ActivateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            user.Activate();

            await _repository.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(Constants.UsersAll);
        }
    }
}
