using ProjectC.Shared.Models;

namespace ProjectC.CLI.Services.Interfaces
{
    public interface IWebhookService
    {
        Task RedirectWebhookEventAsync(WebhookEventDto webhookEvent);
    }
}
