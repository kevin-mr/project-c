using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestEventService
    {
        Task<IEnumerable<RequestEvent>> GetByRequestRuleAsync();
        Task<IEnumerable<RequestEvent>> GetByWebhookRuleAsync();
    }
}
