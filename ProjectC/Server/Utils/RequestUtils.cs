using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace ProjectC.Server.Utils
{
    public static class RequestUtils
    {
        private static readonly int PORT_DEFAULT = 80;

        public static RequestEvent BuildRequestEventAsync(HttpRequest request, string body)
        {
            var headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());

            var requestEvent = new RequestEvent
            {
                Method = request.Method,
                ArrivalDate = DateTime.Now,
            };

            try
            {
                requestEvent.JsonBody = body;
                requestEvent.JsonHeaders = JsonSerializer.Serialize(headers);
            }
            catch (Exception) { }

            return requestEvent;
        }

        public static WebhookRequest BuildWebhookRequestAsync(
            HttpRequest request,
            string body,
            string redirectUrl
        )
        {
            var headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToArray());

            return new WebhookRequest
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? PORT_DEFAULT,
                PathBase = request.PathBase,
                Path = request.Path,
                Method = request.Method,
                Headers = headers,
                ContentType = request.ContentType,
                RedirectUrl = redirectUrl,
                Body = body
            };
        }

        public static WebhookRequest BuildWebhookRequestAsync(
            RequestEvent requestEvent,
            string redirectUrl
        )
        {
            var headers = requestEvent.Headers.ToDictionary(x => x.Key, x => x.Value.ToArray());

            return new Models.WebhookRequest
            {
                Method = requestEvent.Method,
                Headers = requestEvent.Headers.ToDictionary(
                    x => x.Key,
                    x => (string?[])x.Value.Split(",")
                ),
                ContentType = requestEvent.Headers["Content-Type"],
                RedirectUrl = redirectUrl,
                Body = requestEvent.Body
            };
        }

        public static WebhookRequest BuildWebhookRequestAsync(
            WebhookEvent webhookEvent,
            string redirectUrl
        )
        {
            var headers = webhookEvent.Headers.ToDictionary(x => x.Key, x => x.Value.ToArray());

            return new Models.WebhookRequest
            {
                Method = webhookEvent.Method,
                Headers = webhookEvent.Headers.ToDictionary(
                    x => x.Key,
                    x => (string?[])x.Value.Split(",")
                ),
                ContentType = webhookEvent.Headers["Content-Type"],
                RedirectUrl = redirectUrl,
                Body = webhookEvent.Body
            };
        }

        public static async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            var stringBuilder = new StringBuilder();

            using (var reader = new StreamReader(request.Body))
            {
                stringBuilder.AppendLine(await reader.ReadToEndAsync());
            }

            return stringBuilder.ToString();
        }

        public static string BuildRegexPath(string path)
        {
            return path.Contains("{number}") ? path.Replace("{number}", "[0-9]+") : path;
        }
    }
}
