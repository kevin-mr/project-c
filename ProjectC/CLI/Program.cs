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
        private static readonly string HUB_URL = "https://localhost:7026/webhook-rule-events";

        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IWebhookService, WebhookService>();
                    services.AddSingleton<HttpClient>();
                })
                .Build();

            var webhookService = host.Services.GetService<IWebhookService>();
            if (webhookService is not null)
            {
                await ConnectToHubAsync(webhookService);
            }

            await host.RunAsync();
        }

        private static async Task ConnectToHubAsync(IWebhookService webhookService)
        {
            var hubConnection = new HubConnectionBuilder().WithUrl(HUB_URL).Build();

            hubConnection.On<WebhookRequestDto>(
                "WebhookRequestToRedirect",
                async (webhookRequest) =>
                {
                    if (webhookRequest is not null)
                    {
                        await webhookService.RedirectWebhookRequestAsync(webhookRequest);
                    }
                }
            );

            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Couldn't connect to server, please review if it's running correctly"
                );
                Console.ResetColor();
                throw;
            }
        }
    }
}
