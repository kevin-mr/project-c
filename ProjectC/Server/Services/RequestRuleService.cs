using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;
using ProjectC.Server.Services.Interfaces;

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

        public async Task<IEnumerable<RequestRuleMethodCounter>> GetMethodCountersAsync()
        {
            return await context.RequestRule
                .GroupBy(x => x.Method)
                .Select(x => new RequestRuleMethodCounter { Method = x.Key, Counter = x.Count() })
                .ToArrayAsync();
        }

        public async Task CreateAsync(RequestRule requestRule)
        {
            context.RequestRule.Add(requestRule);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RequestRule requestRule)
        {
            var currentRequestRule =
                await context.RequestRule.FirstOrDefaultAsync(x => x.Id == requestRule.Id)
                ?? throw new Exception("Request rule not found");

            currentRequestRule.Path = requestRule.Path;
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
                context.RequestRule.Remove(requestRule);
                await context.SaveChangesAsync();
            }
        }
    }
}
