using POC.Application.Common.Presence;

namespace POC.Infrastructure.Presence
{
    public interface IOnlineUserStore
    {
        IReadOnlyCollection<OnlineUserDto> GetAll();

        void AddOrUpdate(OnlineUserDto user);

        void Remove(Guid userId);

        bool IsOnline(Guid userId);
    }
}
