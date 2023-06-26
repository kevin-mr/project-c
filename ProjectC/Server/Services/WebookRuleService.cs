using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class WebookRuleService : IWebookRuleService
    {
        private readonly ProjectCDbContext context;

        public WebookRuleService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WebhookRule>> GetAsync()
        {
            return await context.WebhookRule.ToArrayAsync();
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
                context.WebhookRule.Remove(webhookRule);
                await context.SaveChangesAsync();
            }
        }
    }
}
