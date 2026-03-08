namespace POC.API.Features.Auth.Logout
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
