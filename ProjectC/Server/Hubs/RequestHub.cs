using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class RequestHub : Hub
    {
        public async Task NotifyRequestEventCaught(RequestEventDto requestEvent)
        {
            await Clients.All.SendAsync("RequestEventCaught", requestEvent);
        }
    }
}
