using MediatR;
using POC.Application.Users.DTO;
using POC.Application.Users.Interfaces;

namespace POC.Application.Users.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;

    public class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _repository;

        public GetUserByIdQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

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
