using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectC.Client.Pages;
using ProjectC.Server.BackgroudServices;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Hubs;
using ProjectC.Server.Models;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Utils;
using ProjectC.Shared.Models;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProjectC.Server.Services
{
    public class MockServerService : IMockServerService
    {
        public static readonly string MOCK_SERVER_PREFIX = "/mockserver";
        public static readonly string WEBHOOK_PREFIX = "/webhook";
        private readonly ProjectCDbContext context;
        private readonly IHubContext<RequestHub> requestHubContext;
        private readonly IHubContext<RequestRuleHub> requestRuleHubContext;
        private readonly IHubContext<WebhookRuleHub> webhookRuleHubContext;
        private readonly IHubContext<WorkflowActionHub> workflowActionHubContext;
        private readonly IWorkflowTriggerService workflowTriggerService;
        private readonly WebhookEventQueue workflowTriggersQueue;
        private readonly IMapper mapper;

        public MockServerService(
            ProjectCDbContext context,
            IHubContext<RequestHub> requestHubContext,
            IHubContext<RequestRuleHub> requestRuleHubContext,
            IHubContext<WebhookRuleHub> webhookRuleHubContext,
            IHubContext<WorkflowActionHub> workflowActionHubContext,
            IWorkflowTriggerService workflowTriggerService,
            WebhookEventQueue workflowTriggersQueue,
            IMapper mapper
        )
        {
            this.context = context;
            this.requestHubContext = requestHubContext;
            this.requestRuleHubContext = requestRuleHubContext;
            this.webhookRuleHubContext = webhookRuleHubContext;
            this.workflowActionHubContext = workflowActionHubContext;
            this.workflowTriggerService = workflowTriggerService;
            this.workflowTriggersQueue = workflowTriggersQueue;
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

            var requestRules = await context.RequestRule
                .Select(
                    x =>
                        new
                        {
                            x.Id,
                            x.PathRegex,
                            x.Method
                        }
                )
                .ToArrayAsync();
            var requestRule = requestRules.FirstOrDefault(
                x => x.Method == method && Regex.IsMatch(path, x.PathRegex)
            );
            return requestRule is null
                ? null
                : await context.RequestRule.FirstOrDefaultAsync(x => x.Id == requestRule.Id);
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

            var workflowActions = await context.WorkflowAction
                .Include(x => x.RequestRule)
                .Where(x => x.WorkflowId == workflowId)
                .Select(
                    x =>
                        new
                        {
                            x.Id,
                            PathRegex = x.RequestRule != null
                                ? x.RequestRule.PathRegex
                                : x.PathRegex!,
                            Method = x.RequestRule != null ? x.RequestRule.Method : x.Method,
                        }
                )
                .ToArrayAsync();
            var workflowAction = workflowActions.FirstOrDefault(
                x => x.Method == method && Regex.IsMatch(path, x.PathRegex)
            );

            return workflowAction is null
                ? null
                : await context.WorkflowAction
                    .Include(x => x.RequestRule)
                    .FirstOrDefaultAsync(x => x.Id == workflowAction.Id);
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

            var requestBody = await RequestUtils.ReadRequestBodyAsync(httpContext.Request);
            var requestEvent = RequestUtils.BuildRequestEventAsync(
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
                await requestRuleHubContext.Clients.All.SendAsync(
                    "RequestRuleEventCaught",
                    requestEventDto
                );
                await requestHubContext.Clients.All.SendAsync(
                    "RequestEventCaught",
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
            if (workflowAction.ResponseDelay is not null)
            {
                Thread.Sleep(workflowAction.ResponseDelay.Value);
            }
            httpContext.Response.StatusCode = workflowAction.ResponseStatus ?? 500;
            await httpContext.Response.WriteAsync(workflowAction.ResponseBody ?? string.Empty);

            var requestBody = await RequestUtils.ReadRequestBodyAsync(httpContext.Request);
            var requestEvent = RequestUtils.BuildRequestEventAsync(
                httpContext.Request,
                requestBody
            );
            if (requestEvent is not null)
            {
                requestEvent.WorkflowActionId = workflowAction.Id;
                if (workflowAction.RequestRule is not null)
                {
                    requestEvent.RequestRuleId = workflowAction.RequestRule.Id;
                }
                context.RequestEvent.Add(requestEvent);
                await context.SaveChangesAsync();

                var requestEventDto = mapper.Map<RequestEventDto>(requestEvent);
                if (workflowAction.RequestRule is not null)
                {
                    requestEventDto.Path = workflowAction.RequestRule.Path;
                }
                else
                {
                    requestEventDto.Path =
                        workflowAction.Path ?? throw new Exception("Invalid Requets Path");
                }
                requestEventDto.WorkflowActionDescription = workflowAction.Description;
                await workflowActionHubContext.Clients.All.SendAsync(
                    "WorkflowActionEventCaught",
                    requestEventDto
                );
                await requestHubContext.Clients.All.SendAsync(
                    "RequestEventCaught",
                    requestEventDto
                );

                var workflowTriggers = await workflowTriggerService.GetByWorkflowActionIdAsync(
                    workflowAction.Id
                );
                workflowTriggersQueue.AddRange(
                    workflowTriggers.Select(x => x.WebhookEventId).ToArray()
                );
            }
        }

        public async Task HandleWebhookRuleResponseAsync(
            HttpContext httpContext,
            WebhookRule webhookRule
        )
        {
            var requestBody = await RequestUtils.ReadRequestBodyAsync(httpContext.Request);
            var requestEvent = RequestUtils.BuildRequestEventAsync(
                httpContext.Request,
                requestBody
            );
            if (requestEvent is not null)
            {
                requestEvent.WebhookRuleId = webhookRule.Id;
                context.RequestEvent.Add(requestEvent);
                await context.SaveChangesAsync();

                var requestEventDto = mapper.Map<RequestEventDto>(requestEvent);
                requestEventDto.Path = webhookRule.Path;
                requestEventDto.RedirectUrl = webhookRule.RedirectUrl;
                await webhookRuleHubContext.Clients.All.SendAsync(
                    "WebhookRuleEventCaught",
                    requestEventDto
                );
                await requestHubContext.Clients.All.SendAsync(
                    "RequestEventCaught",
                    requestEventDto
                );
                if (!string.IsNullOrEmpty(webhookRule.RedirectUrl))
                {
                    var webhookRequest = RequestUtils.BuildWebhookRequestAsync(
                        httpContext.Request,
                        requestBody,
                        webhookRule.RedirectUrl
                    );
                    await webhookRuleHubContext.Clients.All.SendAsync(
                        "WebhookRequestToRedirect",
                        mapper.Map<WebhookRequestDto>(webhookRequest)
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
