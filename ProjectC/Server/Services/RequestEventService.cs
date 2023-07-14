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
    public class RequestEventService : IRequestEventService
    {
        private readonly ProjectCDbContext context;
        private readonly IMapper mapper;
        private readonly IRequestInspectorService requestInspectorService;
        private readonly IHubContext<WebhookRuleHub> webhookRuleHubContext;

        public RequestEventService(
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

        public async Task<IEnumerable<RequestEvent>> GetAsync()
        {
            return await context.RequestEvent
                .Include(x => x.RequestRule)
                .Include(x => x.WebhookRule)
                .Include(x => x.WorkflowAction)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByRequestRuleAsync()
        {
            return await context.RequestEvent
                .Include(x => x.RequestRule)
                .Where(x => x.RequestRuleId != null && x.WorkflowActionId == null)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByWebhookRuleAsync()
        {
            return await context.RequestEvent
                .Include(x => x.WebhookRule)
                .Where(x => x.WebhookRuleId != null)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<RequestEvent>> GetByWorkflowActionAsync()
        {
            return await context.RequestEvent
                .Include(x => x.RequestRule)
                .Include(x => x.WorkflowAction)
                .Where(x => x.RequestRuleId != null && x.WorkflowActionId != null)
                .ToArrayAsync();
        }

        public async Task ResentRequestAsync(int requestEventId)
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

            var webhookEvent = requestInspectorService.BuildWebhookEventAsync(
                requestEvent,
                requestEvent.WebhookRule.RedirectUrl
            );
            await webhookRuleHubContext.Clients.All.SendAsync(
                "WebhookRuleEventToRedirect",
                mapper.Map<WebhookEventDto>(webhookEvent)
            );
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
