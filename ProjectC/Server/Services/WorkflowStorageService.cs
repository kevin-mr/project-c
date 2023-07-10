using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class WorkflowStorageService : IWorkflowStorageService
    {
        private readonly ProjectCDbContext context;

        public WorkflowStorageService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WorkflowStorage>> GetAsync()
        {
            return await context.WorkflowStorage.ToArrayAsync();
        }

        public async Task<WorkflowStorage?> GetByWorkflowIdAsync(int workflowId)
        {
            return await context.WorkflowStorage.FirstOrDefaultAsync(
                x => x.WorkflowId == workflowId
            );
        }

        public async Task CreateAsync(WorkflowStorage workflowStorage)
        {
            context.WorkflowStorage.Add(workflowStorage);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkflowStorage workflowStorage)
        {
            var currentWorkflowStorage =
                await context.WorkflowStorage.FirstOrDefaultAsync(x => x.Id == workflowStorage.Id)
                ?? throw new Exception("Workflow storage not found");

            currentWorkflowStorage.PropertyIdentifier = workflowStorage.PropertyIdentifier;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workflowStorage = await context.WorkflowStorage.FirstOrDefaultAsync(
                x => x.Id == id
            );
            if (workflowStorage is not null)
            {
                context.WorkflowStorage.Remove(workflowStorage);
                await context.SaveChangesAsync();
            }
        }
    }
}
