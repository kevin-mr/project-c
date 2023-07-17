using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Services;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/webhook-event")]
    public class WebhookEventController
    {
        private readonly IMapper mapper;
        private readonly IWebhookEventService webhookEventService;

        public WebhookEventController(IMapper mapper, IWebhookEventService webhookEventService)
        {
            this.mapper = mapper;
            this.webhookEventService = webhookEventService;
        }

        [HttpGet()]
        public async Task<IEnumerable<WebhookEventDto>> GetAsync()
        {
            var webhookEvents = await webhookEventService.GetAsync();

            return webhookEvents.Select(x => mapper.Map<WebhookEventDto>(x)).ToArray();
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
