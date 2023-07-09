using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;
using ProjectC.Server.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace ProjectC.Server.Services
{
    public class RequestInspectorService : IRequestInspectorService
    {
        public RequestInspectorService() { }

        public async Task<RequestEvent> BuildRequestEventAsync(HttpRequest request)
        {
            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(request.Body))
            {
                stringBuilder.AppendLine(await reader.ReadToEndAsync());
            }

            var headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            var body = stringBuilder.ToString();

            var requestEvent = new RequestEvent
            {
                Method = request.Method,
                ArrivalDate = DateTime.Now,
            };

            try
            {
                requestEvent.JsonBody = JsonSerializer.Serialize(body);
                requestEvent.JsonHeaders = JsonSerializer.Serialize(headers);
            }
            catch (Exception e) { }

            return requestEvent;
        }

        public async Task<WebhookEvent> BuildWebhookEventAsync(
            HttpRequest request,
            string redirectUrl
        )
        {
            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(request.Body))
            {
                stringBuilder.AppendLine(await reader.ReadToEndAsync());
            }

            return new WebhookEvent
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? throw new Exception("Invalid port"),
                PathBase = request.PathBase,
                Path = request.Path,
                Method = request.Method,
                Headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToArray()),
                ContentType = request.ContentType,
                RedirectUrl = redirectUrl,
                Body = stringBuilder.ToString()
            };
        }
    }
}
