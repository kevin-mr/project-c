using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
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

        public async Task CreateAsync(RequestRule request)
        {
            context.RequestRule.Add(request);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RequestRule request)
        {
            var currentRequestRule =
                await context.RequestRule.FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("Request rule not found");

            currentRequestRule.Path = request.Path;
            currentRequestRule.Method = request.Method;
            currentRequestRule.ResponseBody = request.ResponseBody;

            await context.SaveChangesAsync();
        }
    }
}
