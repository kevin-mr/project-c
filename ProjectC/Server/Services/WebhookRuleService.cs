using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class WebhookRuleService : IWebhookRuleService
    {
        private readonly ProjectCDbContext context;

        public WebhookRuleService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WebhookRule>> GetAsync()
        {
            return await context.WebhookRule.ToArrayAsync();
        }

        public async Task<WebhookRule?> GetByIdAsync(int id)
        {
            return await context.WebhookRule
                .Include(x => x.WebhookRuleEvents)
                .Include(x => x.WebhookEvents)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(WebhookRule webhookRule)
        {
            context.WebhookRule.Add(webhookRule);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WebhookRule webhookRule)
        {
            var currentWebhookRule =
                await context.WebhookRule.FirstOrDefaultAsync(x => x.Id == webhookRule.Id)
                ?? throw new Exception("Request rule not found");

            currentWebhookRule.Path = webhookRule.Path;
            currentWebhookRule.Method = webhookRule.Method;
            currentWebhookRule.Description = webhookRule.Description;
            currentWebhookRule.RedirectUrl = webhookRule.RedirectUrl;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var webhookRule = await context.WebhookRule.FirstOrDefaultAsync(x => x.Id == id);
            if (webhookRule is not null)
            {
                var webhookRuleEvents = await context.RequestEvent
                    .Include(x => x.WebhookRule)
                    .Where(x => x.WebhookRuleId != null && x.WebhookRuleId == webhookRule.Id)
                    .ToListAsync();

                using var transaction = context.Database.BeginTransaction();
                try
                {
                    if (webhookRuleEvents != null)
                    {
                        webhookRuleEvents.ForEach(x => x.WebhookRuleId = null);
                        context.RequestEvent.UpdateRange(webhookRuleEvents);
                        await context.SaveChangesAsync();
                    }

                    context.WebhookRule.Remove(webhookRule);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception("Invalid database operation", e);
                }
            }
        }
    }
}
