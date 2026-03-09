namespace POC.Domain.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
