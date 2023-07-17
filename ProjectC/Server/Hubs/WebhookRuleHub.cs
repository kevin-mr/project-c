using Microsoft.AspNetCore.SignalR;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Hubs
{
    public class WebhookRuleHub : Hub
    {
        public async Task NotifyWebhookRuleEventCaught(RequestEventDto webhookRequestDto)
        {
            await Clients.All.SendAsync("WebhookRuleEventCaught", webhookRequestDto);
        }

        public async Task NotifyWebhookRequestToRedirect(WebhookRequestDto webhookRequestDto)
        {
            await Clients.All.SendAsync("WebhookRequestToRedirect", webhookRequestDto);
        }
    }
}
