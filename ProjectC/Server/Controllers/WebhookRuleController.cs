using AutoMapper;
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
        private readonly IWebookRuleService webhookRuleService;

        public WebhookRuleController(IMapper mapper, IWebookRuleService webhookRuleService)
        {
            this.mapper = mapper;
            this.webhookRuleService = webhookRuleService;
        }

        [HttpGet()]
        public async Task<IEnumerable<WebhookRuleDto>> GetAsync()
        {
            var webhookRules = await webhookRuleService.GetAsync();

            return webhookRules.Select(x => mapper.Map<WebhookRuleDto>(x)).ToArray();
        }

        [HttpPost()]
        public Task CreateAsync(CreateWebhookRuleDto createWebhookRule)
        {
            var webhookRule = mapper.Map<WebhookRule>(createWebhookRule);

            return webhookRuleService.CreateAsync(webhookRule);
        }

        [HttpPut()]
        public Task UpdateAsync(EditWebhookRuleDto editWebhookRule)
        {
            var webhookRule = mapper.Map<WebhookRule>(editWebhookRule);

            return webhookRuleService.UpdateAsync(webhookRule);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return webhookRuleService.DeleteAsync(id);
        }
    }
}
