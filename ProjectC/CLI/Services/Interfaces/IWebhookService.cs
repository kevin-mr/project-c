using ProjectC.Shared.Models;

namespace ProjectC.CLI.Services.Interfaces
{
    public interface IWebhookService
    {
        Task RedirectWebhookRequestAsync(WebhookRequestDto webhookRequest);
    }
}
