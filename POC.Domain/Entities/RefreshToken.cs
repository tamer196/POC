namespace POC.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = default!;

        public DateTime ExpiresAtUtc { get; set; }

        public string AccessTokenJti { get; set; } = default!;

        public bool IsRevoked { get; private set; }

        public DateTime? RevokedAtUtc { get; private set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;

        public bool IsActive => !IsRevoked && ExpiresAtUtc > DateTime.UtcNow;

        public void Revoke()
        {
            IsRevoked = true;
            RevokedAtUtc = DateTime.UtcNow;
        }

        public RefreshToken(string token, DateTime expiresAtUtc, string accessTokenJti, Guid userId)
        {
            Token = token;
            ExpiresAtUtc = expiresAtUtc;
            AccessTokenJti = accessTokenJti;
            UserId = userId;
        }
    }
}
