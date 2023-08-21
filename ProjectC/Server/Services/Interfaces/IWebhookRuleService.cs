using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IWebhookRuleService
    {
        Task<IEnumerable<WebhookRule>> GetAsync();
        Task<WebhookRule?> GetByIdAsync(int id);
        Task CreateAsync(WebhookRule webhookRule);
        Task UpdateAsync(WebhookRule webhookRule);
        Task DeleteAsync(int id);
    }
}
