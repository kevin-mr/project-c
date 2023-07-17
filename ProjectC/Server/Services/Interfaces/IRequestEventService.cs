using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestEventService
    {
        Task<IEnumerable<RequestEvent>> GetAsync();
        Task<IEnumerable<RequestEvent>> GetByRequestRuleAsync();
        Task<IEnumerable<RequestEvent>> GetByWebhookRuleAsync();
        Task<IEnumerable<RequestEvent>> GetByWorkflowActionAsync();
        Task ResendRequestAsync(int requestEventId);
        Task DeleteAsync(int id);
        Task DeleteByRequestRuleAsync();
        Task DeleteByWebhookRuleAsync();
        Task DeleteByWorkflowActionAsync();
    }
}
