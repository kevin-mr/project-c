using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/request-rule-trigger")]
    public class RequestRuleTriggerController
    {
        private readonly IMapper mapper;
        private readonly IRequestRuleTriggerService requestRuleTriggerService;
        private readonly IValidator<RequestRuleTrigger> requestRuleTriggerValidator;

        public RequestRuleTriggerController(
            IMapper mapper,
            IRequestRuleTriggerService requestRuleTriggerService,
            IValidator<RequestRuleTrigger> requestRuleTriggerValidator
        )
        {
            this.mapper = mapper;
            this.requestRuleTriggerService = requestRuleTriggerService;
            this.requestRuleTriggerValidator = requestRuleTriggerValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<RequestRuleTriggerDto>> GetAsync()
        {
            var requestRuleTriggers = await requestRuleTriggerService.GetAsync();

            return requestRuleTriggers.Select(x => mapper.Map<RequestRuleTriggerDto>(x)).ToArray();
        }

        [HttpPost()]
        public async Task CreateAsync(CreateRequestRuleTriggerDto createRequestRuleTrigger)
        {
            var requestRuleTrigger = mapper.Map<RequestRuleTrigger>(createRequestRuleTrigger);

            await requestRuleTriggerValidator.ValidateAndThrowAsync(requestRuleTrigger);

            await requestRuleTriggerService.CreateAsync(requestRuleTrigger);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditRequestRuleTriggerDto editRequestRuleTrigger)
        {
            var requestRuleTrigger = mapper.Map<RequestRuleTrigger>(editRequestRuleTrigger);

            await requestRuleTriggerValidator.ValidateAndThrowAsync(requestRuleTrigger);

            await requestRuleTriggerService.UpdateAsync(requestRuleTrigger);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return requestRuleTriggerService.DeleteAsync(id);
        }
    }
}
