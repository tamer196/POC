using MediatR;
using POC.Application.Common;
using POC.Application.Redis;
using POC.Application.Users.Interfaces;

namespace POC.Application.Users.Commands
{
    public record DeleteUserCommand(Guid Id) : IRequest;

    public class DeleteUserCommandHandler
        : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IRedisCacheService _cache;

        public DeleteUserCommandHandler(IUserRepository repository, IRedisCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            await _cache.RemoveAsync(Constants.UsersAll);
        }
    }
}
