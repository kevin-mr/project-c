using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestInspectorService
    {
        RequestEvent BuildRequestEventAsync(HttpRequest httpRequest, string body);
        WebhookRequest BuildWebhookRequestAsync(
            HttpRequest request,
            string body,
            string redirectUrl
        );
        WebhookRequest BuildWebhookRequestAsync(WebhookEvent webhookEvent, string redirectUrl);
        WebhookRequest BuildWebhookRequestAsync(RequestEvent request, string redirectUrl);
        Task<string> ReadRequestBodyAsync(HttpRequest request);
    }
}
