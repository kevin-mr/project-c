using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class WebhookHub : Hub
    {
        public async Task NotifyWebhookRuleEventCaught(RequestEventDto webhookEventDto)
        {
            await Clients.All.SendAsync("WebhookRuleEventCaught", webhookEventDto);
        }

        public async Task NotifyWebhookRuleEventToRedirect(WebhookEventDto webhookEventDto)
        {
            await Clients.All.SendAsync("WebhookRuleEventToRedirect", webhookEventDto);
        }
    }
}
