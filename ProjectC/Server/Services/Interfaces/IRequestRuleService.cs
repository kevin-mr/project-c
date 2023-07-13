using ProjectC.Server.Data.Entities;
using ProjectC.Server.Models;

namespace ProjectC.Server.Services.Interfaces
{
    public interface IRequestRuleService
    {
        Task<IEnumerable<RequestRule>> GetAsync();
        Task<IEnumerable<RequestRuleMethodCounter>> GetMethodCountersAsync();
        Task CreateAsync(RequestRule request);
        Task UpdateAsync(RequestRule request);
        Task DeleteAsync(int id);
    }
}
