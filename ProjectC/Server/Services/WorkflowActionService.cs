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
                .Include(x => x.WorkflowTriggers)
                .ThenInclude(x => x.WebhookEvent)
                .ThenInclude(x => x!.WebhookRule)
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
            else
            {
                workflowAction.Path = null;
                workflowAction.PathRegex = null;
                workflowAction.Method = null;
            }

            context.WorkflowAction.Add(workflowAction);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workflowAction = await context.WorkflowAction.FirstOrDefaultAsync(x => x.Id == id);
            if (workflowAction is not null)
            {
                var requestEvents = await context.RequestEvent
                    .Where(x => x.WorkflowActionId == workflowAction.Id)
                    .ToListAsync();
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    if (requestEvents != null)
                    {
                        requestEvents.ForEach(x =>
                        {
                            x.WorkflowActionId = null;
                            x.RequestRuleId = null;
                        });
                        context.RequestEvent.UpdateRange(requestEvents);
                        await context.SaveChangesAsync();
                    }

                    context.WorkflowAction.Remove(workflowAction);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception("Invalid database operation", e);
                }
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
                currentWorkflowAction.Path = null;
                currentWorkflowAction.PathRegex = null;
                currentWorkflowAction.Method = null;
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
