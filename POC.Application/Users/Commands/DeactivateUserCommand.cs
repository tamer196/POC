using MediatR;
using POC.Application.Common;
using POC.Application.Redis;
using POC.Application.Users.Interfaces;

namespace POC.Application.Users.Commands
{
    public record DeactivateUserCommand(Guid Id) : IRequest;

    public class DeactivateUserCommandHandler
        : IRequestHandler<DeactivateUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IRedisCacheService _cache;

        public DeactivateUserCommandHandler(IUserRepository repository, IRedisCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task Handle(
            DeactivateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            user.Deactivate();

            await _repository.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(Constants.UsersAll);
        }
    }
}
