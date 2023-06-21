using Azure.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class MockServerService : IMockServerService
    {
        public static readonly string CUSTOM_PREFIX = "/custom";
        private readonly ProjectCDbContext context;
        private readonly IHubContext<RequestsHub> hubContext;
        private readonly IRequestInspectorService requestInspectorService;

        public MockServerService(
            ProjectCDbContext context,
            IHubContext<RequestsHub> hubContext,
            IRequestInspectorService requestInspectorService
        )
        {
            this.context = context;
            this.hubContext = hubContext;
            this.requestInspectorService = requestInspectorService;
        }

        public async Task<RequestRule?> FindRequestRule(HttpRequest request)
        {
            if (!request.Path.HasValue)
            {
                return null;
            }

            var method = GetRequestMethod(request.Method);
            var path = request.Path.Value.Remove(0, CUSTOM_PREFIX.Length);

            return await context.RequestRule.FirstOrDefaultAsync(
                x => x.Method == method && x.Path == path
            );
        }

        public async Task BuildRequestRuleResponse(HttpContext context, RequestRule requestRule)
        {
            await context.Response.WriteAsync(requestRule.ResponseBody);

            var request = await requestInspectorService.BuildRequestAsync(context.Request);
            await hubContext.Clients.All.SendAsync("NewRequestCaught", request);
        }

        private static RequestRuleMethod GetRequestMethod(string method)
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
    }
}
