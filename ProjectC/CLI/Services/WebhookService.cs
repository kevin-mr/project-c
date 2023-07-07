using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using ProjectC.CLI.Services.Interfaces;
using ProjectC.Shared.Models;
using System.Text;

namespace ProjectC.CLI.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient http;

        public WebhookService(HttpClient http)
        {
            this.http = http;
        }

        public async Task RedirectWebhookEventAsync(WebhookEventDto webhookEvent)
        {
            try
            {
                var httpRequestMessage = BuildHttpRequestMessage(webhookEvent);
                await this.http.SendAsync(httpRequestMessage);
            }
            catch (Exception) { }
        }

        private HttpRequestMessage BuildHttpRequestMessage(WebhookEventDto webhookEvent)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = (new UriBuilder(webhookEvent.RedirectUrl)).Uri,
                Method = new HttpMethod(webhookEvent.Method)
            };

            foreach (var header in webhookEvent.Headers)
            {
                httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            if (string.IsNullOrEmpty(webhookEvent.Body))
            {
                httpRequestMessage.Content = new StreamContent(
                    new MemoryStream(Encoding.UTF8.GetBytes(webhookEvent.Body))
                );
                httpRequestMessage.Content.Headers.Add("Content-Type", webhookEvent.ContentType);
            }

            return httpRequestMessage;
        }
    }
}
