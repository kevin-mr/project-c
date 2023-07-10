using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Client.Pages;
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
        private readonly IValidator<WorkflowAction> workflowActionValidator;

        public WorkflowActionController(
            IMapper mapper,
            IWorkflowActionService workflowActionService,
            IValidator<WorkflowAction> workflowActionValidator
        )
        {
            this.mapper = mapper;
            this.workflowActionService = workflowActionService;
            this.workflowActionValidator = workflowActionValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<WorkflowActionDto>> GetAsync()
        {
            var workflowActions = await workflowActionService.GetAsync();

            return workflowActions.Select(x => mapper.Map<WorkflowActionDto>(x)).ToArray();
        }

        [HttpPost()]
        public async Task CreateAsync(CreateWorkflowActionDto createWorkflowAction)
        {
            var workflowAction = mapper.Map<WorkflowAction>(createWorkflowAction);

            await workflowActionValidator.ValidateAndThrowAsync(workflowAction);

            await workflowActionService.CreateAsync(workflowAction);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditWorkflowActionDto editWorkflowAction)
        {
            var workflowAction = mapper.Map<WorkflowAction>(editWorkflowAction);

            await workflowActionValidator.ValidateAndThrowAsync(workflowAction);

            await workflowActionService.UpdateAsync(workflowAction);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return workflowActionService.DeleteAsync(id);
        }
    }
}
