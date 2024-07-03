using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class RequestRuleVariantHub : Hub
    {
        public async Task NotifyRequestRuleVariantEventCaught(
            RequestEventDto requestRuleVariantEventDto
        )
        {
            await Clients.All.SendAsync(
                "RequestRuleVariantEventCaught",
                requestRuleVariantEventDto
            );
        }
    }
}
