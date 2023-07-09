using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Services;

namespace ProjectC.Server.Middlewares
{
    public static class WebhookMiddleware
    {
        public static IApplicationBuilder UseWebhookHandler(this IApplicationBuilder app)
        {
            return app.Use(
                async (context, next) =>
                {
                    if (context.Request.Path.StartsWithSegments(MockServerService.WEBHOOK_PREFIX))
                    {
                        var mockServerService =
                            context.RequestServices.GetRequiredService<IMockServerService>();
                        if (mockServerService is not null)
                        {
                            var webhookRule = await mockServerService.FindWebhookRuleAsync(
                                context.Request
                            );
                            if (webhookRule is not null)
                            {
                                await mockServerService.HandleWebhookRuleResponseAsync(
                                    context,
                                    webhookRule
                                );
                                return;
                            }
                        }
                    }

                    await next(context);
                }
            );
        }
    }
}
