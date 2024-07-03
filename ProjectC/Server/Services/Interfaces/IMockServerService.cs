using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IMockServerService
    {
        Task<RequestRule?> FindRequestRuleAsync(HttpRequest httpRequest);
        Task<WebhookRule?> FindWebhookRuleAsync(HttpRequest httpRequest);
        Task<RequestRuleVariant?> FindRequestRuleVariantAsync(HttpRequest request, int workflowId);
        Task HandleRequestRuleResponseAsync(HttpContext httpContext, RequestRule requestRule);
        Task HandleWebhookRuleResponseAsync(HttpContext httpContext, WebhookRule requestRule);
        Task HandleRequestRuleVariantResponseForRequestRuleAsync(
            HttpContext httpContext,
            Workflow workflow,
            RequestRuleVariant requestRuleVariant,
            bool storage
        );
    }
}
