using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Utils;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Services
{
    public class RequestEventService : IRequestEventService
    {
        private readonly ProjectCDbContext context;
        private readonly IMapper mapper;
        private readonly IHubContext<WebhookRuleHub> webhookRuleHubContext;

        public RequestEventService(
            ProjectCDbContext context,
            IMapper mapper,
            IHubContext<WebhookRuleHub> webhookRuleHubContext
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.webhookRuleHubContext = webhookRuleHubContext;
        }

        public async Task<IEnumerable<RequestEvent>> GetAsync()
        {
            return await context.RequestEvent
                .Include(x => x.RequestRule)
                .Include(x => x.WebhookRule)
                .Include(x => x.RequestRuleVariant)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByRequestRuleAsync()
        {
            return await context.RequestEvent
                .Include(x => x.RequestRule)
                .Where(x => x.RequestRuleId != null && x.RequestRuleVariantId == null)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByWebhookRuleAsync()
        {
            return await context.RequestEvent
                .Include(x => x.WebhookRule)
                .Where(x => x.WebhookRuleId != null)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByRequestRuleVariantAsync()
        {
            return await context.RequestEvent
                .Include(x => x.RequestRule)
                .Include(x => x.RequestRuleVariant)
                .Where(x => x.RequestRuleVariantId != null)
                .ToArrayAsync();
        }

        public async Task ResendRequestAsync(int requestEventId)
        {
            var requestEvent = await context.RequestEvent
                .Include(x => x.WebhookRule)
                .FirstOrDefaultAsync(x => x.Id == requestEventId);
            if (requestEvent is null)
            {
                throw new Exception("Request event not found");
            }
            if (
                requestEvent.WebhookRule is null
                || string.IsNullOrEmpty(requestEvent.WebhookRule.RedirectUrl)
            )
            {
                throw new Exception("Invalid request event");
            }

            var webhookRequest = RequestUtils.BuildWebhookRequestAsync(
                requestEvent,
                requestEvent.WebhookRule.RedirectUrl
            );
            await webhookRuleHubContext.Clients.All.SendAsync(
                "WebhookRequestToRedirect",
                mapper.Map<WebhookRequestDto>(webhookRequest)
            );
        }

        public async Task DeleteAsync(int id)
        {
            var requestEvent = await context.RequestEvent.FirstOrDefaultAsync(x => x.Id == id);
            if (requestEvent is not null)
            {
                context.RequestEvent.Remove(requestEvent);
                await context.SaveChangesAsync();
            }
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

        public async Task DeleteByRequestRuleVariantAsync()
        {
            var requestEvents = await GetByRequestRuleVariantAsync();

            context.RequestEvent.RemoveRange(requestEvents);
            await context.SaveChangesAsync();
        }
    }
}
