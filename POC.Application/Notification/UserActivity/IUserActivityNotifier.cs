using POC.Application.Common.Presence;

namespace POC.Application.Notification.UserActivity
{
    public interface IUserActivityNotifier
    {
        Task UserLoggedIn(OnlineUserDto user);
        Task UserLoggedOut(Guid userId);
    }
}
