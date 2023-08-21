using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/webhook")]
    public class WebhookRuleController
    {
        private readonly IMapper mapper;
        private readonly IWebhookRuleService webhookRuleService;
        private readonly IValidator<WebhookRule> webhookRuleValidator;
        private readonly IWebhookEventService webhookEventService;

        public WebhookRuleController(
            IMapper mapper,
            IWebhookRuleService webhookRuleService,
            IValidator<WebhookRule> webhookRuleValidator,
            IWebhookEventService webhookEventService
        )
        {
            this.mapper = mapper;
            this.webhookRuleService = webhookRuleService;
            this.webhookRuleValidator = webhookRuleValidator;
            this.webhookEventService = webhookEventService;
        }

        [HttpGet()]
        public async Task<IEnumerable<WebhookRuleDto>> GetAsync()
        {
            var webhookRules = await webhookRuleService.GetAsync();

            return webhookRules.Select(x => mapper.Map<WebhookRuleDto>(x)).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<WebhookRuleDto> GetByIdAsync(int id)
        {
            var webhookRule = await webhookRuleService.GetByIdAsync(id);

            return mapper.Map<WebhookRuleDto>(webhookRule);
        }

        [HttpGet("{id}/events")]
        public async Task<IEnumerable<WebhookEventDto>> GetEventsAsync(int id)
        {
            var webhookEvents = await webhookEventService.GetByWebhookRuleIdAsync(id);

            return webhookEvents.Select(x => mapper.Map<WebhookEventDto>(x)).ToArray();
        }

        [HttpPost()]
        public async Task CreateAsync(CreateWebhookRuleDto createWebhookRule)
        {
            var webhookRule = mapper.Map<WebhookRule>(createWebhookRule);

            await webhookRuleValidator.ValidateAndThrowAsync(webhookRule);

            await webhookRuleService.CreateAsync(webhookRule);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditWebhookRuleDto editWebhookRule)
        {
            var webhookRule = mapper.Map<WebhookRule>(editWebhookRule);

            await webhookRuleValidator.ValidateAndThrowAsync(webhookRule);

            await webhookRuleService.UpdateAsync(webhookRule);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return webhookRuleService.DeleteAsync(id);
        }
    }
}
