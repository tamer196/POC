using MediatR;
using POC.Application.Users.DTO;

namespace POC.Application.Users.Queries
{
    public record GetAllUsersQuery() : IRequest<List<UserDto>>;
}
