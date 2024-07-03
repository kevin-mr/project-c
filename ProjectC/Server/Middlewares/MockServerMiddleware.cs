using ProjectC.Server.Services;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Middlewares
{
    public static class MockServerMiddleware
    {
        public static IApplicationBuilder UseMockServerHandler(this IApplicationBuilder app)
        {
            return app.Use(
                async (context, next) =>
                {
                    if (
                        context.Request.Path.StartsWithSegments(
                            MockServerService.MOCK_SERVER_PREFIX
                        )
                    )
                    {
                        var workflowKey = context.Request.Headers["workflow-key"].ToString();
                        var storageQueryParam = context.Request.Query["storage"].ToString();

                        var mockServerService =
                            context.RequestServices.GetRequiredService<IMockServerService>();
                        var workflowService =
                            context.RequestServices.GetRequiredService<IWorkflowService>();

                        if (mockServerService is null || workflowService is null)
                        {
                            return;
                        }

                        if (
                            !string.IsNullOrEmpty(workflowKey)
                            && int.TryParse(workflowKey, out var workFlowId)
                        )
                        {
                            var workflow = await workflowService.GetAsync(workFlowId);
                            if (workflow is not null)
                            {
                                var requestRuleVariant =
                                    await mockServerService.FindRequestRuleVariantAsync(
                                        context.Request,
                                        workflow.Id
                                    );
                                if (requestRuleVariant is not null)
                                {
                                    await mockServerService.HandleRequestRuleVariantResponseForRequestRuleAsync(
                                        context,
                                        workflow,
                                        requestRuleVariant,
                                        bool.TryParse(storageQueryParam, out var storage) && storage
                                    );
                                }
                            }
                            return;
                        }
                        else
                        {
                            var requestRule = await mockServerService.FindRequestRuleAsync(
                                context.Request
                            );
                            if (requestRule is not null)
                            {
                                await mockServerService.HandleRequestRuleResponseAsync(
                                    context,
                                    requestRule
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
