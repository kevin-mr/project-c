using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;

namespace ProjectC.Server.Services
{
    public class WorkflowActionService : IWorkflowActionService
    {
        private readonly ProjectCDbContext context;

        public WorkflowActionService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WorkflowAction>> GetAsync()
        {
            return await context.WorkflowAction.ToArrayAsync();
        }

        public async Task<IEnumerable<WorkflowAction>> GetByWorkflowIdAsync(int workflowId)
        {
            return await context.WorkflowAction
                .Where(x => x.WorkflowId == workflowId)
                .ToArrayAsync();
        }

        public async Task CreateAsync(WorkflowAction workflowAction)
        {
            context.WorkflowAction.Add(workflowAction);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workflowAction = await context.WorkflowAction.FirstOrDefaultAsync(x => x.Id == id);
            if (workflowAction is not null)
            {
                context.WorkflowAction.Remove(workflowAction);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(WorkflowAction workflowAction)
        {
            var currentWorkflowAction =
                await context.WorkflowAction.FirstOrDefaultAsync(x => x.Id == workflowAction.Id)
                ?? throw new Exception("Request rule not found");

            currentWorkflowAction.Name = workflowAction.Name;
            currentWorkflowAction.ResponseStatus = workflowAction.ResponseStatus;
            currentWorkflowAction.ResponseDelay = workflowAction.ResponseDelay;
            currentWorkflowAction.ResponseBody = workflowAction.ResponseBody;
            currentWorkflowAction.ResponseHeaders = workflowAction.ResponseHeaders;

            if (workflowAction.RequestRuleId > 0)
            {
                currentWorkflowAction.RequestRuleId = workflowAction.RequestRuleId;
            }

            await context.SaveChangesAsync();
        }
    }
}
