namespace POC.API.Features.Auth.Login
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
