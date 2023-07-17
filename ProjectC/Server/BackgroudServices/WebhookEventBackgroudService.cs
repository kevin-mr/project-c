using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.BackgroudServices
{
    public class WebhookEventBackgroudService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly WebhookEventQueue workflowTriggersQueue;

        public WebhookEventBackgroudService(
            IServiceProvider serviceProvider,
            WebhookEventQueue workflowTriggersQueue
        )
        {
            this.serviceProvider = serviceProvider;
            this.workflowTriggersQueue = workflowTriggersQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var workflowEventService =
                scope.ServiceProvider.GetRequiredService<IWebhookEventService>();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

                while (!workflowTriggersQueue.IsEmpty())
                {
                    var workflowEventId = workflowTriggersQueue.Read();
                    await workflowEventService.ResendAsync(workflowEventId);
                }
            }
        }
    }
}
