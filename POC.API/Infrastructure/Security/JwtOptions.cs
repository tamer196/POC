namespace POC.API.Infrastructure.Security
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int AccessTokenMinutes { get; set; }
        public int RefreshTokenDays { get; set; }
    }
}
