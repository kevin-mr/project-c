using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/request")]
    public class RequestRuleController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRequestRuleService requestRuleService;
        private readonly IValidator<RequestRule> requestRuleValidator;

        public RequestRuleController(
            IMapper mapper,
            IRequestRuleService requestRuleService,
            IValidator<RequestRule> requestRuleValidator
        )
        {
            this.mapper = mapper;
            this.requestRuleService = requestRuleService;
            this.requestRuleValidator = requestRuleValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<RequestRuleDto>> GetAsync()
        {
            var requestRules = await requestRuleService.GetAsync();

            return requestRules.Select(x => mapper.Map<RequestRuleDto>(x)).ToArray();
        }

        [HttpPost()]
        public async Task CreateAsync(CreateRequestRuleDto createRequestRule)
        {
            var request = mapper.Map<RequestRule>(createRequestRule);

            await requestRuleValidator.ValidateAndThrowAsync(request);

            await requestRuleService.CreateAsync(request);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditRequestRuleDto editRequestRule)
        {
            var request = mapper.Map<RequestRule>(editRequestRule);

            await requestRuleValidator.ValidateAndThrowAsync(request);

            await requestRuleService.UpdateAsync(request);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return requestRuleService.DeleteAsync(id);
        }
    }
}
