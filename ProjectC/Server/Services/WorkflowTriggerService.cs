using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class WorkflowTriggerService : IWorkflowTriggerService
    {
        private readonly ProjectCDbContext context;

        public WorkflowTriggerService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WorkflowTrigger>> GetAsync()
        {
            return await context.WorkflowTrigger.Include(x => x.WebhookEvent).ToArrayAsync();
        }

        public async Task<IEnumerable<WorkflowTrigger>> GetByWorkflowActionIdAsync(
            int workflowActionId
        )
        {
            return await context.WorkflowTrigger
                .Where(x => x.WorkflowActionId == workflowActionId)
                .Include(x => x.WebhookEvent)
                .ToArrayAsync();
        }

        public async Task CreateAsync(WorkflowTrigger workflowTrigger)
        {
            context.WorkflowTrigger.Add(workflowTrigger);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkflowTrigger workflowTrigger)
        {
            var currentWorkflowTrigger =
                await context.WorkflowTrigger.FirstOrDefaultAsync(x => x.Id == workflowTrigger.Id)
                ?? throw new Exception("Workflow trigger not found");

            currentWorkflowTrigger.Description = workflowTrigger.Description;
            currentWorkflowTrigger.WebhookEventId = workflowTrigger.WebhookEventId;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workflowTrigger = await context.WorkflowTrigger.FirstOrDefaultAsync(
                x => x.Id == id
            );
            if (workflowTrigger is not null)
            {
                context.WorkflowTrigger.Remove(workflowTrigger);
                await context.SaveChangesAsync();
            }
        }
    }
}
