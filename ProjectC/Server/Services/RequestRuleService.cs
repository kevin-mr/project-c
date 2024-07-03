using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;
using ProjectC.Server.Services.Interfaces;
using ProjectC.Server.Utils;

namespace ProjectC.Server.Services
{
    public class RequestRuleService : IRequestRuleService
    {
        private readonly ProjectCDbContext context;

        public RequestRuleService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RequestRule>> GetAsync()
        {
            return await context.RequestRule.ToArrayAsync();
        }

        public async Task<RequestRule?> GetByIdAsync(int id)
        {
            return await context.RequestRule
                .Include(x => x.RequestRuleEvents)
                .Include(x => x.RequestRuleVariants)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<RequestRuleMethodCounter>> GetMethodCountersAsync()
        {
            return await context.RequestRule
                .GroupBy(x => x.Method)
                .Select(x => new RequestRuleMethodCounter { Method = x.Key, Counter = x.Count() })
                .ToArrayAsync();
        }

        public async Task CreateAsync(RequestRule requestRule)
        {
            requestRule.PathRegex = RequestUtils.BuildRegexPath(requestRule.Path);
            context.RequestRule.Add(requestRule);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RequestRule requestRule)
        {
            var currentRequestRule =
                await context.RequestRule.FirstOrDefaultAsync(x => x.Id == requestRule.Id)
                ?? throw new Exception("Request rule not found");

            currentRequestRule.Path = requestRule.Path;
            currentRequestRule.PathRegex = RequestUtils.BuildRegexPath(requestRule.Path);
            currentRequestRule.Method = requestRule.Method;
            currentRequestRule.ResponseStatus = requestRule.ResponseStatus;
            currentRequestRule.ResponseHeaders = requestRule.ResponseHeaders;
            currentRequestRule.ResponseDelay = requestRule.ResponseDelay;
            currentRequestRule.ResponseBody = requestRule.ResponseBody;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var requestRule = await context.RequestRule.FirstOrDefaultAsync(x => x.Id == id);
            if (requestRule is not null)
            {
                var requestRuleEvents = await context.RequestEvent
                    .Include(x => x.RequestRule)
                    .Where(
                        x =>
                            x.RequestRuleId != null
                            && x.RequestRuleId == requestRule.Id
                            && x.RequestRuleVariantId == null
                    )
                    .ToListAsync();
                var requestRuleVariants = await context.RequestRuleVariant
                    .Include(x => x.RequestRule)
                    .Where(x => x.RequestRuleId != null && x.RequestRuleId == requestRule.Id)
                    .ToListAsync();

                using var transaction = context.Database.BeginTransaction();
                try
                {
                    if (requestRuleEvents != null)
                    {
                        requestRuleEvents.ForEach(x => x.RequestRuleId = null);
                        context.RequestEvent.UpdateRange(requestRuleEvents);
                        await context.SaveChangesAsync();
                    }
                    if (requestRuleVariants != null)
                    {
                        requestRuleVariants.ForEach(x =>
                        {
                            if (x.RequestRule != null)
                            {
                                x.Path = x.RequestRule.Path;
                                x.Method = x.RequestRule.Method;
                                x.PathRegex = RequestUtils.BuildRegexPath(x.RequestRule.Path);
                            }
                            x.RequestRuleId = null;
                        });
                        context.RequestRuleVariant.UpdateRange(requestRuleVariants);
                        await context.SaveChangesAsync();
                    }
                    context.RequestRule.Remove(requestRule);
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
    }
}
