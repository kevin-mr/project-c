using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.BackgroudServices;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Utils;
using ProjectC.Shared.Models;
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
        private readonly IHubContext<RequestRuleVariantHub> requestRuleVariantHubContext;
        private readonly IRequestRuleTriggerService requestRuleTriggerService;
        private readonly IWorkflowStorageService workflowStorageService;
        private readonly WebhookEventQueue requestRuleTriggersQueue;
        private readonly IMapper mapper;

        public MockServerService(
            ProjectCDbContext context,
            IHubContext<RequestHub> requestHubContext,
            IHubContext<RequestRuleHub> requestRuleHubContext,
            IHubContext<WebhookRuleHub> webhookRuleHubContext,
            IHubContext<RequestRuleVariantHub> requestRuleVariantHubContext,
            IRequestRuleTriggerService requestRuleTriggerService,
            IWorkflowStorageService workflowStorageService,
            WebhookEventQueue requestRuleTriggersQueue,
            IMapper mapper
        )
        {
            this.context = context;
            this.requestHubContext = requestHubContext;
            this.requestRuleHubContext = requestRuleHubContext;
            this.webhookRuleHubContext = webhookRuleHubContext;
            this.requestRuleVariantHubContext = requestRuleVariantHubContext;
            this.requestRuleTriggerService = requestRuleTriggerService;
            this.workflowStorageService = workflowStorageService;
            this.requestRuleTriggersQueue = requestRuleTriggersQueue;
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
            var queryString = request.QueryString.ToString();

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
                x =>
                    x.Method == method
                    && (
                        Regex.IsMatch(path, x.PathRegex)
                        || Regex.IsMatch(path + queryString, x.PathRegex)
                    )
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

        public async Task<RequestRuleVariant?> FindRequestRuleVariantAsync(
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
            var queryString = request.QueryString.ToString();

            var requestRuleVariants = await context.RequestRuleVariant
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
            var requestRuleVariant = requestRuleVariants.FirstOrDefault(
                x =>
                    x.Method == method
                    && (
                        Regex.IsMatch(path, x.PathRegex)
                        || Regex.IsMatch(path + queryString, x.PathRegex)
                    )
            );

            return requestRuleVariant is null
                ? null
                : await context.RequestRuleVariant
                    .Include(x => x.RequestRule)
                    .FirstOrDefaultAsync(x => x.Id == requestRuleVariant.Id);
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
                requestEvent.Path = requestRule.Path;
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

        public async Task HandleRequestRuleVariantResponseForRequestRuleAsync(
            HttpContext httpContext,
            Workflow workflow,
            RequestRuleVariant requestRuleVariant,
            bool storage
        )
        {
            if (requestRuleVariant.ResponseDelay > 0)
            {
                Thread.Sleep(requestRuleVariant.ResponseDelay);
            }
            httpContext.Response.StatusCode = requestRuleVariant.ResponseStatus;
            SetHeaders(httpContext, requestRuleVariant.ResponseHeaders);

            var requestBody = await RequestUtils.ReadRequestBodyAsync(httpContext.Request);
            var requestEvent = RequestUtils.BuildRequestEventAsync(
                httpContext.Request,
                requestBody
            );
            if (requestEvent is not null)
            {
                requestEvent.RequestRuleVariantId = requestRuleVariant.Id;
                if (requestRuleVariant.RequestRule is not null)
                {
                    requestEvent.RequestRuleId = requestRuleVariant.RequestRule.Id;
                    requestEvent.Path = requestRuleVariant.RequestRule.Path;
                }
                context.RequestEvent.Add(requestEvent);
                await context.SaveChangesAsync();

                var requestEventDto = mapper.Map<RequestEventDto>(requestEvent);
                requestEventDto.Path = requestRuleVariant.RequestRule is not null
                    ? requestRuleVariant.RequestRule.Path
                    : requestRuleVariant.Path ?? throw new Exception("Invalid Requets Path");
                requestEventDto.RequestRuleVariantDescription = requestRuleVariant.Description;
                await requestRuleVariantHubContext.Clients.All.SendAsync(
                    "RequestRuleVariantEventCaught",
                    requestEventDto
                );
                await requestHubContext.Clients.All.SendAsync(
                    "RequestEventCaught",
                    requestEventDto
                );

                var requestRuleTriggers =
                    await requestRuleTriggerService.GetByRequestRuleVariantIdAsync(
                        requestRuleVariant.Id
                    );
                requestRuleTriggersQueue.AddRange(
                    requestRuleTriggers.Select(x => x.WebhookEventId).ToArray()
                );

                if (storage)
                {
                    var result = await workflowStorageService.HandleRequestAsync(
                        httpContext.Request.Query,
                        workflow.Id,
                        requestEvent.Id
                    );
                    if (!string.IsNullOrEmpty(result))
                    {
                        await httpContext.Response.WriteAsync(result);
                    }
                    else
                    {
                        await httpContext.Response.WriteAsync(
                            requestRuleVariant.ResponseBody ?? string.Empty
                        );
                    }
                }
                else
                {
                    await httpContext.Response.WriteAsync(requestRuleVariant.ResponseBody);
                }
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
                requestEvent.Path = webhookRule.Path;
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
