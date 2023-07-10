using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class RequestEventService : IRequestEventService
    {
        private readonly ProjectCDbContext context;

        public RequestEventService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RequestEvent>> GetByRequestRuleAsync()
        {
            return await context.RequestEvent
                .Where(x => x.RequestRuleId != null && !x.ForWorkflowAction)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByWebhookRuleAsync()
        {
            return await context.RequestEvent.Where(x => x.WebhookRuleId != null).ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByWorkflowActionAsync()
        {
            return await context.RequestEvent
                .Where(x => x.RequestRuleId != null && x.ForWorkflowAction)
                .ToArrayAsync();
        }

        public async Task DeleteByRequestRuleAsync()
        {
            var requestEvents = await GetByRequestRuleAsync();

            context.RequestEvent.RemoveRange(requestEvents);
            await context.SaveChangesAsync();
        }

        public async Task DeleteByWebhookRuleAsync()
        {
            var requestEvents = await GetByWebhookRuleAsync();

            context.RequestEvent.RemoveRange(requestEvents);
            await context.SaveChangesAsync();
        }

        public async Task DeleteByWorkflowActionAsync()
        {
            var requestEvents = await GetByWorkflowActionAsync();

            context.RequestEvent.RemoveRange(requestEvents);
            await context.SaveChangesAsync();
        }
    }
}
