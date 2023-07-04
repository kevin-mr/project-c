using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectC.CLI.Services;
using ProjectC.CLI.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.CLI
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddCommandLine(args).Build();

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IWebhookService, WebhookService>();
                    services.AddSingleton<HttpClient>();
                })
                .Build();

            var webhookService = host.Services.GetService<IWebhookService>();

            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7026/webhook-rule-events")
                .Build();

            hubConnection.On<WebhookEventDto>(
                "WebhookRuleEventToRedirect",
                async (webhookEvent) =>
                {
                    if (webhookEvent is not null && webhookService is not null)
                    {
                        await webhookService.RedirectWebhookEventAsync(webhookEvent);
                    }
                }
            );

            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception)
            {
                throw;
            }

            await host.RunAsync();
        }
    }
}
