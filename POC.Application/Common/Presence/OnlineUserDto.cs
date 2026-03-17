namespace POC.Application.Common.Presence
{
    public sealed class OnlineUserDto
    {
        public Guid UserId { get; init; }

        public string UserName { get; init; } = string.Empty;

        public DateTime LastActivityUtc { get; init; }
    }
}
