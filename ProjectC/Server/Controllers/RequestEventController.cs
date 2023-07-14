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

        [HttpGet()]
        public async Task<IEnumerable<RequestEventDto>> GetAsync()
        {
            var requestEvents = await requestEventService.GetAsync();

            return requestEvents.Select(x => mapper.Map<RequestEventDto>(x)).ToArray();
        }

        [HttpGet("request-rule")]
        public async Task<IEnumerable<RequestEventDto>> GetByRequestRuleAsync()
        {
            var requestEvents = await requestEventService.GetByRequestRuleAsync();

            return requestEvents.Select(x => mapper.Map<RequestEventDto>(x)).ToArray();
        }

        [HttpGet("webhook-rule")]
        public async Task<IEnumerable<RequestEventDto>> GetByWebhookRuleAsync()
        {
            var requestEvents = await requestEventService.GetByWebhookRuleAsync();

            return requestEvents.Select(x => mapper.Map<RequestEventDto>(x)).ToArray();
        }

        [HttpGet("workflow-action")]
        public async Task<IEnumerable<RequestEventDto>> GetByWorkflowActionAsync()
        {
            var requestEvents = await requestEventService.GetByWorkflowActionAsync();

            return requestEvents.Select(x => mapper.Map<RequestEventDto>(x)).ToArray();
        }

        [HttpPost("webhook-rule/resend")]
        public Task ResentRequestAsync(RequestEventDto requestEvent)
        {
            return requestEventService.ResentRequestAsync(requestEvent.Id);
        }

        [HttpDelete("request-rule")]
        public Task DeleteByRequestRuleAsync()
        {
            return requestEventService.DeleteByRequestRuleAsync();
        }

        [HttpDelete("webhook-rule")]
        public Task DeleteByWebhookRuleAsync()
        {
            return requestEventService.DeleteByWebhookRuleAsync();
        }

        [HttpDelete("workflow-action")]
        public Task DeleteByWorkflowActionAsync()
        {
            return requestEventService.DeleteByWorkflowActionAsync();
        }
    }
}
