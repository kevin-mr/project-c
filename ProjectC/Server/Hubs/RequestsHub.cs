using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class RequestsHub : Hub
    {
        public async Task SendMessage(RequestDto request)
        {
            await Clients.All.SendAsync("NewRequestCaught", request);
        }
    }
}
