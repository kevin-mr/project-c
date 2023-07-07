using AutoMapper;
using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Validators;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/webhook")]
    public class WebhookRuleController
    {
        private readonly IMapper mapper;
        private readonly IWebookRuleService webhookRuleService;
        private readonly IValidator<WebhookRule> webhookRuleValidator;

        public WebhookRuleController(
            IMapper mapper,
            IWebookRuleService webhookRuleService,
            IValidator<WebhookRule> webhookRuleValidator
        )
        {
            this.mapper = mapper;
            this.webhookRuleService = webhookRuleService;
            this.webhookRuleValidator = webhookRuleValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<WebhookRuleDto>> GetAsync()
        {
            var webhookRules = await webhookRuleService.GetAsync();

            return webhookRules.Select(x => mapper.Map<WebhookRuleDto>(x)).ToArray();
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
