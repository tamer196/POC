namespace POC.Application.Auth.Refresh
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
