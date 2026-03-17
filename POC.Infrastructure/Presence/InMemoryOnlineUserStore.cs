using POC.Application.Common.Presence;
using System.Collections.Concurrent;

namespace POC.Infrastructure.Presence
{
    public sealed class InMemoryOnlineUserStore : IOnlineUserStore
    {
        private readonly ConcurrentDictionary<Guid, OnlineUserDto> _users = new();

        public IReadOnlyCollection<OnlineUserDto> GetAll()
        {
            return _users.Values.ToList();
        }

        public void AddOrUpdate(OnlineUserDto user)
        {
            _users.AddOrUpdate(
                user.UserId,
                user,
                (_, _) => user);
        }

        public void Remove(Guid userId)
        {
            _users.TryRemove(userId, out _);
        }

        public bool IsOnline(Guid userId)
        {
            return _users.ContainsKey(userId);
        }
    }
}
