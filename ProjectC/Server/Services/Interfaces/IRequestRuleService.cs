using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestRuleService
    {
        Task<IEnumerable<RequestRule>> GetAsync();
        Task CreateAsync(RequestRule request);
        Task UpdateAsync(RequestRule request);
    }
}
