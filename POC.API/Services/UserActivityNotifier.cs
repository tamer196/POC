using Microsoft.AspNetCore.SignalR;
using POC.API.Hubs;
using POC.Application.Common.Presence;
using POC.Application.Notification.UserActivity;

namespace POC.API.Services
{
    public class UserActivityNotifier : IUserActivityNotifier
    {
        private readonly IHubContext<UserActivityHub> _hub;

        public UserActivityNotifier(IHubContext<UserActivityHub> hub)
        {
            _hub = hub;
        }

        public Task UserLoggedIn(OnlineUserDto user)
        {
            return _hub.Clients.All.SendAsync("UserLoggedIn", user);
        }

        public Task UserLoggedOut(Guid userId)
        {
            return _hub.Clients.All.SendAsync("UserLoggedOut", userId);
        }
    }
}
