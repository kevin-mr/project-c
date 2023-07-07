using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/workflow-action")]
    public class WorkflowActionController
    {
        private readonly IMapper mapper;
        private readonly IWorkflowActionService workflowActionService;

        public WorkflowActionController(
            IMapper mapper,
            IWorkflowActionService workflowActionService
        )
        {
            this.mapper = mapper;
            this.workflowActionService = workflowActionService;
        }

        [HttpGet()]
        public async Task<IEnumerable<WorkflowActionDto>> GetAsync()
        {
            var workflowActions = await workflowActionService.GetAsync();

            return workflowActions.Select(x => mapper.Map<WorkflowActionDto>(x)).ToArray();
        }

        [HttpPost()]
        public Task CreateAsync(CreateWorkflowActionDto createWorkflowAction)
        {
            var workflowAction = mapper.Map<WorkflowAction>(createWorkflowAction);

            return workflowActionService.CreateAsync(workflowAction);
        }

        [HttpPut()]
        public Task UpdateAsync(EditWorkflowActionDto editWorkflowAction)
        {
            var workflow = mapper.Map<WorkflowAction>(editWorkflowAction);

            return workflowActionService.UpdateAsync(workflow);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return workflowActionService.DeleteAsync(id);
        }
    }
}
