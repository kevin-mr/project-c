using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class WebhookHub : Hub
    {
        public async Task NotifyWebhookRuleEventCaught(RequestDto webhookEventDto)
        {
            await Clients.All.SendAsync("WebhookRuleEventCaught", webhookEventDto);
        }
    }
}
