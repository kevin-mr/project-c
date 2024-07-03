using AutoMapper;
using FluentValidation;
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
        private readonly IRequestRuleVariantService requestRuleVariantService;
        private readonly IWorkflowStorageService workflowStorageService;
        private readonly IValidator<Workflow> workflowValidator;

        public WorkflowController(
            IMapper mapper,
            IWorkflowService workflowService,
            IRequestRuleVariantService requestRuleVariantService,
            IWorkflowStorageService workflowStorageService,
            IValidator<Workflow> workflowValidator
        )
        {
            this.mapper = mapper;
            this.workflowService = workflowService;
            this.requestRuleVariantService = requestRuleVariantService;
            this.workflowStorageService = workflowStorageService;
            this.workflowValidator = workflowValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<WorkflowDto>> GetAsync()
        {
            var workflows = await workflowService.GetAsync();

            return workflows.Select(x => mapper.Map<WorkflowDto>(x)).ToArray();
        }

        [HttpGet("{id}/action")]
        public async Task<IEnumerable<RequestRuleVariantDto>> GetActionsAsync(int id)
        {
            var requestRuleVariants = await requestRuleVariantService.GetByWorkflowIdAsync(id);

            return requestRuleVariants.Select(x => mapper.Map<RequestRuleVariantDto>(x)).ToArray();
        }

        [HttpGet("{id}/storage")]
        public async Task<WorkflowStorageDto?> GetStorageAsync(int id)
        {
            var workflowStorage = await workflowStorageService.GetByWorkflowIdAsync(id);

            return workflowStorage is not null
                ? mapper.Map<WorkflowStorageDto>(workflowStorage)
                : null;
        }

        [HttpPost()]
        public async Task CreateAsync(CreateWorkflowDto createWorkflow)
        {
            var workflow = mapper.Map<Workflow>(createWorkflow);

            await workflowValidator.ValidateAndThrowAsync(workflow);

            await workflowService.CreateAsync(workflow);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditWorkflowDto editWorkflow)
        {
            var workflow = mapper.Map<Workflow>(editWorkflow);

            await workflowValidator.ValidateAndThrowAsync(workflow);

            await workflowService.UpdateAsync(workflow);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return workflowService.DeleteAsync(id);
        }
    }
}
