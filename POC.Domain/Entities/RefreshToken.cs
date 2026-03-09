namespace POC.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string AccessTokenJti { get; set; } = default!;
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAtUtc { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; } = default!;
    }
}
