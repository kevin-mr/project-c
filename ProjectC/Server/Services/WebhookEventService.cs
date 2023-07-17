using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Services
{
    public class WebhookEventService : IWebhookEventService
    {
        private readonly ProjectCDbContext context;
        private readonly IMapper mapper;
        private readonly IRequestInspectorService requestInspectorService;
        private readonly IHubContext<WebhookRuleHub> webhookRuleHubContext;

        public WebhookEventService(
            ProjectCDbContext context,
            IMapper mapper,
            IRequestInspectorService requestInspectorService,
            IHubContext<WebhookRuleHub> webhookRuleHubContext
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.requestInspectorService = requestInspectorService;
            this.webhookRuleHubContext = webhookRuleHubContext;
        }

        public async Task<IEnumerable<WebhookEvent>> GetAsync()
        {
            return await context.WebhookEvents.Include(x => x.WebhookRule).ToArrayAsync();
        }

        public async Task<IEnumerable<WebhookEvent>> GetByWebhookRuleIdAsync(int id)
        {
            return await context.WebhookEvents
                .Include(x => x.WebhookRule)
                .Where(x => x.WebhookRuleId == id)
                .ToArrayAsync();
        }

        public async Task CopyAndSaveFromRequestEventAsync(int requestEventId)
        {
            var requestEvent = await context.RequestEvent.FirstOrDefaultAsync(
                x => x.Id == requestEventId
            );
            if (requestEvent is null)
            {
                throw new Exception("Request event not found");
            }

            var webhookEvent = mapper.Map<WebhookEvent>(requestEvent);

            context.WebhookEvents.Add(webhookEvent);
            await context.SaveChangesAsync();
        }

        public async Task ResendAsync(int id)
        {
            var webhookEvent = await context.WebhookEvents
                .Include(x => x.WebhookRule)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (webhookEvent is null)
            {
                throw new Exception("Webhook event not found");
            }
            if (
                webhookEvent.WebhookRule is null
                || string.IsNullOrEmpty(webhookEvent.WebhookRule.RedirectUrl)
            )
            {
                throw new Exception("Invalid webhook event");
            }

            var webhookRequest = requestInspectorService.BuildWebhookRequestAsync(
                webhookEvent,
                webhookEvent.WebhookRule.RedirectUrl
            );
            await webhookRuleHubContext.Clients.All.SendAsync(
                "WebhookRequestToRedirect",
                mapper.Map<WebhookRequestDto>(webhookRequest)
            );
        }

        public async Task DeleteAsync(int id)
        {
            var webhookEvent = await context.WebhookEvents.FirstOrDefaultAsync(x => x.Id == id);
            if (webhookEvent is not null)
            {
                context.WebhookEvents.Remove(webhookEvent);
                await context.SaveChangesAsync();
            }
        }
    }
}
