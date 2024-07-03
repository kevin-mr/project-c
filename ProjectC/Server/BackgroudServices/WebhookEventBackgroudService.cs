using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.BackgroudServices
{
    public class WebhookEventBackgroudService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly WebhookEventQueue requestRuleTriggersQueue;

        public WebhookEventBackgroudService(
            IServiceProvider serviceProvider,
            WebhookEventQueue requestRuleTriggersQueue
        )
        {
            this.serviceProvider = serviceProvider;
            this.requestRuleTriggersQueue = requestRuleTriggersQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var workflowEventService =
                scope.ServiceProvider.GetRequiredService<IWebhookEventService>();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                while (!requestRuleTriggersQueue.IsEmpty())
                {
                    var workflowEventId = requestRuleTriggersQueue.Read();
                    await workflowEventService.ResendAsync(workflowEventId);
                }
            }
        }
    }
}
