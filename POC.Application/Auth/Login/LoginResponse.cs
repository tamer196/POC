using POC.Domain.Enums;

namespace POC.Application.Auth.Login
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public string? UserName { get; set; }

        public string? Role { get; set; }

        public string? Error { get; set; }
    }
}
