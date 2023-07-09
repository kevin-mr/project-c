using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestInspectorService
    {
        Task<RequestEvent> BuildRequestEventAsync(HttpRequest httpRequest);
        Task<WebhookEvent> BuildWebhookEventAsync(HttpRequest request, string redirectUrl);
    }
}
