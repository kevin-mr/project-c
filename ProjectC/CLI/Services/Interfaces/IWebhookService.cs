using ProjectC.Shared.Models;

namespace ProjectC.CLI.Services.Interfaces
{
    public interface IWebhookService
    {
        void RedirectWebhookEvent(WebhookEventDto webhookEventDto);
    }
}
