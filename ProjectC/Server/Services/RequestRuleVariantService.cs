using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Utils;

namespace ProjectC.Server.Services
{
    public class RequestRuleVariantService : IRequestRuleVariantService
    {
        private readonly ProjectCDbContext context;

        public RequestRuleVariantService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RequestRuleVariant>> GetAsync()
        {
            return await context.RequestRuleVariant.Include(x => x.RequestRule).ToArrayAsync();
        }

        public async Task<IEnumerable<RequestRuleVariant>> GetByWorkflowIdAsync(int workflowId)
        {
            return await context.RequestRuleVariant
                .Where(x => x.WorkflowId == workflowId)
                .Include(x => x.RequestRule)
                .Include(x => x.RequestRuleTriggers)
                .ThenInclude(x => x.WebhookEvent)
                .ThenInclude(x => x!.WebhookRule)
                .ToArrayAsync();
        }

        public async Task CreateAsync(RequestRuleVariant requestRuleVariant)
        {
            if (requestRuleVariant.RequestRuleId is null || requestRuleVariant.RequestRuleId == 0)
            {
                if (
                    !string.IsNullOrEmpty(requestRuleVariant.Path)
                    && requestRuleVariant.Method is not null
                )
                {
                    requestRuleVariant.PathRegex = RequestUtils.BuildRegexPath(
                        requestRuleVariant.Path
                    );
                    requestRuleVariant.RequestRuleId = null;
                }
                else
                {
                    throw new Exception("Invalid data");
                }
            }
            else
            {
                requestRuleVariant.Path = null;
                requestRuleVariant.PathRegex = null;
                requestRuleVariant.Method = null;
            }

            context.RequestRuleVariant.Add(requestRuleVariant);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var requestRuleVariant = await context.RequestRuleVariant.FirstOrDefaultAsync(
                x => x.Id == id
            );
            if (requestRuleVariant is not null)
            {
                var requestEvents = await context.RequestEvent
                    .Where(x => x.RequestRuleVariantId == requestRuleVariant.Id)
                    .ToListAsync();
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    if (requestEvents != null)
                    {
                        requestEvents.ForEach(x =>
                        {
                            x.RequestRuleVariantId = null;
                            x.RequestRuleId = null;
                        });
                        context.RequestEvent.UpdateRange(requestEvents);
                        await context.SaveChangesAsync();
                    }

                    context.RequestRuleVariant.Remove(requestRuleVariant);
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

        public async Task UpdateAsync(RequestRuleVariant requestRuleVariant)
        {
            var currentRequestRuleVariant =
                await context.RequestRuleVariant.FirstOrDefaultAsync(
                    x => x.Id == requestRuleVariant.Id
                ) ?? throw new Exception("Request rule not found");

            if (requestRuleVariant.RequestRuleId > 0)
            {
                currentRequestRuleVariant.RequestRuleId = requestRuleVariant.RequestRuleId;
                currentRequestRuleVariant.Path = null;
                currentRequestRuleVariant.PathRegex = null;
                currentRequestRuleVariant.Method = null;
            }
            else
            {
                if (
                    !string.IsNullOrEmpty(requestRuleVariant.Path)
                    && requestRuleVariant.Method is not null
                )
                {
                    currentRequestRuleVariant.Path = requestRuleVariant.Path;
                    currentRequestRuleVariant.Method = requestRuleVariant.Method;
                    currentRequestRuleVariant.PathRegex = RequestUtils.BuildRegexPath(
                        requestRuleVariant.Path
                    );
                    currentRequestRuleVariant.RequestRuleId = null;
                }
                else
                {
                    throw new Exception("Invalid data");
                }
            }

            currentRequestRuleVariant.Description = requestRuleVariant.Description;
            currentRequestRuleVariant.ResponseStatus = requestRuleVariant.ResponseStatus;
            currentRequestRuleVariant.ResponseDelay = requestRuleVariant.ResponseDelay;
            currentRequestRuleVariant.ResponseHeaders = requestRuleVariant.ResponseHeaders;
            currentRequestRuleVariant.ResponseBody = requestRuleVariant.ResponseBody;

            await context.SaveChangesAsync();
        }
    }
}
