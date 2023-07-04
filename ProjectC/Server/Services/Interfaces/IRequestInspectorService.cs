using ProjectC.Shared.Models;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestInspectorService
    {
        Task<RequestDto> BuildRequestAsync(HttpRequest httpRequest);
        Task<WebhookEventDto> BuildWebhookEventAsync(HttpRequest request, string redirectUrl);
    }
}
