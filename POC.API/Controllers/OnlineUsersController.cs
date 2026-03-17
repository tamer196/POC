using Microsoft.AspNetCore.Mvc;
using POC.Infrastructure.Presence;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnlineUsersController : ControllerBase
    {
        private readonly IOnlineUserStore _onlineUserStore;

        public OnlineUsersController(IOnlineUserStore onlineUserStore)
        {
            _onlineUserStore = onlineUserStore;
        }

        [HttpGet]
        public IActionResult GetOnlineUsers()
        {
            var users = _onlineUserStore.GetAll();

            return Ok(users);
        }
    }
}
