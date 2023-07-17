using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IWebhookEventService
    {
        Task<IEnumerable<WebhookEvent>> GetByWebhookRuleIdAsync(int id);
        Task CopyAndSaveFromRequestEventAsync(int requestEventId);
        Task ResendAsync(int id);
        Task DeleteAsync(int id);
    }
}
