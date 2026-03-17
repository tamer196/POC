using POC.Application.Events.UserActivity;

namespace POC.KafkaProducer
{
    public class UserActivitySimulator
    {
        private readonly List<(Guid Id, string Name)> _users =
        [
            (Guid.NewGuid(), "Tamer"),
        (Guid.NewGuid(), "Ahmad"),
        (Guid.NewGuid(), "Sara"),
        (Guid.NewGuid(), "Lina"),
        (Guid.NewGuid(), "Omar")
        ];

        private readonly Random _random = new();

        public UserActivityEvent Generate()
        {
            var user = _users[_random.Next(_users.Count)];

            var eventType =
                _random.Next(2) == 0
                ? UserEventType.Login
                : UserEventType.Logout;

            return new UserActivityEvent
            {
                UserId = user.Id,
                UserName = user.Name,
                EventType = eventType,
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}
