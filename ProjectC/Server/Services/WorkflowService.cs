using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly ProjectCDbContext context;

        public WorkflowService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Workflow>> GetAsync()
        {
            return await context.Workflow
                .AsNoTracking()
                .Include(x => x.WorkflowStorage)
                .Include(x => x.WorkflowActions)
                .ThenInclude(x => x.RequestRule)
                .ToArrayAsync();
        }

        public async Task<Workflow?> GetAsync(int id)
        {
            return await context.Workflow
                .AsNoTracking()
                .Include(x => x.WorkflowStorage)
                .Include(x => x.WorkflowActions)
                .ThenInclude(x => x.RequestRule)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Workflow workflow)
        {
            context.Workflow.Add(workflow);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workflow = await context.Workflow.FirstOrDefaultAsync(x => x.Id == id);
            if (workflow is not null)
            {
                context.Workflow.Remove(workflow);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Workflow workflow)
        {
            var currentWorkflow =
                await context.Workflow.FirstOrDefaultAsync(x => x.Id == workflow.Id)
                ?? throw new Exception("Request rule not found");

            currentWorkflow.Name = workflow.Name;

            await context.SaveChangesAsync();
        }
    }
}
