using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class RequestsHub : Hub
    {
        public async Task NotifyRequestRuleEventCaught(RequestEventDto requestRuleEvent)
        {
            await Clients.All.SendAsync("RequestRuleEventCaught", requestRuleEvent);
        }
    }
}
