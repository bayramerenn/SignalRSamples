using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.API.Hubs;
using System.Threading.Tasks;

namespace SignalRSample.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<MyHub> _hubContext;

        public NotificationController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("{teamCount}")]
        public async Task<IActionResult> Index(int teamCount)
        {
            await _hubContext.Clients.All.SendAsync("Notify", $"Arkadaslar takim {teamCount} kisi olacaktir.");
            return Ok();
        }
    }
}
