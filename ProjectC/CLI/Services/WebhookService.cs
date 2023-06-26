using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using ProjectC.CLI.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.CLI.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient http;

        public WebhookService(HttpClient http)
        {
            this.http = http;
        }

        public void RedirectWebhookEvent(WebhookEventDto webhookEventDto)
        {
            if (webhookEventDto.Request is not null)
            {
                this.http.SendAsync(webhookEventDto.Request.ToHttpRequestMessage());
            }
        }
    }
}
