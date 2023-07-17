using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Services;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/webhook-event")]
    public class WebhookEventController
    {
        private readonly IWebhookEventService webhookEventService;

        public WebhookEventController(IWebhookEventService webhookEventService)
        {
            this.webhookEventService = webhookEventService;
        }

        [HttpPost("{id}/resend")]
        public Task DeleteAsync(int id)
        {
            return webhookEventService.ResendAsync(id);
        }

        [HttpDelete("{id}")]
        public Task DeleteByRequestRuleAsync(int id)
        {
            return webhookEventService.DeleteAsync(id);
        }
    }
}
