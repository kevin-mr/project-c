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
        private static readonly string HUB = "webhook-rule-events";
        private static string HUB_URL = string.Empty;

        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

            SetupHubURL(configuration);

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

        private static void SetupHubURL(IConfiguration configuration)
        {
            var urlArg = configuration["url"];
            if (!string.IsNullOrEmpty(urlArg))
            {
                HUB_URL = $"{urlArg}/{HUB}";
            }
            else
            {
                var errorMessage = "Couldn't setup hub url, please provide a 'url' argument";
                PrintErrorMessage(errorMessage);
                throw new Exception(errorMessage);
            }
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
                        PrintSuccesMessage(
                            $"EVENT Path: {webhookRequest.Path} - Method: {webhookRequest.Method} - RedirectUrl: {webhookRequest.RedirectUrl}"
                        );
                    }
                }
            );

            try
            {
                await hubConnection.StartAsync();
                PrintSuccesMessage("Server hub successfully connection ");
            }
            catch (Exception)
            {
                PrintErrorMessage(
                    "Couldn't connect to server, please review if it's running correctly"
                );
                throw;
            }
        }

        private static void PrintSuccesMessage(string message)
        {
            PrintMessage(message, ConsoleColor.Green);
        }

        private static void PrintErrorMessage(string message)
        {
            PrintMessage(message, ConsoleColor.Red);
        }

        private static void PrintMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
