using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/request-event")]
    public class RequestEventController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRequestEventService requestEventService;

        public RequestEventController(IMapper mapper, IRequestEventService requestEventService)
        {
            this.mapper = mapper;
            this.requestEventService = requestEventService;
        }

        [HttpGet("request-rule")]
        public async Task<IEnumerable<RequestEventDto>> GetByRequestRuleAsync()
        {
            var requestEvents = await requestEventService.GetByRequestRuleAsync();
            try
            {
                return requestEvents.Select(x => mapper.Map<RequestEventDto>(x)).ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("webhook-rule")]
        public async Task<IEnumerable<RequestEventDto>> GetByWebhookRuleAsync()
        {
            var requestEvents = await requestEventService.GetByWebhookRuleAsync();

            return requestEvents.Select(x => mapper.Map<RequestEventDto>(x)).ToArray();
        }
    }
}
