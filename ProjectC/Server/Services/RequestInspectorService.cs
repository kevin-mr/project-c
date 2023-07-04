using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;
using System.Text;
using System.Text.Json;

namespace ProjectC.Server.Services
{
    public class RequestInspectorService : IRequestInspectorService
    {
        private int index = 0;

        public RequestInspectorService() { }

        public async Task<RequestDto> BuildRequestAsync(HttpRequest request)
        {
            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(request.Body))
            {
                stringBuilder.AppendLine(await reader.ReadToEndAsync());
            }

            var requestDto = new RequestDto
            {
                Id = index++,
                Method = request.Method,
                Headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()),
                Body = stringBuilder.ToString(),
                ArrivalDate = DateTime.Now,
            };

            try
            {
                requestDto.JsonBody = JsonSerializer.Serialize(
                    requestDto.Body,
                    new JsonSerializerOptions() { WriteIndented = true }
                );
            }
            catch (Exception)
            {
                requestDto.JsonBody = requestDto.Body;
            }

            return requestDto;
        }

        public async Task<WebhookEventDto> BuildWebhookEventAsync(
            HttpRequest request,
            string redirectUrl
        )
        {
            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(request.Body))
            {
                stringBuilder.AppendLine(await reader.ReadToEndAsync());
            }

            return new WebhookEventDto
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
