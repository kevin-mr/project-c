using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Shared.Models;

namespace ProjectC.Server.Controllers
{
    [ApiController()]
    [Route("api/v1/workflow-storage")]
    public class WorkflowStorageController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWorkflowStorageService workflowStorageService;
        private readonly IValidator<WorkflowStorage> workflowStorageValidator;

        public WorkflowStorageController(
            IMapper mapper,
            IWorkflowStorageService workflowStorageService,
            IValidator<WorkflowStorage> workflowStorageValidator
        )
        {
            this.mapper = mapper;
            this.workflowStorageService = workflowStorageService;
            this.workflowStorageValidator = workflowStorageValidator;
        }

        [HttpGet()]
        public async Task<IEnumerable<WorkflowStorageDto>> GetAsync()
        {
            var workflowStorages = await workflowStorageService.GetAsync();

            return workflowStorages.Select(x => mapper.Map<WorkflowStorageDto>(x)).ToArray();
        }

        [HttpPost()]
        public async Task CreateAsync(CreateWorkflowStorageDto createWorkflowStorage)
        {
            var workflowStorage = mapper.Map<WorkflowStorage>(createWorkflowStorage);

            await workflowStorageValidator.ValidateAndThrowAsync(workflowStorage);

            await workflowStorageService.CreateAsync(workflowStorage);
        }

        [HttpPost("{id}/clear")]
        public async Task ClearDataAsync(int id)
        {
            await workflowStorageService.ClearDataAsync(id);
        }

        [HttpPut()]
        public async Task UpdateAsync(EditWorkflowStorageDto editWorkflowStorage)
        {
            var workflowStorage = mapper.Map<WorkflowStorage>(editWorkflowStorage);

            await workflowStorageValidator.ValidateAndThrowAsync(workflowStorage);

            await workflowStorageService.UpdateAsync(workflowStorage);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id)
        {
            return workflowStorageService.DeleteAsync(id);
        }
    }
}
