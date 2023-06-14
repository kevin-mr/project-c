using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services;

namespace ProjectC.Server.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHubContext<RequestHistoryHub> _hubContext;
        private readonly IRequestInspectorService _requestInspectorService;

        public TestController(
            IHubContext<RequestHistoryHub> hubContext,
            IRequestInspectorService requestInspectorService
        )
        {
            _hubContext = hubContext;
            _requestInspectorService = requestInspectorService;
        }

        [HttpPost("test/content")]
        public async Task TestRequestContent()
        {
            var request = await _requestInspectorService.BuildRequestAsync(Request);
            await _hubContext.Clients.All.SendAsync("NewRequestCaught", request);
        }
    }
}
