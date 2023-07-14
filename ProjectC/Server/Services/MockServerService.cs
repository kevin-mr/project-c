using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectC.Client.Pages;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace ProjectC.Server.Services
{
    public class MockServerService : IMockServerService
    {
        public static readonly string MOCK_SERVER_PREFIX = "/mockserver";
        public static readonly string WEBHOOK_PREFIX = "/webhook";
        private readonly ProjectCDbContext context;
        private readonly IHubContext<RequestRuleHub> requestHubContext;
        private readonly IHubContext<WebhookRuleHub> webhookHubContext;
        private readonly IHubContext<WorkflowActionHub> workflowHubContext;
        private readonly IRequestInspectorService requestInspectorService;
        private readonly IMapper mapper;

        public MockServerService(
            ProjectCDbContext context,
            IHubContext<RequestRuleHub> requestHubContext,
            IHubContext<WebhookRuleHub> webhookHubContext,
            IHubContext<WorkflowActionHub> workflowHubContext,
            IRequestInspectorService requestInspectorService,
            IMapper mapper
        )
        {
            this.context = context;
            this.requestHubContext = requestHubContext;
            this.webhookHubContext = webhookHubContext;
            this.workflowHubContext = workflowHubContext;
            this.requestInspectorService = requestInspectorService;
            this.mapper = mapper;
        }

        public async Task<RequestRule?> FindRequestRuleAsync(HttpRequest request)
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

        public async Task<WebhookRule?> FindWebhookRuleAsync(HttpRequest request)
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

        public async Task<WorkflowAction?> FindWorkflowActionAsync(
            HttpRequest request,
            int workflowId
        )
        {
            if (!request.Path.HasValue)
            {
                return null;
            }

            var method = GetRequestRuleMethod(request.Method);
            var path = request.Path.Value.Remove(0, MOCK_SERVER_PREFIX.Length);

            return await context.WorkflowAction
                .Include(x => x.RequestRule)
                .FirstOrDefaultAsync(
                    x =>
                        x.RequestRule != null
                        && x.RequestRule.Method == method
                        && x.RequestRule.Path == path
                        && x.WorkflowId == workflowId
                );
        }

        public async Task HandleRequestRuleResponseAsync(
            HttpContext httpContext,
            RequestRule requestRule
        )
        {
            if (requestRule.ResponseDelay > 0)
            {
                Thread.Sleep(requestRule.ResponseDelay);
            }
            httpContext.Response.StatusCode = requestRule.ResponseStatus;
            SetHeaders(httpContext, requestRule.ResponseHeaders);
            await httpContext.Response.WriteAsync(requestRule.ResponseBody);

            var requestBody = await requestInspectorService.ReadRequestBodyAsync(
                httpContext.Request
            );
            var requestEvent = requestInspectorService.BuildRequestEventAsync(
                httpContext.Request,
                requestBody
            );
            if (requestEvent is not null)
            {
                requestEvent.RequestRuleId = requestRule.Id;
                context.RequestEvent.Add(requestEvent);
                await context.SaveChangesAsync();

                var requestEventDto = mapper.Map<RequestEventDto>(requestEvent);
                requestEventDto.Path = requestRule.Path;
                await requestHubContext.Clients.All.SendAsync(
                    "RequestRuleEventCaught",
                    requestEventDto
                );
            }
        }

        public async Task HandleWorkflowActionResponseForRequestRuleAsync(
            HttpContext httpContext,
            Workflow workflow,
            WorkflowAction workflowAction
        )
        {
            if (workflowAction.RequestRule is null)
            {
                return;
            }

            if (workflowAction.ResponseDelay > 0 || workflowAction.RequestRule.ResponseDelay > 0)
            {
                Thread.Sleep(
                    workflowAction.ResponseDelay ?? workflowAction.RequestRule.ResponseDelay
                );
            }

            httpContext.Response.StatusCode =
                workflowAction.ResponseStatus ?? workflowAction.RequestRule.ResponseStatus;

            await httpContext.Response.WriteAsync(
                workflowAction.ResponseBody ?? workflowAction.RequestRule.ResponseBody
            );

            var requestBody = await requestInspectorService.ReadRequestBodyAsync(
                httpContext.Request
            );
            var requestEvent = requestInspectorService.BuildRequestEventAsync(
                httpContext.Request,
                requestBody
            );
            if (requestEvent is not null)
            {
                requestEvent.WorkflowActionId = workflowAction.Id;
                requestEvent.RequestRuleId = workflowAction.RequestRule.Id;
                context.RequestEvent.Add(requestEvent);
                await context.SaveChangesAsync();

                var requestEventDto = mapper.Map<RequestEventDto>(requestEvent);
                requestEventDto.Path = workflowAction.RequestRule.Path;
                requestEventDto.WorkflowActionName = workflowAction.Name;
                await workflowHubContext.Clients.All.SendAsync(
                    "WorkflowActionEventCaught",
                    requestEventDto
                );
            }
        }

        public async Task HandleWebhookRuleResponseAsync(
            HttpContext httpContext,
            WebhookRule webhookRule
        )
        {
            var requestBody = await requestInspectorService.ReadRequestBodyAsync(
                httpContext.Request
            );
            var requestEvent = requestInspectorService.BuildRequestEventAsync(
                httpContext.Request,
                requestBody
            );
            if (requestEvent is not null)
            {
                requestEvent.WebhookRuleId = webhookRule.Id;
                context.RequestEvent.Add(requestEvent);
                await context.SaveChangesAsync();

                await webhookHubContext.Clients.All.SendAsync(
                    "WebhookRuleEventCaught",
                    mapper.Map<RequestEventDto>(requestEvent)
                );
                if (!string.IsNullOrEmpty(webhookRule.RedirectUrl))
                {
                    var webhookEvent = requestInspectorService.BuildWebhookEventAsync(
                        httpContext.Request,
                        requestBody,
                        webhookRule.RedirectUrl
                    );
                    var requestEventDto = mapper.Map<WebhookEventDto>(webhookEvent);
                    requestEventDto.Path = webhookEvent.Path;
                    requestEventDto.RedirectUrl = webhookEvent.RedirectUrl;
                    await webhookHubContext.Clients.All.SendAsync(
                        "WebhookRuleEventToRedirect",
                        requestEventDto
                    );
                }
            }
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

        private void SetHeaders(HttpContext httpContext, string jsonHeaders)
        {
            try
            {
                var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonHeaders);
                if (headers is not null)
                {
                    foreach (var header in headers)
                    {
                        httpContext.Response.Headers.Add(header.Key, header.Value);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
