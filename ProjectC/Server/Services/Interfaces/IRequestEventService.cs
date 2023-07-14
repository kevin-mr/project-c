using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestEventService
    {
        Task<IEnumerable<RequestEvent>> GetAsync();
        Task<IEnumerable<RequestEvent>> GetByRequestRuleAsync();
        Task<IEnumerable<RequestEvent>> GetByWebhookRuleAsync();
        Task<IEnumerable<RequestEvent>> GetByWorkflowActionAsync();
        Task ResentRequestAsync(int requestEventId);
        Task DeleteByRequestRuleAsync();
        Task DeleteByWebhookRuleAsync();
        Task DeleteByWorkflowActionAsync();
    }
}
