using Microsoft.EntityFrameworkCore;
using ProjectC.Client.Pages;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Utils;

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
            return await context.WorkflowAction.Include(x => x.RequestRule).ToArrayAsync();
        }

        public async Task<IEnumerable<WorkflowAction>> GetByWorkflowIdAsync(int workflowId)
        {
            return await context.WorkflowAction
                .Where(x => x.WorkflowId == workflowId)
                .Include(x => x.RequestRule)
                .ToArrayAsync();
        }

        public async Task CreateAsync(WorkflowAction workflowAction)
        {
            if (workflowAction.RequestRuleId is null || workflowAction.RequestRuleId == 0)
            {
                if (!string.IsNullOrEmpty(workflowAction.Path) && workflowAction.Method is not null)
                {
                    workflowAction.PathRegex = RequestUtils.BuildRegexPath(workflowAction.Path);
                    workflowAction.RequestRuleId = null;
                }
                else
                {
                    throw new Exception("Invalid data");
                }
            }

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

            if (workflowAction.RequestRuleId > 0)
            {
                currentWorkflowAction.RequestRuleId = workflowAction.RequestRuleId;
            }
            else
            {
                if (!string.IsNullOrEmpty(workflowAction.Path) && workflowAction.Method is not null)
                {
                    currentWorkflowAction.Path = workflowAction.Path;
                    currentWorkflowAction.Method = workflowAction.Method;
                    currentWorkflowAction.PathRegex = RequestUtils.BuildRegexPath(
                        workflowAction.Path
                    );
                    currentWorkflowAction.RequestRuleId = null;
                }
                else
                {
                    throw new Exception("Invalid data");
                }
            }

            currentWorkflowAction.Description = workflowAction.Description;
            currentWorkflowAction.ResponseStatus = workflowAction.ResponseStatus;
            currentWorkflowAction.ResponseDelay = workflowAction.ResponseDelay;
            currentWorkflowAction.ResponseHeaders = workflowAction.ResponseHeaders;
            currentWorkflowAction.ResponseBody = workflowAction.ResponseBody;

            await context.SaveChangesAsync();
        }
    }
}
