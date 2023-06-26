using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Services
{
    public class MockServerService : IMockServerService
    {
        public static readonly string MOCK_SERVER_PREFIX = "/mockserver";
        public static readonly string WEBHOOK_PREFIX = "/webhook";
        private readonly ProjectCDbContext context;
        private readonly IHubContext<RequestsHub> requestHubContext;
        private readonly IHubContext<WebhookHub> webhookHubContext;
        private readonly IRequestInspectorService requestInspectorService;

        public MockServerService(
            ProjectCDbContext context,
            IHubContext<RequestsHub> requestHubContext,
            IHubContext<WebhookHub> webhookHubContext,
            IRequestInspectorService requestInspectorService
        )
        {
            this.context = context;
            this.requestHubContext = requestHubContext;
            this.webhookHubContext = webhookHubContext;
            this.requestInspectorService = requestInspectorService;
        }

        public async Task<RequestRule?> FindRequestRule(HttpRequest request)
        {
            if (!request.Path.HasValue)
            {
                return null;
            }

            var method = GetRequestRuleMethod(request.Method);
            var path = request.Path.Value.Remove(0, MOCK_SERVER_PREFIX.Length);

            return await context.RequestRule.FirstOrDefaultAsync(
                x => x.Method == method && x.Path == path
            );
        }

        public async Task<WebhookRule?> FindWebhookRule(HttpRequest request)
        {
            if (!request.Path.HasValue)
            {
                return null;
            }

            var method = GetWebhookRuleMethod(request.Method);
            var path = request.Path.Value.Remove(0, WEBHOOK_PREFIX.Length);

            return await context.WebhookRule.FirstOrDefaultAsync(
                x => x.Method == method && x.Path == path
            );
        }

        public async Task BuildRequestRuleResponse(HttpContext context, RequestRule requestRule)
        {
            await context.Response.WriteAsync(requestRule.ResponseBody);

            var request = await requestInspectorService.BuildRequestAsync(context.Request);
            await requestHubContext.Clients.All.SendAsync("WebhookRuleEventCaught", request);
        }

        public async Task BuildWebhookRuleResponse(HttpContext context, WebhookRule webhookRule)
        {
            var request = await requestInspectorService.BuildRequestAsync(context.Request);
            await webhookHubContext.Clients.All.SendAsync("WebhookRuleEventCaught", request);
            await webhookHubContext.Clients.All.SendAsync(
                "WebhookRuleEventToRedirect",
                new WebhookEventDto
                {
                    Request = context.Request,
                    RedirectUrl = webhookRule.RedirectUrl ?? string.Empty,
                }
            );
        }

        private static RequestRuleMethod GetRequestRuleMethod(string method)
        {
            return method switch
            {
                "GET" => RequestRuleMethod.GET,
                "POST" => RequestRuleMethod.POST,
                "PUT" => RequestRuleMethod.PUT,
                "DELETE" => RequestRuleMethod.DELETE,
                _ => throw new Exception("Invalid Method"),
            };
        }

        private static WebhookRuleMethod GetWebhookRuleMethod(string method)
        {
            return method switch
            {
                "POST" => WebhookRuleMethod.POST,
                "PUT" => WebhookRuleMethod.PUT,
                _ => throw new Exception("Invalid Method"),
            };
        }
    }
}
