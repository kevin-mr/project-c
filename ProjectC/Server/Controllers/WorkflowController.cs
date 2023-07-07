using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/workflow")]
    public class WorkflowController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWorkflowService workflowService;
        private readonly IWorkflowActionService workflowActionService;

        public WorkflowController(
            IMapper mapper,
            IWorkflowService workflowService,
            IWorkflowActionService workflowActionService
        )
        {
            this.mapper = mapper;
            this.workflowService = workflowService;
            this.workflowActionService = workflowActionService;
        }

        [HttpGet()]
        public async Task<IEnumerable<WorkflowDto>> GetAsync()
        {
            var workflows = await workflowService.GetAsync();

            return workflows.Select(x => mapper.Map<WorkflowDto>(x)).ToArray();
        }

        [HttpGet("{id}/action")]
        public async Task<IEnumerable<WorkflowActionDto>> GetActionsAsync(int id)
        {
            var workflowActions = await workflowActionService.GetByWorkflowIdAsync(id);

            return workflowActions.Select(x => mapper.Map<WorkflowActionDto>(x)).ToArray();
        }

        [HttpPost()]
        public Task CreateAsync(CreateWorkflowDto createWorkflow)
        {
            var workflow = mapper.Map<Workflow>(createWorkflow);

            return workflowService.CreateAsync(workflow);
        }

        [HttpPut()]
        public Task UpdateAsync(EditWorkflowDto editWorkflow)
        {
            var workflow = mapper.Map<Workflow>(editWorkflow);

            return workflowService.UpdateAsync(workflow);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return workflowService.DeleteAsync(id);
        }
    }
}
