using MediatR;
using POC.API.Features.Users.DTO;

namespace POC.API.Features.Users.Queries
{
    public record GetAllUsersQuery() : IRequest<List<UserDto>>;
}
