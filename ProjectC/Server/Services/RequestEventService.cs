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
            return await context.RequestEvent.Where(x => x.RequestRuleId != null).ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByWebhookRuleAsync()
        {
            return await context.RequestEvent.Where(x => x.WebhookRuleId != null).ToArrayAsync();
        }
    }
}
