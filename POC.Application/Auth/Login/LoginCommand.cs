using MediatR;
using POC.Application.Security;
using POC.Application.Services;
using POC.Application.Users.Interfaces;

namespace POC.Application.Auth.Login
{
    public record LoginCommand(string UserName, string Password) : IRequest<LoginResponse>;

    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(
                request.UserName,
                cancellationToken);

            if (user is null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            {
                return new LoginResponse
                {
                    Success = false,
                    UserName = request.UserName,
                    Error = "Invalid username or password"
                };
            }

            var jti = Guid.NewGuid().ToString();

            var accessToken = _jwtService.GenerateAccessToken(user, jti);

            var refreshToken = await _refreshTokenService.CreateAsync(user, jti);

            return new LoginResponse
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }
    }
}


