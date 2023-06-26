using AutoMapper;
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

        public RequestRuleController(IMapper mapper, IRequestRuleService requestRuleService)
        {
            this.mapper = mapper;
            this.requestRuleService = requestRuleService;
        }

        [HttpGet()]
        public async Task<IEnumerable<RequestRuleDto>> GetAsync()
        {
            var requestRules = await requestRuleService.GetAsync();

            return requestRules.Select(x => mapper.Map<RequestRuleDto>(x)).ToArray();
        }

        [HttpPost()]
        public Task CreateAsync(CreateRequestRuleDto createRequestRule)
        {
            var request = mapper.Map<RequestRule>(createRequestRule);

            return requestRuleService.CreateAsync(request);
        }

        [HttpPut()]
        public Task UpdateAsync(EditRequestRuleDto editRequestRule)
        {
            var request = mapper.Map<RequestRule>(editRequestRule);

            return requestRuleService.UpdateAsync(request);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return requestRuleService.DeleteAsync(id);
        }
    }
}
