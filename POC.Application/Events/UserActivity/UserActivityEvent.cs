namespace POC.Application.Events.UserActivity
{
    public sealed class UserActivityEvent
    {
        public Guid UserId { get; init; }

        public string UserName { get; init; } = string.Empty;

        public UserEventType EventType { get; init; }

        public DateTime TimestampUtc { get; init; }
    }
}
