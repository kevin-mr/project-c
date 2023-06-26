using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IWebookRuleService
    {
        Task<IEnumerable<WebhookRule>> GetAsync();
        Task CreateAsync(WebhookRule webhookRule);
        Task UpdateAsync(WebhookRule webhookRule);
        Task DeleteAsync(int id);
    }
}
