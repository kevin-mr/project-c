using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/workflow-trigger")]
    public class WorkflowTriggerController
    {
        private readonly IMapper mapper;
        private readonly IWorkflowTriggerService workflowTriggerService;
        private readonly IValidator<WorkflowTrigger> workflowTriggerValidator;

        public WorkflowTriggerController(
            IMapper mapper,
            IWorkflowTriggerService workflowTriggerService,
            IValidator<WorkflowTrigger> workflowTriggerValidator
        )
        {
            this.mapper = mapper;
            this.workflowTriggerService = workflowTriggerService;
            this.workflowTriggerValidator = workflowTriggerValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<WorkflowTriggerDto>> GetAsync()
        {
            var workflowTriggers = await workflowTriggerService.GetAsync();

            return workflowTriggers.Select(x => mapper.Map<WorkflowTriggerDto>(x)).ToArray();
        }

        [HttpPost()]
        public async Task CreateAsync(CreateWorkflowTriggerDto createWorkflowTrigger)
        {
            var workflowTrigger = mapper.Map<WorkflowTrigger>(createWorkflowTrigger);

            await workflowTriggerValidator.ValidateAndThrowAsync(workflowTrigger);

            await workflowTriggerService.CreateAsync(workflowTrigger);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditWorkflowTriggerDto editWorkflowTrigger)
        {
            var workflowTrigger = mapper.Map<WorkflowTrigger>(editWorkflowTrigger);

            await workflowTriggerValidator.ValidateAndThrowAsync(workflowTrigger);

            await workflowTriggerService.UpdateAsync(workflowTrigger);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return workflowTriggerService.DeleteAsync(id);
        }
    }
}
