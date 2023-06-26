using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectC.CLI.Services;
using ProjectC.CLI.Services.Interfaces;
using ProjectC.Shared.Models;
using static System.Net.WebRequestMethods;

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
                .WithUrl($"{config["redirect-to"]}/webhook-rule-events")
                .Build();

            hubConnection.On<WebhookEventDto>(
                "WebhookRuleEventToRedirect",
                (webhookEvent) =>
                {
                    if (webhookEvent is not null)
                    {
                        webhookService?.RedirectWebhookEvent(webhookEvent);
                    }
                }
            );

            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            await host.RunAsync();
        }
    }
}
