using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IMockServerService
    {
        Task<RequestRule?> FindRequestRule(HttpRequest httpRequest);
        Task<WebhookRule?> FindWebhookRule(HttpRequest httpRequest);
        Task HandleRequestRuleResponse(HttpContext httpContext, RequestRule requestRule);
        Task HandleWebhookRuleResponse(HttpContext httpContext, WebhookRule requestRule);
    }
}
