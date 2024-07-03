using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/request-rule-variant")]
    public class RequestRuleVariantController
    {
        private readonly IMapper mapper;
        private readonly IRequestRuleVariantService requestRuleVariantService;
        private readonly IRequestRuleTriggerService requestRuleTriggerService;
        private readonly IValidator<RequestRuleVariant> requestRuleVariantValidator;

        public RequestRuleVariantController(
            IMapper mapper,
            IRequestRuleVariantService requestRuleVariantService,
            IRequestRuleTriggerService requestRuleTriggerService,
            IValidator<RequestRuleVariant> requestRuleVariantValidator
        )
        {
            this.mapper = mapper;
            this.requestRuleVariantService = requestRuleVariantService;
            this.requestRuleTriggerService = requestRuleTriggerService;
            this.requestRuleVariantValidator = requestRuleVariantValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<RequestRuleVariantDto>> GetAsync()
        {
            var requestRuleVariants = await requestRuleVariantService.GetAsync();

            return requestRuleVariants.Select(x => mapper.Map<RequestRuleVariantDto>(x)).ToArray();
        }

        [HttpGet("{id}/trigger")]
        public async Task<IEnumerable<RequestRuleTriggerDto>> GetTriggersAsync(int id)
        {
            var requestRuleTriggers =
                await requestRuleTriggerService.GetByRequestRuleVariantIdAsync(id);

            return requestRuleTriggers.Select(x => mapper.Map<RequestRuleTriggerDto>(x)).ToArray();
        }

        [HttpPost()]
        public async Task CreateAsync(CreateRequestRuleVariantDto createRequestRuleVariant)
        {
            var requestRuleVariant = mapper.Map<RequestRuleVariant>(createRequestRuleVariant);

            await requestRuleVariantValidator.ValidateAndThrowAsync(requestRuleVariant);

            await requestRuleVariantService.CreateAsync(requestRuleVariant);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditRequestRuleVariantDto editRequestRuleVariant)
        {
            var requestRuleVariant = mapper.Map<RequestRuleVariant>(editRequestRuleVariant);

            await requestRuleVariantValidator.ValidateAndThrowAsync(requestRuleVariant);

            await requestRuleVariantService.UpdateAsync(requestRuleVariant);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return requestRuleVariantService.DeleteAsync(id);
        }
    }
}
