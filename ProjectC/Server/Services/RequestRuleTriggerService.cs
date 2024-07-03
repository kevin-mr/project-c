using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class RequestRuleTriggerService : IRequestRuleTriggerService
    {
        private readonly ProjectCDbContext context;

        public RequestRuleTriggerService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RequestRuleTrigger>> GetAsync()
        {
            return await context.RequestRuleTrigger.Include(x => x.WebhookEvent).ToArrayAsync();
        }

        public async Task<IEnumerable<RequestRuleTrigger>> GetByRequestRuleVariantIdAsync(
            int requestRuleVariantId
        )
        {
            return await context.RequestRuleTrigger
                .Where(x => x.RequestRuleVariantId == requestRuleVariantId)
                .Include(x => x.WebhookEvent)
                .ToArrayAsync();
        }

        public async Task CreateAsync(RequestRuleTrigger requestRuleTrigger)
        {
            context.RequestRuleTrigger.Add(requestRuleTrigger);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RequestRuleTrigger requestRuleTrigger)
        {
            var currentRequestRuleTrigger =
                await context.RequestRuleTrigger.FirstOrDefaultAsync(
                    x => x.Id == requestRuleTrigger.Id
                ) ?? throw new Exception("Request rule trigger not found");

            currentRequestRuleTrigger.Description = requestRuleTrigger.Description;
            currentRequestRuleTrigger.WebhookEventId = requestRuleTrigger.WebhookEventId;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var requestRuleTrigger = await context.RequestRuleTrigger.FirstOrDefaultAsync(
                x => x.Id == id
            );
            if (requestRuleTrigger is not null)
            {
                context.RequestRuleTrigger.Remove(requestRuleTrigger);
                await context.SaveChangesAsync();
            }
        }
    }
}
