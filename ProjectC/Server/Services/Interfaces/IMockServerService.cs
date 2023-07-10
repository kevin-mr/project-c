using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IMockServerService
    {
        Task<RequestRule?> FindRequestRuleAsync(HttpRequest httpRequest);
        Task<WebhookRule?> FindWebhookRuleAsync(HttpRequest httpRequest);
        Task<WorkflowAction?> FindWorkflowActionAsync(HttpRequest request, int workflowId);
        Task HandleRequestRuleResponseAsync(HttpContext httpContext, RequestRule requestRule);
        Task HandleWebhookRuleResponseAsync(HttpContext httpContext, WebhookRule requestRule);
        Task HandleWorkflowActionResponseForRequestRuleAsync(
            HttpContext httpContext,
            Workflow workflow,
            WorkflowAction workflowAction
        );
    }
}
