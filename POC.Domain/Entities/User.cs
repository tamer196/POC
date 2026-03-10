using POC.Domain.Enums;

namespace POC.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;

        public UserRole Role { get; private set; }

        public bool IsActive { get; private set; } = true;

        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

        public void Deactivate() => IsActive = false;

        public void Activate() => IsActive = true;

        public void SetPassword(string hash)
        {
            PasswordHash = hash;
        }

        public void UpdateUsername(string username)
        {
            UserName = username;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void UpdateRole(UserRole role)
        {
            Role = role;
        }

        public void AddRefreshToken(RefreshToken token)
        {
            _refreshTokens.Add(token);
        }

        private User() { }

        public User(string userName, string email, string passwordHash, UserRole role)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public RefreshToken CreateRefreshToken(string token, DateTime expiry, string jti)
        {
            var refreshToken = new RefreshToken(token, expiry, jti, Id);

            _refreshTokens.Add(refreshToken);

            return refreshToken;
        }
    }
}