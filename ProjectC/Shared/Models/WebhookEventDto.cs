using Microsoft.AspNetCore.Http;

namespace ProjectC.Shared.Models
{
    public class WebhookEventDto
    {
        public WebhookEventDto()
        {
            RedirectUrl = string.Empty;
        }

        public HttpRequest? Request { get; set; }
        public string RedirectUrl { get; set; }
    }
}
