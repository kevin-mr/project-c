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

        public async Task RedirectWebhookRequestAsync(WebhookRequestDto webhookRequest)
        {
            try
            {
                var httpRequestMessage = BuildHttpRequestMessage(webhookRequest);
                await http.SendAsync(httpRequestMessage);
            }
            catch (Exception) { }
        }

        private HttpRequestMessage BuildHttpRequestMessage(WebhookRequestDto webhookRequest)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = (new UriBuilder(webhookRequest.RedirectUrl)).Uri,
                Method = new HttpMethod(webhookRequest.Method)
            };

            foreach (var header in webhookRequest.Headers)
            {
                httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            if (!string.IsNullOrEmpty(webhookRequest.Body))
            {
                httpRequestMessage.Content = new StreamContent(
                    new MemoryStream(Encoding.UTF8.GetBytes(webhookRequest.Body))
                );
                httpRequestMessage.Content.Headers.Add("Content-Type", webhookRequest.ContentType);
            }

            return httpRequestMessage;
        }
    }
}
